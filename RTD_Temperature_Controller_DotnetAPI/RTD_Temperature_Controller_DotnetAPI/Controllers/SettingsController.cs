using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        // GET: api/<SettingsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SettingsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SettingsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
