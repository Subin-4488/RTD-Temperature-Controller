namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    public enum Colors
    {
        red = 'R', green = 'G', blue = 'B'
    }

    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Settings model containing various configuration properties.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public class Settings
    {
        public double Threshold { get; set; }
        public int DataAcquisitionRate { get; set; }

        public double Temperature_4mA { get; set; }
        public double Temperature_20mA { get; set; }

        public Colors Color_Range_1 { get; set; }
        public Colors Color_Range_2 { get; set; }
        public Colors Color_Range_3 { get; set; }

    }
}
