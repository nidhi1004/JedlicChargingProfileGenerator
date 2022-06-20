using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JedlixChargingProfileGenerator.Model
{
    public class Input
    {
        public string StartingTime { get; set; }
        public UserSettings UserSettings { get; set; }
        public CarData CarData { get; set; }

    }
}
