using Contracts;
using RTD_Temperature_Controller_DotnetAPI.DBContext;
using RTD_Temperature_Controller_DotnetAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TemperatureRepository : ITemperatureRepository
    {
        private readonly RTDSensorDBContext _rtdSensorDBContext;

        public TemperatureRepository(RTDSensorDBContext rTDSensorDBContext)
        {
            _rtdSensorDBContext = rTDSensorDBContext;
        }

        public async void Insert(Data data)
        {
            await _rtdSensorDBContext.AddAsync(data);
        }
    }
}
