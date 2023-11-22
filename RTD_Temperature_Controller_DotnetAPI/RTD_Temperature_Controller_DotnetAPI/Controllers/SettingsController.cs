using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RTD_Temperature_Controller_DotnetAPI.Hubs;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SerialPort _serialPort;
        public SettingsController(ISerialPortService serialPortService)
        {
            this._serialPort = serialPortService.SerialPort;
        }
        // GET: api/<SettingsController>
        [HttpGet]
        public async Task<Settings> Get()
        {
            string fileName = @"..\..\settingsFile.json";
            using FileStream openStream = System.IO.File.OpenRead(fileName);
            Settings? settingsValue =
                await JsonSerializer.DeserializeAsync<Settings>(openStream);

            return settingsValue;
        }

        // GET api/<SettingsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SettingsController>
        [HttpPost]
        public void Post([FromBody] JsonObject s)
        {
            var newSettings= new Settings();
            newSettings.Threshold = Convert.ToInt32(s["Threshold"].ToString());
            newSettings.DataAcquisitionRate = Convert.ToInt32(s["DataAcquisitionRate"].ToString());
            newSettings.Temperature_4mA = Convert.ToInt32(s["Temperature_4mA"].ToString());
            newSettings.Temperature_20mA = Convert.ToInt32(s["Temperature_20mA"].ToString());
            newSettings.Color_0_15 = (Colors)Enum.Parse(typeof(Colors), Convert.ToString(s["Color_0_15"]));
            newSettings.Color_16_30 = (Colors)Enum.Parse(typeof(Colors), Convert.ToString(s["Color_16_30"]));
            newSettings.Color_31_45 = (Colors)Enum.Parse(typeof(Colors), Convert.ToString(s["Color_31_45"]));
            StringBuilder sendString = new StringBuilder("SET CON LED:");
            //Console.WriteLine((char)newSettings.Color_0_15);
            sendString.Append($"{(char)newSettings.Color_0_15}{(char)newSettings.Color_16_30}{(char)newSettings.Color_31_45}");
            sendString.Append($",OL:{s["Temperature_4mA"]},OH:{s["Temperature_20mA"]}\r");
            Console.WriteLine(sendString);

            byte[] bytes = Encoding.UTF8.GetBytes(sendString.ToString());
            _serialPort.Write(bytes, 0, bytes.Length);
            string jsonString = JsonSerializer.Serialize<Settings>(newSettings);
            System.IO.File.WriteAllText(@"..\..\settingsFile.json", jsonString);
        }

        //public string GetColor(string inp)
        //{
        //    switch (Convert.ToString(inp))
        //    {
        //        case "red":
        //            return "R";
        //            break;
        //        case "green":
        //            return "G";
        //            break;
        //        case "blue":
        //            return "B";
        //            break;
        //        default:
        //            return "";
        //    }
        //}

        // PUT api/<SettingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SettingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
