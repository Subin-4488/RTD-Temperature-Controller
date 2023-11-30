using Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using Serilog;
using System.IO.Ports;
using System.Text.Json;

namespace Services
{
    /// <summary>
    /// Service for handling and processing data from hardware devices.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly IHubContext<TemperatureHub> _hubContext;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the DataService class.
        /// </summary>
        /// <param name="hubContext">The SignalR hub context for temperature updates.</param>
        /// <param name="configuration">The configuration for the application.</param>

        public DataService(IHubContext<TemperatureHub> hubContext, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Reads data from a hardware device and processes it.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments containing information about the serial data received.</param>

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
                }
                catch (OperationCanceledException ex)
                {
                    await SendDeviceError("Device disconnected");
                    Log.Information($"Operation canceled: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    await SendDeviceError("Device disconnected");
                    Log.Information($"Invalid operation: {ex.Message}");
                }
                processTemperatureData(resultArr);
                processSettingsData(resultArr);
                processManualModeData(resultArr);

            }
            catch (Exception ex)
            {
                Log.Information($"{ex.Message}");
            }

        }
        /// <summary>
        /// Processes temperature data and updates connected clients.
        /// </summary>
        /// <param name="resultArr">An array containing parsed data from the hardware.</param>
        private async void processTemperatureData(string[] resultArr)
        {
            if (resultArr[0] == "OK" && resultArr[1] == "TMPA")
            {
                var data = new Data { Temperature = Convert.ToDouble(resultArr[2]), Time = DateTime.Now };
                await sendTemperature(data);
                var flag = await WriteToDatabase(data);
            }
        }

        /// <summary>
        /// Processes settings data received from the hardware.
        /// </summary>
        /// <param name="resultArr">An array containing parsed data from the hardware.</param>
        private void processSettingsData(string[] resultArr)
        {
            if (resultArr[0] == "OK" && resultArr[1] == "CON" && resultArr.Length > 2)
            {
                var newSettings = parseSettingsData(resultArr[2]);
                saveSettingsToFile(newSettings);
            }
        }

        /// <summary>
        /// Processes manual mode data received from the hardware.
        /// </summary>
        /// <param name="resultArr">An array containing parsed data from the hardware.</param>
        private async void processManualModeData(string[] resultArr)
        {
            if (resultArr[0] == "OK" && (resultArr[1] == "TMPM" || resultArr[1] == "RES"))
            {
                var data = new ManualModeData { Response = $"OK {resultArr[1]}", value = resultArr[2] };
                await sendManualModeData(data);
            }
            else if (resultArr[0] == "OK" && (resultArr[1] == "EPR" || resultArr[1] == "MOD"))
            {
                var data = new ManualModeData { Response = $"OK {resultArr[1]}", value = $"OK {resultArr[1]}" };
                await sendManualModeData(data);
            }
            else if (resultArr[0] == "OK" && resultArr[1] == "BTN")
            {
                var data = new ManualModeData { Response = "OK BTN", value = $"{resultArr[0]} {resultArr[1]} {resultArr[2]} {resultArr[3]}" };
                await sendManualModeData(data);
            }
            else if (resultArr[0] == "OK" && resultArr[1] == "DTY")
            {
                var data = new ManualModeData { Response = "OK DTY", value = $"{resultArr[0]} {resultArr[1]}" };
                await sendManualModeData(data);
            }
        }

        /// <summary>
        /// Sends temperature data to all connected clients via the SignalR hub.
        /// </summary>
        /// <param name="data">The Data object containing temperature information.</param>

        private async Task sendTemperature(Data data)
        {
            await _hubContext.Clients.All.SendAsync("UpdateTemperature", data);
        }

        /// <summary>
        /// Sends manual mode data to all connected clients via the SignalR hub.
        /// </summary>
        /// <param name="data">The ManualModeData object containing manual mode information.</param>

        private async Task sendManualModeData(ManualModeData data)
        {
            await _hubContext.Clients.All.SendAsync("ManualModeData", data);
        }

        /// <summary>
        /// Sends device error information to all connected clients via the SignalR hub.
        /// </summary>
        /// <param name="error">The error message indicating the device error.</param>

        private async Task SendDeviceError(string error)
        {
            await _hubContext.Clients.All.SendAsync("DeviceError", new { Error = error });
        }

        /// <summary>
        /// Parses configuration data received from the hardware.
        /// </summary>
        /// <param name="configData">The raw configuration data string.</param>
        /// <returns>A new Settings object based on the parsed configuration data.</returns>
        private Settings parseSettingsData(string configData)
        {
            string[] properties = configData.Split(',');

            Settings? settingsValue;
            string fileName = @"..\..\settingsFile.json";

            using (FileStream openStream = System.IO.File.OpenRead(fileName))
            {
                settingsValue = JsonSerializer.DeserializeAsync<Settings>(openStream).Result;
            }

            var newSettings = new Settings
            {
                Threshold = settingsValue.Threshold,
                DataAcquisitionRate = settingsValue.DataAcquisitionRate
            };

            foreach (var item in properties)
            {
                var d = item.Split(':');

                if (d[0] == "LED")
                {
                    string s = d[1];
                    newSettings.Color_0_15 = (Colors)Enum.Parse(typeof(Colors), getColorCode(s[0]));
                    newSettings.Color_16_30 = (Colors)Enum.Parse(typeof(Colors), getColorCode(s[1]));
                    newSettings.Color_31_45 = (Colors)Enum.Parse(typeof(Colors), getColorCode(s[2]));
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
            return newSettings;
        }

        /// <summary>
        /// Saves updated settings to a JSON file.
        /// </summary>
        /// <param name="settings">The Settings object to be saved.</param>
        private void saveSettingsToFile(Settings settings)
        {
            string jsonString = JsonSerializer.Serialize(settings);
            System.IO.File.WriteAllText(@"..\..\settingsFile.json", jsonString);
        }
        /// <summary>
        /// Writes temperature data to the database.
        /// </summary>
        /// <param name="data">The Data object containing temperature information.</param>
        /// <returns>A tuple indicating the success status and a message.</returns>
        public async Task<(bool, string)> WriteToDatabase(Data data)
        {
            var dbContextOptions = new DbContextOptionsBuilder<RTDSensorDBContext>()
                                        .UseSqlServer(_configuration["ConnectionStrings:LocalConnection"])
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
                        Log.Information($"Data already exist");
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

        /// <summary>
        /// Retrieves the color code associated with a given character.
        /// </summary>
        /// <param name="c">The input character representing a color.</param>
        /// <returns>The color code corresponding to the input character.
        /// Returns an empty string if the character is not recognized.
        /// </returns>
        private string getColorCode(char c)
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
