using RTD_Temperature_Controller_DotnetAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITemperatureRepository
    {
        public void Insert(Data data);
    }
}
