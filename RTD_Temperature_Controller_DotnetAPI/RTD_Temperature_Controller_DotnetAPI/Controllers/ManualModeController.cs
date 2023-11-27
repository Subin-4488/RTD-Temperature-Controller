using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using System.IO.Ports;
using System.Text;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("manualmode")]
    [ApiController]
    public class ManualModeController : ControllerBase
    {
        private readonly SerialPort _serialPort;
        private readonly IHubContext<TemperatureHub> _hubContext;
        public ManualModeController(ISerialPortService serialPortService, IHubContext<TemperatureHub> hubContext)
        {
            _serialPort = serialPortService.SerialPort;
            _hubContext = hubContext;
        }

        // POST api/<ManualModeController>
        [HttpPost]
        public async void Post([FromBody] JsonObject value)
        {
            Console.WriteLine(value);
            byte[] bytes = Encoding.UTF8.GetBytes(value["Value"].ToString());
            Console.WriteLine(bytes);
            string hexString = Convert.ToHexString(bytes);
            Console.WriteLine(hexString);
            try
            {
                _serialPort.Write(bytes, 0, bytes.Length);
            }
            catch (OperationCanceledException ex)
            {
                // Log or handle the cancellation exception
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
            //return true;
        }
    }
}
