using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IR_MALDESI.Databasing
{
    public class HTTPQuery
    {
        public static readonly HttpClient client = new HttpClient();

        /// <summary>
        ///
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public List<PlateMap> Query(string barcode = "XXX56307-1")
        {
            var list = new List<PlateMap>();

            // Check which type of barcode it is
            string p;
            if (barcode == null) return list;
            var master = $"XX-{barcode.Split('-').ToList()[1]}";
            p =
                $@"http://xyz.com:9944/&rack_barcode={master}";

            try
            {
                // Above three lines can be replaced with new helper method below
                string responseBody = client.GetStringAsync(p).GetAwaiter().GetResult();
                list = JsonConvert.DeserializeObject<List<PlateMap>>(responseBody);

                // This is hardcoded, because the first appears just to be a summary
                // But may want to check for WELL_ADDRESS==null
                list.RemoveAt(0);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return list;
        }

        public class PlateMap
        {
            public string PLATE_ID { get; set; }
            public string WELL_ADDRESS { get; set; }
            public string ANUMBER { get; set; }
            public string ASSAY { get; set; }
            public string PLATE_PREP_STATUS { get; set; }
            public int? PREPS_ORDER_ID { get; set; }
            public int? LIBRARY_ID { get; set; }
            public int? RUN_COUNT { get; set; }
            public string CREATED_DATE { get; set; }
            public string CREATED_BY { get; set; }
            public string ECHO_LOGFILE_NAME { get; set; }

            public string VOL_NL { get; set; }
            public string SITE { get; set; }
            public string CONC_UNIT { get; set; }

            public string MODIFIED_DATE { get; set; }
            public string MODIFIED_BY { get; set; }
            public string UPDATED_DATE { get; set; }
            public string UPDATED_BY { get; set; }
            public int? LOT_NUMBER { get; set; }
            public double? CONC { get; set; }

            public string ERROR { get; set; }
            public string TRANSFER_ERROR { get; set; }
            public string BIO_REF_COMPD_STATUS { get; set; }
            public string SAMPLE_ID { get; set; }
            public string DILUTION_NUMBER { get; set; }

            public double? MMW { get; set; }// mono molecular weight
            public double? MW { get; set; }

            public PlateMap DeepCopy()
            {
                // Shallow copy
                var temp = (PlateMap)MemberwiseClone();

                // Deep transfer for strings, classes, and lists
                //temp.PLATE_ID = string.Copy(PLATE_ID);
                temp.ANUMBER = string.Copy(ANUMBER);

                // need to implement more eventually...

                // Return it
                return temp;
            }
        }

        public sealed class csvPlateMap : ClassMap<PlateMap>
        {
            public csvPlateMap()
            {
                var ind = 0;

                // Can order csv by changing order below
                Map(m => m.WELL_ADDRESS).Index(ind); ind += 1;
                Map(m => m.MMW).Index(ind); ind += 1;
                Map(m => m.ANUMBER).Index(ind); ind += 1;
            }
        }
    }
}