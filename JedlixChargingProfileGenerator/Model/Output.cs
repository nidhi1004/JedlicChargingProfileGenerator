using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JedlixChargingProfileGenerator.Model
{
    public class Output
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsCharging { get; set; }
    }
}
