using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using Serilog;
using Services;
using System.IO.Ports;
using System.Text;
using System.Text.Json.Nodes;


namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{

    /// <summary>
    /// Controller responsible for requesting the temperature data from hardware
    /// </summary>

    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISerialPortService _serialPortService;
        private readonly IHubContext<TemperatureHub> _hubContext;

        /// <summary>
        /// Constructor for HomeController.
        /// </summary>
        /// <param name="serialPortService">Service for managing the serial port</param>
        /// <param name="hubcontext">SignalR hub context for real-time communication</param>

        public HomeController(ISerialPortService serialPortService, IHubContext<TemperatureHub> hubcontext)
        {
            _serialPortService = serialPortService;
            _hubContext = hubcontext;
        }

        /// <summary>
        /// Sends command to hardware
        /// </summary>
        /// <param name="value">>JSON object containing the command to be sent</param>

        [HttpPost]
        public async Task<bool> Post([FromBody] JsonObject value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value["Value"].ToString());
            string hexString = Convert.ToHexString(bytes);
            Console.WriteLine(hexString);
            try
            {
                _serialPortService.WriteToPort(bytes);
                return true;
            }
            catch (OperationCanceledException ex)
            {
                await _hubContext.Clients.All.SendAsync("DeviceError", new { Error = "Device disconnected" });
                Log.Information($"Operation canceled: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                await _hubContext.Clients.All.SendAsync("DeviceError", new { Error = "Device disconnected" });
                Log.Information($"Invalid operation: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log.Information($"{ex.Message}");
            }
            return false;
        }
    }
}
