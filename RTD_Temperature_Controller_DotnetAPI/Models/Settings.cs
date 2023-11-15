namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    public enum Colors
    {
        red, green, blue 
    }


    public class Settings
    {
        public int Threshold { get; set; }
        public int DataAcquisitionRate { get; set; }

        public int Temperature_4mA { get; set; }
        public int Temperature_20mA { get; set; }

        public Colors Color_0_15 { get; set; }
        public Colors Color_16_30 { get; set; }
        public Colors Color_31_45 { get; set; }



    }
}
