using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using JedlixChargingProfileGenerator.Model;
using Newtonsoft.Json.Linq;

namespace JedlixChargingProfileGenerator.DAL
{
    public class ConnectJson
    {
        private IConfiguration _configuration;
        public static Input? input = new Input();
        public static Output? output = new Output();
        private string url = "https://api.jsonbin.io/b/62b0f6c75c2a444a2d9306bf/1";
        private Input Input { get; set; }

        public string Url { get => url; set => url = value; }
        
        private Calculation calculation = new Calculation(input, output);

        public void ReadFromJson()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(Url);
                    Input = JsonConvert.DeserializeObject<Input>(json);
                    WriteToInputJson(Input);
                    Calci();
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Problem reading Json");
            }
        }
        public void Calci()
        {
            calculation = new Calculation(Input, output);
            this.calculation.CheckForDCP();
            calculation.CheckSchedule();

        }

        public void WriteToOutputJson(Output output)
        {
            string json = JsonConvert.SerializeObject(output);
            StreamWriter file = new StreamWriter(@"outputFile.json", append: true);
            file.WriteLine(json);
            file.Close();
            Console.WriteLine(json.ToString());
            Console.WriteLine("\n");

        }

        public void WriteToInputJson(Input input)
        {
            string json = JsonConvert.SerializeObject(input);
            StreamWriter file = new StreamWriter(@"inputFile.json", append: true);
            file.WriteLine(json);
            file.Close();
        }
    }
}
