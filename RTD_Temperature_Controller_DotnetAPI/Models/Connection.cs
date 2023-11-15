namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    public enum Bps
    {
        Bps_1 = 1200,
        Bps_2 = 2400,
        Bpe_3 = 4800,
        Bps_4 = 9600,
        Bps_5 = 14400,
        Bps_6 = 38400 
    }

    public enum DataBits
    {
        DataBit_1 = 5,
        DataBit_2 = 6,
        DataBit_3 = 7,
        DataBit_4 = 8,
    }

    public enum Parity
    {
        None,
        Even,
        Odd,
        Mark,
        Space
    }

    public struct StopBits
    {
        public const int StopBits_1 = 1;
        public const double StopBits_2 = 1.5; 
        public const int StopBits_3 = 2;
    }

    public class Connection
    {
        public Bps BitsPerSecond { get; set; }
        public DataBits DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
    }
}
