﻿using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
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

        //delete this line
        //private readonly RTDSensorDBContext _dbContext;

        Thread _thread;
        private static Boolean _status = false;

        public ConnectionController(IHubContext<TemperatureHub> hubContext, 
            ISerialPortService serialPortService, 
            IDataService dataService
            //RTDSensorDBContext dbContext
            )
        {
            serialPortService.deleteMethod();
            this._serialPort = serialPortService.SerialPort;
            this._dataService = dataService;
            _hubContext = hubContext;

            //delete this line
            //_dbContext = dbContext;

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
            catch (Exception e)
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
            
            //_serialPort.ReadTimeout=500;
            //_serialPort.DataReceived += _dataService.ReadDataFromHardware;

            try
            {

                if (!_serialPort.IsOpen)
                _serialPort.Open();

                //_serialPort.DiscardInBuffer();

                //byte[] bytes = Encoding.UTF8.GetBytes("SET MOD MAN\r");
                //_serialPort.Write(bytes, 0, bytes.Length);

                //string manMod = _serialPort.ReadTo("\r");

                //Console.WriteLine(manMod);
                //_serialPort.DiscardInBuffer();


                _serialPort.WriteTimeout = 3000;
                _serialPort.ReadTimeout = 3000;
                string[] temp;
                byte[]  bytes = Encoding.UTF8.GetBytes("GET VER\r");
                try
                {
                    _serialPort.Write(bytes, 0, bytes.Length);
                    string version = _serialPort.ReadTo("\r");
                    Console.WriteLine(version);
                    temp = version.Split(" ");

                    if (temp.Length < 2 || temp[0] != "OK" || temp[1] != "VER")
                        return false;



                    bytes = Encoding.UTF8.GetBytes("SET MOD ATM\r");

                    _serialPort.Write(bytes, 0, bytes.Length);
                    string mod = _serialPort.ReadTo("\r");
                    Console.WriteLine(mod);
                    temp = mod.Split(" ");


                    if (temp.Length < 2 || temp[0] != "OK" || temp[1] != "MOD")
                        return false;

                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(_dataService.ReadDataFromHardware);
                    bytes = Encoding.UTF8.GetBytes("GET CON\r");
                    _serialPort.Write(bytes, 0, bytes.Length);

                }
                catch (TimeoutException ex) {
                    return false;
                }
                finally {
                    _serialPort.WriteTimeout = Timeout.Infinite;
                    _serialPort.ReadTimeout = Timeout.Infinite;
                }





                _status = true;
                //_thread = new Thread(sendRandom);
                //_thread.Start();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private async Task sendRandom()
        {
            Console.WriteLine("In thread");
            Random rnd = new Random();

            while (_status == true)
            {
                try
                {
                    int num = rnd.Next(1, 45);
                    var data = new Data();
                    data.Time = DateTime.Now;
                    data.Temperature = num;

                    //delete following 2 lines
                    //await _dbContext.TemperatureTable.AddAsync(data);
                    //await _dbContext.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("UpdateTemperature", data);
                    Console.WriteLine(data.Time + " : " + data.Temperature);
                    Thread.Sleep(1000);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        [HttpPost("disconnect")]
        public bool Post()
        {
            try
            {
                
                //byte[] bytes = Encoding.UTF8.GetBytes("SET MOD ATM\r");
                //_serialPort.Write(bytes, 0, bytes.Length);
                _serialPort.DataReceived -= _dataService.ReadDataFromHardware;
                //Thread.Sleep(1500);
                _serialPort.Close();
                _status = false;
                //_thread.Abort();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("disconnect"+ex);
                return false;
            }
        }
        //[HttpPost("setatm")]
        //public bool SetMode()
        //{
        //    try
        //    {

        //        byte[] bytes = Encoding.UTF8.GetBytes("SET MOD ATM\r");
        //        _serialPort.Write(bytes, 0, bytes.Length);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("disconnect" + ex);
        //        return false;
        //    }
        //}
    }
}