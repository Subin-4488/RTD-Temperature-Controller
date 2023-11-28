using Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class DataService : IDataService
    {
        private readonly IHubContext<TemperatureHub> _hubContext;
        private readonly IConfiguration _configuration;

        public DataService(IHubContext<TemperatureHub> hubContext, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
        }
        public async void ReadDataFromHardware(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort spL = (SerialPort)sender;
            if (!spL.IsOpen)
            {
                return;
            }
            try
            {
                string[] resultArr = new string[6];
                try
                {
                    string result = spL.ReadTo("\r");
                    resultArr = result.Split(' ');
                    Console.WriteLine(result);
                }
                catch (OperationCanceledException ex)
                {
                    // Log or handle the cancellation exception
                    await _hubContext.Clients.All.SendAsync("DeviceError",new { Error = "Device disconnected"});
                    Console.WriteLine($"Operation canceled: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    await _hubContext.Clients.All.SendAsync("DeviceError", new { Error = "Device disconnected" });
                    Console.WriteLine($"Invalid operation: {ex.Message}");
                }

                if (resultArr[0] == "OK" && resultArr[1] == "TMPA")
                {
                    var data = new Data { Temperature = Convert.ToDouble(resultArr[2]), Time = DateTime.Now };
                    await _hubContext.Clients.All.SendAsync("UpdateTemperature", data);

                    //db
                    var flag = await WriteToDatabase(data);
                    //await Console.Out.WriteLineAsync(flag.Item2);
                    //await _dbContext.TemperatureTable.AddAsync(data);
                    //await _dbContext.SaveChangesAsync();

                }
                else if (resultArr[0] == "OK" && resultArr[1] == "CON")
                {
                    string[] properties = resultArr[2].Split(',');

                    Settings? settingsValue;
                    string fileName = @"..\..\settingsFile.json";
                    
                    using (FileStream openStream = System.IO.File.OpenRead(fileName))
                    {
                        settingsValue = await JsonSerializer.DeserializeAsync<Settings>(openStream);
                    }

                    var newSettings = new Settings();
                    newSettings.Threshold = settingsValue.Threshold;
                    newSettings.DataAcquisitionRate = settingsValue.DataAcquisitionRate;

                    foreach (var item in properties)
                    {
                        var d = item.Split(':');

                        if (d[0] == "LED")
                        {
                            string s = d[1];
                            newSettings.Color_0_15 = (Colors)Enum.Parse(typeof(Colors), GetColorCode(s[0]));
                            newSettings.Color_16_30 = (Colors)Enum.Parse(typeof(Colors), GetColorCode(s[1]));
                            newSettings.Color_31_45 = (Colors)Enum.Parse(typeof(Colors), GetColorCode(s[2]));
                        }
                        else if (d[0] == "OL")
                        {
                            newSettings.Temperature_4mA = Convert.ToDouble(d[1]);
                        }
                        else if (d[0] == "OH")
                        {
                            newSettings.Temperature_20mA = Convert.ToDouble(d[1]);
                        }
                    }
                    string jsonString = JsonSerializer.Serialize<Settings>(newSettings);
                    System.IO.File.WriteAllText(@"..\..\settingsFile.json", jsonString);
                }
                else //manual mode
                {
                    if (resultArr[0] == "OK" && resultArr[1] == "TMPM")
                    {
                        var data = new ManualModeData { Response = "OK TMPM", value = resultArr[2] };
                        await _hubContext.Clients.All.SendAsync("ManualModeData", data);
                    }
                    else if (resultArr[0] == "OK" && resultArr[1] == "RES")
                    {
                        var data = new ManualModeData { Response = "OK RES", value = resultArr[2] };
                        await _hubContext.Clients.All.SendAsync("ManualModeData", data);
                    }
                    else if (resultArr[0] == "OK" && resultArr[1] == "EPR")
                    {
                        var data = new ManualModeData { Response = "OK EPR", value = "OK EPR" };
                        await _hubContext.Clients.All.SendAsync("ManualModeData", data);
                    }
                    else if (resultArr[0] == "OK" && resultArr[1] == "MOD")
                    {
                        var data = new ManualModeData { Response = "OK MOD", value = "OK MOD" };
                        await _hubContext.Clients.All.SendAsync("ManualModeData", data);
                    }
                    else if (resultArr[0] == "OK" && resultArr[1] == "BTN")
                    {
                        var data = new ManualModeData { Response = "OK BTN", value = $"{resultArr[0]} {resultArr[1]} {resultArr[2]} {resultArr[3]}" };
                        await _hubContext.Clients.All.SendAsync("ManualModeData", data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<(bool, string)> WriteToDatabase(Data data)
        {
            var dbContextOptions = new DbContextOptionsBuilder<RTDSensorDBContext>()
                                        .UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"])
                                        .Options;
            using (var _dbContext = new RTDSensorDBContext(dbContextOptions))
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

        private string GetColorCode(char c)
        {
            switch (c)
            {
                case 'R':
                    return "red";
                case 'G':
                    return "green";
                case 'B':
                    return "blue";
            }
            return "";
        }
    }
}
