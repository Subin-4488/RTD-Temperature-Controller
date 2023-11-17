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
    [Route("connection")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly SerialPort _serialPort;
        private IDataService _dataService;
        private readonly IHubContext<TemperatureHub> _hubContext;
        Thread _thread;
        private static Boolean _status=false;

        public ConnectionController(IHubContext<TemperatureHub> hubContext,ISerialPortService serialPortService, IDataService dataService)
        {
            this._serialPort = serialPortService.SerialPort;
            this._dataService = dataService;
            _hubContext = hubContext;

            if (this._serialPort.IsOpen) this._serialPort.Close();

        }

        //GET: api/<ConnectionController>
        [HttpGet("ports")]
        public IEnumerable<string> Get()
        {
            return SerialPort.GetPortNames();
        }

        // POST api/<ConnectionController>
        [HttpPost]
        public bool Post([FromBody] JsonObject value)
        {
            try
            {
                this._serialPort.PortName = Convert.ToString(value["PortName"]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
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

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(_dataService.ReadDataFromHardware);

            try
            {
                _serialPort.Open();
                _status = true;
                _thread = new Thread(sendRandom);
                _thread.Start();
                return true;
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        private async void sendRandom()
        {
            Console.WriteLine("In thread");
            Random rnd = new Random();
            
            while (_status==true)
            {
                int num = rnd.Next(1, 45);
                var data = new Data();
                data.Time = DateTime.Now;
                data.Temperature = num;
                await _hubContext.Clients.All.SendAsync("UpdateTemperature", data);
                Console.WriteLine(data.Time+" : "+data.Temperature);
                Thread.Sleep(1000);
            }
        }

        [HttpPost("disconnect")]
        public bool Post()
        {
            try
            {
                _serialPort.DataReceived -= _dataService.ReadDataFromHardware;
                _serialPort.Close();
                _status = false;
                //_thread.Abort();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
