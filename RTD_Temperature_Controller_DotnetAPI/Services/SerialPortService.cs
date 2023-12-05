using Contracts;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;

namespace Services
{
    /// <summary>
    /// Service for managing a serial port connection
    /// It involves reseting, opening new serial port
    /// </summary>
    public class SerialPortService : ISerialPortService
    {
        private SerialPort _serialPort;
        //public SerialPort SerialPort { get => _serialPort; }

        /// <summary>
        /// Creates a new serial port instance
        /// </summary>
        public SerialPortService()
        {
            _serialPort = new SerialPort();
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
                _serialPort.Close();
            }
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

        public string ReadFromPort(string delim)
        { 
            return _serialPort.ReadTo(delim);
        }

        public void SetListener(Action<object, SerialDataReceivedEventArgs> action)
        {
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(action);
        }

        public void RemoveListener(SerialDataReceivedEventHandler action)
        {
            _serialPort.DataReceived -= action;
        }
    }
}