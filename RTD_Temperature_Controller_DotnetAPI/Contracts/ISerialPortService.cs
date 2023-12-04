using System.IO.Ports;

namespace Contracts
{
    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This interface defines the contract for serial port operations.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

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