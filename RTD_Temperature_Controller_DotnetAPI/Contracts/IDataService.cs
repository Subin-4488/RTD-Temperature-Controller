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
        public Task ReadDataFromHardware(string result);
        public Task<(bool, string)> WriteToDatabase(Data data);
    }
}
