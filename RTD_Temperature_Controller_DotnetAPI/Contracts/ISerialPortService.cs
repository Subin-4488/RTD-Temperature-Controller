using System.IO.Ports;

namespace Contracts
{
    public interface ISerialPortService
    {
        public SerialPort SerialPort { get; }

        public void ResetPort();
    }
}