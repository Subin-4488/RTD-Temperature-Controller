using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Models;

namespace RTD_Temperature_Controller_DotnetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly RTDSensorDBContext _context;

        public DataController(RTDSensorDBContext context)
        {
            _context = context;
        }

        // GET: api/Data
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Data>>> GetEntireTemperatureData()
        {
            if (_context.TemperatureTable == null)
            {
                return NotFound();
            }
            return await _context.TemperatureTable.Select(d => d).ToListAsync();
        }

        // GET: api/Data/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Data>> GetTemperatureData(DateTime id)
        {
            if (_context.TemperatureTable == null)
            {
                return NotFound();
            }
            var data = await _context.TemperatureTable.FirstOrDefaultAsync(d => d.Time == id);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<Data>> GetLatestTemperatureData()
        {
            if (_context.TemperatureTable == null)
            {
                return NotFound();
            }
            var data = await _context.TemperatureTable.LastOrDefaultAsync();

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }

        // PUT: api/Data/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutData(DateTime id, Data data)
        {
            if (id != data.Time)
            {
                return BadRequest();
            }

            var matchData = await _context.TemperatureTable.FirstOrDefaultAsync(d => d.Time == id);
            matchData.Temperature = data.Temperature;

            _context.Entry(data).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Data
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        // DELETE: api/Data/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteData(DateTime id)
        {
            if (_context.TemperatureTable == null)
            {
                return NotFound();
            }
            var data = await _context.TemperatureTable.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            _context.TemperatureTable.Remove(data);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DataExists(DateTime id)
        {
            return (_context.TemperatureTable?.Any(e => e.Time == id)).GetValueOrDefault();
        }
    }
}
