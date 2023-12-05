using Contracts;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System;
using System.Text;
using System.IO.Ports;

namespace Services
{
    public delegate Task DataReceived(string message);
    /// <summary>
    /// Service for managing a serial port connection
    /// It involves reseting, opening new serial port
    /// </summary>
    public class SerialPortService : ISerialPortService
    {
        private static SerialPort _serialPort = new SerialPort();
        private IDataService _dataservice;
        //public SerialPort SerialPort { get => _serialPort; }
        public event DataReceived _dataReceived; 
        /// <summary>
        /// Creates a new serial port instance
        /// </summary>
        public SerialPortService(IDataService dataService)
        {
            //_serialPort = new SerialPort();
            _dataservice = dataService;
        }
        /// <summary>
        ///  Resets the SerialPort instance by closing the existing one (if open) and creating a new instance.
        /// </summary>
        public void ResetPort()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();
            _serialPort = new SerialPort();
        }

        public void OpenPort()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void ClosePort()
        {
            if (_serialPort.IsOpen)
            {
                _dataReceived -= _dataservice.ReadDataFromHardware;
                _serialPort.DataReceived -= ReadFromPort;
                _serialPort.Close();
            }
        }

        public bool IsOpen()
        {
            return _serialPort.IsOpen;
        }
        public void WriteToPort(byte[] bytes)
        {
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        public void ConfigurePortSettings(Connection connection)
        {
            _serialPort.PortName = connection.PortName;
            _serialPort.BaudRate = connection.BitsPerSecond;
            _serialPort.Parity = connection.Parity;
            _serialPort.DataBits = connection.DataBits;
            _serialPort.StopBits = connection.StopBits;
        }

        public void SetReadTimeout(int millis)
        {
            _serialPort.ReadTimeout = millis;
        }

        public void SetWriteTimeout(int millis)
        {
            _serialPort.WriteTimeout = millis;
        }

        public void ReadFromPort(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string result = _serialPort.ReadTo("\r");
                Console.WriteLine(result);
                string[] resultArr = result.Split(' ');
                if (resultArr.Length > 2 && resultArr[0] == "OK" && resultArr[1] == "RTD" && resultArr[2] == "RDY")
                {
                    byte[] bytes = Encoding.UTF8.GetBytes("GET TMPA\r");
                    WriteToPort(bytes);
                }
                else
                    _dataReceived?.Invoke(result);
                //invoke
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public string ReadInitial(string delim)
        {
            return _serialPort.ReadTo(delim);
        }

        public void SetListener()
        {
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(ReadFromPort);
            _dataReceived += _dataservice.ReadDataFromHardware;
        }

    }
}