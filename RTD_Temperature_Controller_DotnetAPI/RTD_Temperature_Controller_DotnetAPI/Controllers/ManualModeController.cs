using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("manualmode")]
    [ApiController]
    public class ManualModeController : ControllerBase
    {
        // GET: api/<ManualModeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ManualModeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManualModeController>
        [HttpPost]
        public bool Post([FromBody] JsonObject value)
        {
            Console.WriteLine(value);
            byte[] bytes = Encoding.UTF8.GetBytes(value["Value"].ToString());
            Console.WriteLine(bytes);
            string hexString = Convert.ToHexString(bytes);
            Console.WriteLine(hexString);
            try
            {

                Program.SerialPort.Write(bytes, 0, bytes.Length);
            }
            catch(Exception ex)
            {

            }
            return true;
        }

        // PUT api/<ManualModeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManualModeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
