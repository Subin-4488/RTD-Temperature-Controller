using Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RTD_Temperature_Controller_DotnetAPI.Models;
using Serilog;
using System.IO.Ports;
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
        private readonly ISerialPortService _serialPortService;
        private readonly IDataService _dataService;

        /// <summary>
        /// Constructor for the ConnectionController class.
        /// </summary>
        /// <param name="serialPortService">The serial port service for managing the connection</param>
        /// <param name="dataService">The data service for handling data from the RTD temperature controller</param>

        public ConnectionController(
            ISerialPortService serialPortService,
            IDataService dataService)
        {

            _serialPortService = serialPortService;
            this._dataService = dataService;
            
            serialPortService.ResetPort();
            _serialPortService.ClosePort();
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
                Connection connection = JsonConvert.DeserializeObject<Connection>(value.ToString());
                 
                _serialPortService.ConfigurePortSettings(connection);
                _serialPortService.OpenPort();

                _serialPortService.SetWriteTimeout(3000);
                _serialPortService.SetReadTimeout(3000);
                string[] temp;
                byte[] bytes = Encoding.UTF8.GetBytes("GET VER\r");
                try
                {
                    _serialPortService.WriteToPort(bytes);
                    string version = _serialPortService.ReadFromPort("\r");
                    Console.WriteLine(version);
                    temp = version.Split(" ");

                    if (temp.Length < 2 || temp[0] != "OK" || temp[1] != "VER")
                        return false;

                    bytes = Encoding.UTF8.GetBytes("SET MOD ATM\r");

                    _serialPortService.WriteToPort(bytes);
                    string mod = _serialPortService.ReadFromPort("\r");
                    Console.WriteLine(mod);
                    temp = mod.Split(" ");

                    if (temp.Length < 2 || temp[0] != "OK" || temp[1] != "MOD")
                        return false;

                    _serialPortService.SetListener(_dataService.ReadDataFromHardware);
                    bytes = Encoding.UTF8.GetBytes("GET CON\r");
                    _serialPortService.WriteToPort(bytes);
                }
                catch (TimeoutException ex)
                {
                    Log.Information("Connection Failed because of ReadTimeOut.");
                    return false;
                }
                finally
                {
                    _serialPortService.SetWriteTimeout(Timeout.Infinite);
                    _serialPortService.SetReadTimeout(Timeout.Infinite);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Information("Connection Failed " + ex.ToString());
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
                _serialPortService.RemoveListener(_dataService.ReadDataFromHardware);
                _serialPortService.ClosePort();
                return true;
            }
            catch (Exception ex)
            {
                Log.Information("Disconnect Failed " + ex.ToString());
                return false;
            }
        }
    }
}