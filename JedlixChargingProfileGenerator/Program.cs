using JedlixChargingProfileGenerator.DAL;

namespace JedlixChargingProfileGenerator
{
    class Program
    {
        public static void Main()
        {
            var connectDB = new ConnectJson();
            connectDB.ReadFromJson();
        }
    }

}


