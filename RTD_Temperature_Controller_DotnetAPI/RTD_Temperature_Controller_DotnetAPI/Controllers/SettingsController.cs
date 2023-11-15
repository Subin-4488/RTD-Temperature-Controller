using Microsoft.AspNetCore.Mvc;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // GET: api/<SettingsController>
        [HttpGet]
        public async Task<Settings> Get()
        {
            string fileName = "C:\\Users\\Alby Joseph\\Desktop\\RTD-Temperature-Controller\\settingsFile.json";
            using FileStream openStream = System.IO.File.OpenRead(fileName);
            Settings? settingsValue =
                await JsonSerializer.DeserializeAsync<Settings>(openStream);
            return settingsValue;
            //return new Settings(0, 0, 0, 0, Colors.red, Colors.green, Colors.blue);
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
            string jsonString = JsonSerializer.Serialize<Settings>(newSettings);
            System.IO.File.WriteAllText(@"C:\Users\Alby Joseph\Desktop\RTD-Temperature-Controller\settingsFile.json", jsonString);
        }

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
