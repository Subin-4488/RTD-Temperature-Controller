using System.IO.Ports;

namespace RTD_Temperature_Controller_DotnetAPI.Models
{
    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Represents a connection configuration for communication settings.
    /// </summary>
    //////////////////////////////////////////////////////////////////////////

    public class Connection
    {
        public string PortName { get; set; }
        public int BitsPerSecond { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
    }
}
