using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    public class Data
    {
        [Key]
        [Column("time")]
        public DateTime Time { get; set; }

        [Column("temperature")]
        public double Temperature { get; set; }
    }
}
