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
        public bool IsOpen();
        public void SetReadTimeout(int millis);
        public void SetWriteTimeout(int millis);

        public void ReadFromPort(object sender, SerialDataReceivedEventArgs e);
        public string ReadInitial(string delim);
        public void WriteToPort(byte[] bytes);
        public void ResetPort();
        public void ConfigurePortSettings(Connection connection);
        public void SetListener();
    }
}