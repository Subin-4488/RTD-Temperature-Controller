using Contracts;
using Microsoft.AspNetCore.Mvc;
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
        public ManualModeController(ISerialPortService serialPortService)
        {
            _serialPort = serialPortService.SerialPort;
        }

        // POST api/<ManualModeController>
        [HttpPost]
        public bool Post([FromBody] JsonObject value)
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
            catch(Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return true;
        }
    }
}
