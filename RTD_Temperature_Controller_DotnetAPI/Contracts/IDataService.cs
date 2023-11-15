using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
namespace Contracts
{
    public interface IDataService
    {
        public void ReadDataFromHardware(object sender, SerialDataReceivedEventArgs e);
        public Task<(bool, string)> WriteToDatabase(Data data);
    }
}
