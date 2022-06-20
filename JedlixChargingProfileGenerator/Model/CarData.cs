using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JedlixChargingProfileGenerator.Model
{
    public class CarData
    {
        public decimal ChargePower { get; set; }
        public decimal BatteryCapacity { get; set; }
        public decimal CurrentBatteryLevel { get; set; }
    }
}
