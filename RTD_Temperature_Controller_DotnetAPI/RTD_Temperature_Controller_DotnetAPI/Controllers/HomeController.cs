using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
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
        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController>
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
            return true;
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
