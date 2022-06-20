using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JedlixChargingProfileGenerator.Model
{
    public class Tariffs
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double EnergyPrice { get; set; }
    }
}
