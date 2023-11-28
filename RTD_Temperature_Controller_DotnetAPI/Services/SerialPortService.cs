using Contracts;
using System.IO.Ports;

namespace Services
{
    /// <summary>
    /// Service for managing a serial port connection
    /// It involves reseting, opening new serial port
    /// </summary>
    public class SerialPortService : ISerialPortService
    {
        private  SerialPort _serialPort;
        public SerialPort SerialPort { get => _serialPort;  }

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
    }
}