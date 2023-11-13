using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Data>>> GetTemperatureTable()
        {
          if (_context.TemperatureTable == null)
          {
              return NotFound();
          }
            return await _context.TemperatureTable.ToListAsync();
        }

        // GET: api/Data/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Data>> GetData(DateTime id)
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
        [HttpPost]
        public async Task<ActionResult<Data>> PostData(Data data)
        {
          if (_context.TemperatureTable == null)
          {
              return Problem("Entity set 'RTDSensorDBContext.TemperatureTable'  is null.");
          }
            _context.TemperatureTable.Add(data);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DataExists(data.Time))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetData", new { id = data.Time }, data);
        }

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
