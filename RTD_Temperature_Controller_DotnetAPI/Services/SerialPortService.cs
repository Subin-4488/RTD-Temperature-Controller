using Contracts;
using System.IO.Ports;

namespace Services
{
    public class SerialPortService : ISerialPortService
    {
        private  SerialPort _serialPort;
        public SerialPort SerialPort { get => _serialPort;  }

        public SerialPortService()
        {
            _serialPort = new SerialPort();
        }

        public void deleteMethod()
        {
            if (_serialPort != null && _serialPort.IsOpen)
                _serialPort.Close();
            _serialPort = new SerialPort();
        }
    }
}