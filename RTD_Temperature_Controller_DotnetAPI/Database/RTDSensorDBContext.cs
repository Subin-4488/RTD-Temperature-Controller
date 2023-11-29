using Microsoft.EntityFrameworkCore;
using RTD_Temperature_Controller_DotnetAPI.Models;

namespace RTD_Temperature_Controller_DotnetAPI.DBContext
{

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Represents the database context for the RTD sensor data(Temperature) and Time.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public class RTDSensorDBContext : DbContext
    {
        public DbSet<Data> TemperatureTable { get; set; }
        public RTDSensorDBContext(DbContextOptions<RTDSensorDBContext> options) : base(options) { }

        /// <summary>
        /// Configures the model of the database.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data>().ToTable(nameof(Data));
        }
    }
}
