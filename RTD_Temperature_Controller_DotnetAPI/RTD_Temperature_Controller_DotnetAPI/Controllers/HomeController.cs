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
    /// Controller responsible for requesting the temperature data from hardware
    /// </summary>

    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly SerialPort _serialPort;
        private readonly IHubContext<TemperatureHub> _hubContext;
        public HomeController(ISerialPortService serialPortService, IHubContext<TemperatureHub> hubcontext)
        {
            _serialPort = serialPortService.SerialPort;
            _hubContext = hubcontext;
        }

        /// <summary>
        /// Sends command to hardware
        /// </summary>
        /// <param name="value">>JSON object containing the command to be sent</param>
        /// <returns>NIL</returns>

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
