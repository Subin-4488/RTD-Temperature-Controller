using Contracts;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Controller for managing the connection to the RTD temperature sensor.
    /// </summary>
    /// <remarks>
    /// Manages the serial port connection, configuration, and disconnection,
    /// </remarks>
    //////////////////////////////////////////////////////////////////////////

    [Route("connection")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly SerialPort _serialPort;
        private readonly IDataService _dataService;

        public ConnectionController(
            ISerialPortService serialPortService,
            IDataService dataService
        )
        {
            serialPortService.ResetPort();
            this._serialPort = serialPortService.SerialPort;
            this._dataService = dataService;

            if (this._serialPort.IsOpen) this._serialPort.Close();

        }

        /// <summary>
        /// Gets the available serial ports.
        /// </summary>
        /// <returns>
        /// A list of available serial port names.
        /// </returns>

        [HttpGet("ports")]
        public IEnumerable<string> Get()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Connects to the RTD temperature controller using the specified configuration.
        /// </summary>
        /// <param name="value">The configuration parameters for the serial port connection</param>
        /// <returns>True if the connection is successful; otherwise, false.</returns>
        /// <exception cref="Exception">Thrown when parity format is bad</exception>
        /// <exception cref="FormatException">Thrown when stopbits format is bad</exception>

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
            this._serialPort.BaudRate = int.Parse(value["BitsPerSecond"].ToString());

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
                    throw new FormatException("Bad Parity format");
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
                    throw new Exception("Bad Stopbits format");
            }

            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();

                _serialPort.WriteTimeout = 3000;
                _serialPort.ReadTimeout = 3000;
                string[] temp;
                byte[] bytes = Encoding.UTF8.GetBytes("GET VER\r");
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
                catch (TimeoutException ex)
                {
                    Log.Information("Connection Failed because of ReadTimeOut.");
                    return false;
                }
                finally
                {
                    _serialPort.WriteTimeout = Timeout.Infinite;
                    _serialPort.ReadTimeout = Timeout.Infinite;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Information("Connection Failed "+ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Disconnects from the RTD temperature controller.
        /// </summary>
        /// <returns>True if disconnection is successful; otherwise, false.</returns>

        [HttpPost("disconnect")]
        public bool Post()
        {
            try
            {
                _serialPort.DataReceived -= _dataService.ReadDataFromHardware;
                _serialPort.Close();
                return true;
            }
            catch (Exception ex)
            {
                Log.Information("Disconnect Failed "+ex.ToString());
                return false;
            }
        }
    }
}