using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Reflection;

namespace Contracts
{
    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This interface defines the contract for serial port operations.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public interface ISerialPortService
    {

        public void OpenPort(); 
        public void ClosePort();

        public void SetReadTimeout(int millis);
        public void SetWriteTimeout(int millis);

        public string ReadFromPort(string delim);
        public void WriteToPort(byte[] bytes);
        public void ResetPort();

        public void SetListener(Action<object, SerialDataReceivedEventArgs> action);
        public void RemoveListener(SerialDataReceivedEventHandler action);

        public void ConfigurePortSettings(Connection connection);
    }
}