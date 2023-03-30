using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace IR_MALDESI.Databasing
{
    public static class JSON
    {
        #region Serialize

        public static void serializeCOMports(COM COMports)
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter($@"..\..\Files\COMports_{Environment.MachineName}.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, COMports);
            }
        }

        public static void serializeCalibrations(Calibrations cal)
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter($@"..\..\Files\Calibrations_{Environment.MachineName}.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, cal);
            }
        }

        public static void serializeFormFields(scanInfo formInfo)
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(@"..\..\Files\formInfo.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, formInfo);
            }
        }

        public static void serializeScanInfo(string location, List<scanInfo> info)
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(location))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, info);
            }
        }

        public static void serializeMALDESImethod(string location, MALDESImethod info)
        {
            var serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(location))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, info);
            }
        }

        #endregion Serialize

        #region deserialize

        public static COM deserializeCOMports()
        {
            // Filename
            var file = $@"..\..\Files\COMports_{Environment.MachineName}.json";

            // Update to including instrument name if needed.
            if (!File.Exists(file))
            {
                serializeCOMports(JsonConvert.DeserializeObject<COM>(File.ReadAllText(@"..\..\Files\COMports.json")));
            }

            return JsonConvert.DeserializeObject<COM>(File.ReadAllText(file));
        }

        public static Calibrations deserializeCalibrations()
        {
            // Filename
            var file = $@"..\..\Files\Calibrations_{Environment.MachineName}.json";

            // Update to including instrument name if needed.
            if (!File.Exists(file))
            {
                serializeCalibrations(JsonConvert.DeserializeObject<Calibrations>(File.ReadAllText(@"..\..\Files\Calibrations.json")));
            }

            return JsonConvert.DeserializeObject<Calibrations>(File.ReadAllText(file));
        }

        public static scanInfo deserializeFormFields()
        {
            return JsonConvert.DeserializeObject<scanInfo>(File.ReadAllText(@"..\..\Files\formInfo.json"));
        }

        public static List<scanInfo> deserializeScanInfo(string fullPath)
        {
            return JsonConvert.DeserializeObject<List<scanInfo>>(File.ReadAllText(fullPath));
        }

        public static MALDESImethod deserializeMALDESImethod(string fullPath)
        {
            var method = new MALDESImethod();
            try
            {
                method = JsonConvert.DeserializeObject<MALDESImethod>(File.ReadAllText(fullPath));
            }
            catch
            {
                var temp = JsonConvert.DeserializeObject<MALDESImethoddb2>(File.ReadAllText(fullPath));

                // Transfer over to method file
                foreach (var item in temp.GetType().GetProperties())
                {
                    method.GetType().GetProperty(item.Name)?.SetValue(method, item.GetValue(temp, null), null);
                }
                var xm = new XMethod();
                method.XMethod = xm.GetXMethod(temp.Xmethod);

                method.wellPlate = temp.wellPlate;
            }

            return method;
        }

        #endregion deserialize
    }
}