using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Models;

namespace RTD_Temperature_Controller_DotnetAPI.Hubs
{
    public class TemperatureHub : Hub
    {
        public async Task SendTemperature(Data data)
        {
            await Clients.All.SendAsync("UpdateTemperature", data);
        }

        public async Task SendManualModeData(ManualModeData data)
        {
            await Clients.All.SendAsync("ManualModeData", data);
        }
    }
}
