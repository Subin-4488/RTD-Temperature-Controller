using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using Serilog;
using System.IO.Ports;
using System.Text;
using System.Text.Json.Nodes;

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    /// <summary>
    /// Controller for handling manual mode operations. 
    /// </summary>

    [Route("manualmode")]
    [ApiController]
    public class ManualModeController : ControllerBase
    {
        private readonly SerialPort _serialPort;
        private readonly IHubContext<TemperatureHub> _hubContext;

        /// <summary>
        /// Constructor for ManualModeController.
        /// </summary>
        /// <param name="serialPortService">The service providing the SerialPort instance</param>
        /// <param name="hubContext">The SignalR hub context for TemperatureHub</param>

        public ManualModeController(ISerialPortService serialPortService, IHubContext<TemperatureHub> hubContext)
        {
            _serialPort = serialPortService.SerialPort;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Handles HTTP POST requests for manual mode control.
        /// </summary>
        /// <param name="value"></param>

        [HttpPost]
        public async Task<bool> Post([FromBody] JsonObject value)
        {
            Console.WriteLine(value);
            byte[] bytes = Encoding.UTF8.GetBytes(value["Value"].ToString());
            Console.WriteLine(bytes);
            string hexString = Convert.ToHexString(bytes);
            Console.WriteLine(hexString);
            try
            {
                _serialPort.Write(bytes, 0, bytes.Length);
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
