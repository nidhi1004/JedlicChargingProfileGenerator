using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JedlixChargingProfileGenerator.DAL;
using JedlixChargingProfileGenerator.Model;

namespace JedlixChargingProfileGenerator
{
    public class Calculation
    {

        private Output Output { get; set; }
        private DateTime startTime;
        private bool isCharging;
        private Input Input { get; set; }
        public Calculation(Input? input, Output? output) 
        {
            Input = input; 
            Output = output; 
        }
        public void CheckForDCP()
        {
            if(Input.UserSettings.DirectChargingPercentage <= 15 || Input.UserSettings != null)
            {
                //Charging Car to 20%
                var timetaken = (20 - Input.CarData.CurrentBatteryLevel) / Input.CarData.ChargePower;
                TimeSpan time = this.ToDateTime(timetaken);
                var newStartTime = DateTimeOffset.Parse(Input.StartingTime).UtcDateTime;
                startTime = newStartTime.Add(time);
                Input.CarData.CurrentBatteryLevel = 20;
                isCharging = true;
            }
            CheckSchedule();
        }

        public void CheckSchedule()
        {
            try
            {
                if (Output.StartTime == null && Output.EndTime == null)
                {
                    Output.StartTime = Input.StartingTime;
                    Output.EndTime = this.startTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                    Output.IsCharging = isCharging;
                    CreateChargingSchedule();
                }
                if (Convert.ToDateTime(Output.EndTime) <= Convert.ToDateTime(Input.UserSettings.LeavingTime))
                {
                    if (Input.CarData.CurrentBatteryLevel <= Input.CarData.BatteryCapacity)
                    {
                        var lowestEnergyPrice = Input.UserSettings.Tariffs.OrderByDescending(x => x.EnergyPrice).Select(x => x.EnergyPrice);
                        while(Convert.ToDateTime(Output.EndTime) < Convert.ToDateTime(Input.UserSettings.LeavingTime))
                        {
                            foreach (var tariff in Input.UserSettings.Tariffs)
                            {
                                var s = (DateTimeOffset.Parse(this.startTime.ToString()).ToString("HH:mm:ss"));
                                var startTime = TimeSpan.Parse(s, CultureInfo.InvariantCulture);
                                var tariffStartTime = TimeSpan.Parse(tariff.StartTime);
                                var tariffEndTime = TimeSpan.Parse(tariff.EndTime);
                                TimeSpan timeTaken = TotalChargingTime();
                                if (startTime >= tariffStartTime && startTime <= tariffEndTime)
                                {
                                    var minvalue = lowestEnergyPrice.Min();
                                    //Optimal Schedule with low price
                                    if (tariff.EnergyPrice == minvalue)
                                    {
                                        Output.StartTime = this.startTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                        var datePart = this.startTime.Date;
                                        Output.EndTime = datePart.Add(startTime + timeTaken).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                        Output.IsCharging = isCharging = true;
                                    }
                                    else
                                    {
                                        Output.StartTime = this.startTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                        var datePart = this.startTime.Date;
                                        Output.EndTime = datePart.Add(startTime + timeTaken).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                        Output.IsCharging = isCharging = false;
                                    }
                                }
                                else
                                {
                                    Output.StartTime = this.startTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                    var datePart = this.startTime.Date;
                                    Output.EndTime = datePart.Add(startTime + timeTaken).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                    Output.IsCharging = isCharging = false;
                                }
                                this.startTime = DateTimeOffset.Parse(Output.EndTime).UtcDateTime;
                                CreateChargingSchedule();
                            }

                        }
                    }
                }
            }
            catch (Exception) { }
        }
        public void CreateChargingSchedule()
        {
            ConnectJson con = new ConnectJson();
            con.WriteToOutputJson(Output);
        }

        public TimeSpan TotalChargingTime()
        {
            decimal time = (Input.UserSettings.DesiredStateOfCharge - Input.CarData.CurrentBatteryLevel) / Input.CarData.ChargePower;
            TimeSpan timeSpan = ToDateTime(time);
            return timeSpan;
        }
        public TimeSpan ToDateTime(decimal value)
        {
            int minutes = 0;
            int hours = 0;
            if (value.ToString().StartsWith('0'))
            {
                minutes = (int)(value * 60);
            }
            else
            {
                string[] parts = value.ToString().Split(new char[] { '.' });
                hours = Convert.ToInt32(parts[0]);
                minutes = Convert.ToInt32(parts[1]);
                if ((hours > 23) || (hours < 0))
                {
                    throw new ArgumentOutOfRangeException("decimal value must be no greater than 23.59 and no less than 0");
                }
                if ((minutes > 59) || (minutes < 0))
                {
                    throw new ArgumentOutOfRangeException("decimal value must be no greater than 23.59 and no less than 0");
                }
                hours = hours * 60;
                minutes = minutes * 60;
            }
            TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);
            return timeSpan;
        }

    }
}
