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
         SerialPort SerialPort { get; }

        public void ResetPort();
    }
}