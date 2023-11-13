using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.Models;

namespace RTD_Temperature_Controller_DotnetAPI.DBContext
{
    public class RTDSensorDBContext : DbContext
    {
        public DbSet<Data> TemperatureTable { get; set; }

        public RTDSensorDBContext(DbContextOptions<RTDSensorDBContext> options) : base(options)
        {
            
        }
    }
}
