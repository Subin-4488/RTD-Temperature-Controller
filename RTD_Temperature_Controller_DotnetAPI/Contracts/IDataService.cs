using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
namespace Contracts
{
    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// This interface defines the contract for interacting with data services.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public interface IDataService
    {
        public void ReadDataFromHardware(object sender, SerialDataReceivedEventArgs e);
        public Task<(bool, string)> WriteToDatabase(Data data);
    }
}
