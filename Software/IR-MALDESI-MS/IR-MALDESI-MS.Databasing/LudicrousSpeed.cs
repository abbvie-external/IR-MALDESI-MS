using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IR_MALDESI.Databasing
{
    public class LudicrousSpeed
    {
        public class Speeds
        {
            public double R7500 { get; set; }
            public double R15000 { get; set; }
            public double R30000 { get; set; }
            public double R60000 { get; set; }
            public double R120000 { get; set; }
            public double R240000 { get; set; }
        }

        public static Speeds deserializeLudicrousSpeeds()
        {
            // Filename
            var file = $@"..\..\Files\LudicrousSpeeds_{Environment.MachineName}.json";

            return JsonConvert.DeserializeObject<Speeds>(File.ReadAllText(file));
        }

        public static void serializeLudicrousSpeeds(Speeds speeds)
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter($@"..\..\Files\LudicrousSpeeds_{Environment.MachineName}.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, speeds);
            }
        }
    }
}