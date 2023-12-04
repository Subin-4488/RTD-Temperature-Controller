using Contracts;
using System.IO.Ports;

namespace Services
{
    public class SerialPortService : ISerialPortService
    {
        private SerialPort _serialPort;
        //public SerialPort SerialPort { get => _serialPort; }

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
            _serialPort.Open();
        }

        public void ClosePort()
        {
            _serialPort.Close();
        }

        public void WriteToPort(byte[] bytes)
        {
            _serialPort.Write(bytes, 0, bytes.Length);
        }

        public void ConfigurePortSettings(string portname, int baudrate, Parity parity, int databits, StopBits stopBits)
        {
            this._serialPort.PortName = portname;
            this._serialPort.BaudRate = baudrate;
            this._serialPort.Parity = parity;
            this._serialPort.DataBits = databits;
            this._serialPort.StopBits = stopBits;
        }
    }
}