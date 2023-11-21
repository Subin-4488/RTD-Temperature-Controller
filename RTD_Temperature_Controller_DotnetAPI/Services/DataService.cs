using Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;

namespace Services
{
    public class DataService : IDataService
    {
        private readonly RTDSensorDBContext _dbContext;
        private readonly IHubContext<TemperatureHub> _hubContext;

        public DataService(RTDSensorDBContext dBContext, IHubContext<TemperatureHub> hubContext)
        {
            _hubContext = hubContext;
            _dbContext = dBContext;
        }
        public async void ReadDataFromHardware(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort spL = (SerialPort)sender;
            //Do the parsing and write to database
            if (!spL.IsOpen)
            {
                return;
            }
            string result = spL.ReadTo("\r");
            //result = result.Substring(0, result.Length - 1);
            string[] resultArr = result.Split(' ');
            Console.WriteLine(result);
             
            if (resultArr[0] == "OK" && resultArr[1] == "TMPA")
            {
                var data = new Data { Temperature = Convert.ToDouble(resultArr[2]), Time = DateTime.Now };
                await _hubContext.Clients.All.SendAsync("UpdateTemperature", data);

                //db
                //var flag = await WriteToDatabase(data);
                //await Console.Out.WriteLineAsync(flag.Item2);
                await _dbContext.TemperatureTable.AddAsync(data);
                await _dbContext.SaveChangesAsync();

            }
            else if (resultArr[0] == "OK" && resultArr[1] == "CON")
            {
                string[] properties = resultArr[2].Split(',');
                foreach (var item in properties)
                {
                    var d = item.Split(':');
                    if (d[0] == "LED")
                    {

                    }
                    else if (d[0] == "OL")
                    {

                    }
                    else if (d[0] == "OH")
                    {

                    }
                }
            }
            else  //manual mode
            {
                if (resultArr[0] == "OK" && resultArr[2] == "TMPM")
                {
                    var data = new ManualModeData { Response = "OK TMPM", value = resultArr[2] };
                    await _hubContext.Clients.All.SendAsync("manualmodedata", data);
                }
                else if (resultArr[0] == "OK" && resultArr[2] == "RES")
                {
                    var data = new ManualModeData { Response = "OK RES", value = resultArr[2] };
                    await _hubContext.Clients.All.SendAsync("manualmodedata", data);
                }
                else if (resultArr[0] == "OK" && resultArr[2] == "EPR")
                {
                    var data = new ManualModeData { Response = "OK EPR", value = "OK EPR" };
                    await _hubContext.Clients.All.SendAsync("manualmodedata", data);
                }
                else if (resultArr[0] == "OK" && resultArr[2] == "MOD")
                {
                    var data = new ManualModeData { Response = "OK MOD", value = "OK MOD" };
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
            await _dbContext.TemperatureTable.AddAsync(data);
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
