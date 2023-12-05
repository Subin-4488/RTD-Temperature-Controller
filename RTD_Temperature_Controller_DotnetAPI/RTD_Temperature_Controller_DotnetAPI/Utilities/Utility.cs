using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RTD_Temperature_Controller_DotnetAPI.Utilities
{
   
    public static class Command
    {
        public const string GetVersion = "GET VER\r";
        public const string GetTemperatureAutomaticMode = "GET TMPA\r";
        public const string GetConfiguration = "GET CON\r";

        public const string OKVersion = "OK VER\r";
        public const string OKMode = "OK MODE\r";
        public const string OKConfiguration = "OK CON\r";
        public const string OkTemperatureAutomaticMode = "OK TMPA\r";
        public const string OkTemperatureManualMode = "OK TMPM\r";
        public const string OkResistance = "OK RES\r";
        public const string OkEPROM = "OK EPR\r";
        public const string OkButton = "OK BTN\r";
        public const string OkPWMDutyCycle = "OK PWM\r";
        public const string OkSensorReady = "OK RTD RDY\r";

        public const string SetAutomaticMode = "SET MODE ATM\r";

        public const string PropertyLED = "LED\r";
        public const string PropertyCurrentLow = "OL\r";
        public const string PropertyCurrentHigh = "OH\r";

    }
}
