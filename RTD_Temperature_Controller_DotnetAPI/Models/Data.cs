using System.ComponentModel.DataAnnotations;

namespace RTD_Temperature_Controller_DotnetAPI.Models
{

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Represents a data class for storing time and temperature information.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public class Data
    {
        [Key]
        public DateTime Time { get; set; }

        public double Temperature { get; set; }
    }
}
