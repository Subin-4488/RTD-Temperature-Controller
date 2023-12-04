using System.IO.Ports;

namespace Contracts
{
    public interface ISerialPortService
    {
        //public SerialPort SerialPort { get; }

        public void OpenPort(); 
        public void ClosePort();

        public void WriteToPort(byte[] bytes);
        public void ResetPort();

        public void ConfigurePortSettings(string portname, int baudrate, Parity parity,  int databits, StopBits stopBits);
    }
}