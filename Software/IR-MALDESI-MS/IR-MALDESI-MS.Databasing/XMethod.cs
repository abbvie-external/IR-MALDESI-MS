using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomationEngineering.LoggingHelper;
using log4net;

namespace IR_MALDESI.Databasing
{
    public class XMethod
    {
        // logging
        private static readonly ILog Log = LogCreator.ClassLogger();

        public string FullFile { get; set; }
        public string File { get; set; }
        public string[] FullXMethodText { get; set; }
        public string Polarity { get; set; }
        public int Resolution { get; set; }
        public string ScanRange { get; set; }
        public string Tunefile { get; set; }
        public double Duration { get; set; }//minutes
        public double MaximumIT { get; set; }//ms
        public int Microscans { get; set; }
        public bool? isProfile { get; set; }// Is data profile or centroid

        public XMethod GetXMethod(string FullFileName)
        {
            var xMethod = new XMethod();

            // See if you can find the file.
            if (!System.IO.File.Exists(FullFileName))
            {
                return xMethod;
            }
            xMethod.FullXMethodText = System.IO.File.ReadAllLines(FullFileName);
            xMethod.FullFile = FullFileName;
            xMethod.File = Path.GetFileName(FullFileName);

            ParseXMethodText(xMethod);

            return xMethod;
        }

        public static void ParseXMethodText(XMethod xMethod)
        {
            // Parse full XMethod
            foreach (string s in xMethod.FullXMethodText)
            {
                string line = s.Replace("\0", "");

                if (line.Contains("Method duration") && xMethod.Duration == 0)
                {
                    xMethod.Duration =
                        double.Parse(line.Remove(line.Length - 3).Remove(0, 20).Replace(" ", "").Replace(",", ""));
                }

                if (line.Contains("Spectrum") && xMethod.isProfile == null)
                {
                    xMethod.isProfile = line.Remove(0, 20).Replace(" ", "").Replace(",", "").ToLower() == "profile";
                }

                if (line.Contains("Polarity") && xMethod.Polarity == null)
                {
                    xMethod.Polarity = line.Remove(0, 20).Replace(" ", "").Replace(",", "");
                }

                if (line.Contains("Resolution") && xMethod.Resolution == 0)
                    xMethod.Resolution = int.Parse(line.Remove(0, 20).Replace(" ", "").Replace(",", ""));
                if (line.Contains("Microscans") && xMethod.Microscans == 0)
                    xMethod.Microscans = int.Parse(line.Remove(0, 20).Replace(" ", "").Replace(",", ""));
                if (line.Contains("Maximum IT") && xMethod.MaximumIT == 0)
                    xMethod.MaximumIT =
                        double.Parse(line.Remove(line.Length - 2).Remove(0, 20).Replace(" ", "").Replace(",", ""));
                if (line.Contains("Scan range") && xMethod.ScanRange == null)
                    xMethod.ScanRange = line.Remove(0, 20).Replace(" ", "").Replace(",", "");
                if (line.Contains("Base Tunefile") && xMethod.Tunefile == null)
                    xMethod.Tunefile = line.Remove(0, 13).Replace(" ", "").Replace(",", "");
            }
        }
    }
}