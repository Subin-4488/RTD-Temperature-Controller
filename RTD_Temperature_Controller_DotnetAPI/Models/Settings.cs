namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    public enum Colors
    {
        red = 'R', green = 'G', blue = 'B' 
    }


    public class Settings
    {
        public double Threshold { get; set; }
        public int DataAcquisitionRate { get; set; }

        public double Temperature_4mA { get; set; }
        public double Temperature_20mA { get; set; }

        public Colors Color_0_15 { get; set; }
        public Colors Color_16_30 { get; set; }
        public Colors Color_31_45 { get; set; }



    }
}
