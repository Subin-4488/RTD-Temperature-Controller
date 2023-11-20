using Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataService : IDataService
    {
        private readonly RTDSensorDBContext _dbContext;
        private readonly IHubContext<TemperatureHub> _hubContext;

        public DataService(RTDSensorDBContext dBContext, IHubContext<TemperatureHub> hubContext)
        {
            _hubContext = hubContext;   
        }
        public async void ReadDataFromHardware(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort spL = (SerialPort)sender;
            //Do the parsing and write to database
            //await WriteToDatabase(new Data() { Temperature = 0, Time = DateTime.Now });

            string result = spL.ReadExisting();
            result = result.Substring(0, result.Length - 1);
            string[] resultArr  = result.Split(' ');
            Console.WriteLine(spL.ReadExisting());

            if (resultArr[0]=="OK" && resultArr[1]=="TMP")
            {
                var data = new Data { Temperature = Convert.ToDouble(resultArr[2]), Time = DateTime.Now };
                await _hubContext.Clients.All.SendAsync("UpdateTemperature", data);
               
            }
            else if (resultArr[0] == "OK" && resultArr[1]=="MAN")
            {
                if (resultArr[2] == "TMP")
                {
                    var data = new ManualModeData { Response = "OK MAN TMP", value = resultArr[3] };
                    await _hubContext.Clients.All.SendAsync("manualmodedata", data);
                }
                else if (resultArr[2] == "RES")
                {
                    var data = new ManualModeData { Response = "OK MAN RES", value = resultArr[3] };
                    await _hubContext.Clients.All.SendAsync("manualmodedata", data);
                }
                else //for EEPROM and SET PWM
                {
                    var data = new ManualModeData { Response = resultArr[0]+" " + resultArr[1], value = resultArr[0] + " " + resultArr[1] };
                    await _hubContext.Clients.All.SendAsync("manualmodedata", data);
                }
            }

        }

        public async Task<(bool, string)> WriteToDatabase(Data data)
        {
            if (_dbContext.TemperatureTable == null)
            {
                return (false, "Entity set 'RTDSensorDBContext.TemperatureTable'  is null.");
            }
            _dbContext.TemperatureTable.Add(data);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                var d = await _dbContext.TemperatureTable.Where(d => d.Time == data.Time).ToListAsync();
                if (d.Count > 0)
                {
                    return (false, "Data already exist");
                }
                else
                {
                    throw;
                }
            }

            return (true, "Successfully added to database");
        }
    }
}
