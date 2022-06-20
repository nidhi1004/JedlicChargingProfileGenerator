using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JedlixChargingProfileGenerator.Model
{
    public class UserSettings
    {
        public int DesiredStateOfCharge { get; set; }
        public string LeavingTime { get; set; }
        public int DirectChargingPercentage { get; set; }
        public Tariffs[] Tariffs { get; set; }
    }
}
