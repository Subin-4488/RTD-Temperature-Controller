using Contracts;
using System.IO.Ports;

namespace Services
{
    public class SerialPortService : ISerialPortService
    {
        private readonly SerialPort _serialPort;
        public SerialPort SerialPort { get => _serialPort;  }

        public SerialPortService()
        {
            _serialPort = new SerialPort();
        }
    }
}