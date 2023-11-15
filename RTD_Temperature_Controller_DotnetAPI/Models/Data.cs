using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    public class Data
    {
        [Key]
        public DateTime Time { get; set; }

        public double Temperature { get; set; }
    }
}
