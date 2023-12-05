using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    /// <summary>
    /// Controller for storing and retrieving settings data from the JSON file and
    /// sending the same to the hardware
    /// </summary>
    [Route("settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IHubContext<TemperatureHub> _hubContext;
        private readonly ISerialPortService _serialPortService;

        /// <summary>
        /// Initializes a new instance of the SettingsController class.
        /// </summary>
        /// <param name="hubContext">The SignalR hub context for real-time communication.</param>
        /// <param name="serialPortService">The service providing access to the serial port.</param>
        public SettingsController(IHubContext<TemperatureHub> hubContext, ISerialPortService serialPortService)
        {
            _serialPortService = serialPortService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Retrieves the current temperature control settings.
        /// </summary>
        /// <returns>The current temperature control settings.</returns>
        [HttpGet]
        public async Task<Settings> Get()
        {
            string fileName = @"..\..\settingsFile.json";
            using FileStream openStream = System.IO.File.OpenRead(fileName);
            Settings? settingsValue =
                await JsonSerializer.DeserializeAsync<Settings>(openStream);

            return settingsValue;
        }

        /// <summary>
        /// Updates the temperature control settings to the hardware and stores
        /// it in the JSON file based on the provided JSON data.
        /// </summary>
        /// <param name="s">The JSON object containing updated settings from the frontend</param>
        // POST api/<SettingsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonObject s)
        {
            var newSettings = new Settings();
            newSettings.Threshold = Convert.ToDouble(s["Threshold"]?.ToString());
            newSettings.DataAcquisitionRate = Convert.ToInt32(s["DataAcquisitionRate"]?.ToString());
            newSettings.Temperature_4mA = Convert.ToDouble(s["Temperature_4mA"]?.ToString());
            newSettings.Temperature_20mA = Convert.ToDouble(s["Temperature_20mA"]?.ToString());
            newSettings.Color_Range_1 = (Colors)Enum.Parse(typeof(Colors), Convert.ToString(s["Color_0_15"]));
            newSettings.Color_Range_2 = (Colors)Enum.Parse(typeof(Colors), Convert.ToString(s["Color_16_30"]));
            newSettings.Color_Range_3 = (Colors)Enum.Parse(typeof(Colors), Convert.ToString(s["Color_31_45"]));
            StringBuilder sendString = new StringBuilder("SET CON LED:");
            sendString.Append($"{(char)newSettings.Color_Range_1}{(char)newSettings.Color_Range_2}{(char)newSettings.Color_Range_3}");
            sendString.Append($",OL:{s["Temperature_4mA"]},OH:{s["Temperature_20mA"]}\r");

            string jsonString = JsonSerializer.Serialize<Settings>(newSettings);
            System.IO.File.WriteAllText(@"..\..\settingsFile.json", jsonString);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sendString.ToString());
                _serialPortService.WriteToPort(bytes);
                return Ok(new { message = "Settings updated successfully" });
            }
            catch (OperationCanceledException ex)
            {
                await _hubContext.Clients.All.SendAsync("DeviceError", new { Error = "Device disconnected" });
                Console.WriteLine($"Operation canceled: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                await _hubContext.Clients.All.SendAsync("DeviceError", new { Error = "Device disconnected" });
                Console.WriteLine($"Invalid operation: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Ok(new { message = "Settings updation failed!! please retry" });
        }
    }
}
