using Microsoft.AspNetCore.Mvc;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("connection")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        //GET: api/<ConnectionController>
        [HttpGet("ports")]
        public IEnumerable<string> Get()
        {
            //return new string[] { "COM1", "COM2" };
            return SerialPort.GetPortNames();
        }

        [HttpGet]
        // GET api/<ConnectionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ConnectionController>
        [HttpPost]
        public void Post([FromBody] JsonObject value)
        {
            //object objtemp = value["BitsPerSecond"];
            try
            {
                //Program.SerialPort.PortName = value["PortName"]!.ToString();
                Program.SerialPort.PortName = Convert.ToString(value["PortName"]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Program.SerialPort.PortName = Program.SerialPort.PortName;
            }
            //Program.SerialPort.PortName = "COM5";
            //Program.SerialPort.PortName = Convert.ToString(value["PortName"]);
            Program.SerialPort.BaudRate = int.Parse(value["BitsPerSecond"]!.ToString());
            
            switch (value["Parity"]!.ToString())
            {
                case "Even":
                    Program.SerialPort.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "Odd":
                    Program.SerialPort.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "Mark":
                    Program.SerialPort.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case "Space":
                    Program.SerialPort.Parity = System.IO.Ports.Parity.Space;
                    break;
                case "None":
                    Program.SerialPort.Parity = System.IO.Ports.Parity.None;
                    break;
                default:
                    throw new Exception("BAD Parity format");
            }

            Program.SerialPort.DataBits = int.Parse(value["DataBits"]!.ToString());


            switch (value["StopBits"]!.ToString())
            {
                case "1":
                    Program.SerialPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    Program.SerialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    Program.SerialPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    throw new Exception("BAD Parity format");
            }

            Console.WriteLine(value.ToString());
            try
            {
                Program.SerialPort.Open();
                Program.ReadThread.Start();
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
            }
            //Program.SerialPort.Open();
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
