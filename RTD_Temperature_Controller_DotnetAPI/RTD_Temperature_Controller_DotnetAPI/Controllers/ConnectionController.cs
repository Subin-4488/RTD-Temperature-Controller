using Contracts;
using Microsoft.AspNetCore.Mvc;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("connection")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly SerialPort _serialPort;

        public static void ReadDataFromHardware(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort spL = (SerialPort)sender;
            //write handling code
            //Console.WriteLine(spL.ReadExisting());
            StringBuilder stringBuilder = new StringBuilder();
            for (string i = spL.ReadExisting(); i.Length > 0; i = spL.ReadExisting())
            {
                stringBuilder.Append(i);
            }
            Console.WriteLine(stringBuilder.ToString());
        }

        public ConnectionController(ISerialPortService serialPortService)
        {
            this._serialPort = serialPortService.SerialPort;
            
            if (this._serialPort.IsOpen) this._serialPort.Close();

        }

        //GET: api/<ConnectionController>
        [HttpGet("ports")]
        public IEnumerable<string> Get()
        {
            //return new string[] { "COM1", "COM2" };
            return SerialPort.GetPortNames();
        }

        // POST api/<ConnectionController>
        [HttpPost]
        public bool Post([FromBody] JsonObject value)
        {
            //object objtemp = value["BitsPerSecond"];
            try
            {
                //Program.SerialPort.PortName = value["PortName"]!.ToString();
                this._serialPort.PortName = Convert.ToString(value["PortName"]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            //Program.SerialPort.PortName = "COM5";
            //Program.SerialPort.PortName = Convert.ToString(value["PortName"]);
            this._serialPort.BaudRate = int.Parse(value["BitsPerSecond"]!.ToString());
            
            switch (value["Parity"]!.ToString())
            {
                case "Even":
                    this._serialPort.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "Odd":
                    this._serialPort.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "Mark":
                    this._serialPort.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case "Space":
                    this._serialPort.Parity = System.IO.Ports.Parity.Space;
                    break;
                case "None":
                    this._serialPort.Parity = System.IO.Ports.Parity.None;
                    break;
                default:
                    throw new Exception("BAD Parity format");
            }

            this._serialPort.DataBits = int.Parse(value["DataBits"]!.ToString());


            switch (value["StopBits"]!.ToString())
            {
                case "1":
                    this._serialPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    this._serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    this._serialPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    throw new Exception("BAD Parity format");
            }

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(ReadDataFromHardware);

            try
            {
                _serialPort.Open();
                return true;
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        [HttpPost("disconnect")]
        public bool Post()
        {
            try
            {
                _serialPort.DataReceived -= ReadDataFromHardware;
                _serialPort.Close();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


            // PUT api/<ConnectionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ConnectionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
