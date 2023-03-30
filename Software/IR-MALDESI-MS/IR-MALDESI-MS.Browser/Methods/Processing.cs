using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutomationEngineering.LoggingHelper;
using IR_MALDESI.Browser.Forms;
using IR_MALDESI.Databasing;
using log4net;
using ThermoRawFileReader;

/* Some info here: https://planetorbitrap.com/rawfilereader    */

namespace IR_MALDESI.Browser.Methods
{
    public class Processing
    {
        // Store mz, Intensity, and time data
        public List<timepoint> allData;

        // Store current method
        public MALDESImethod currentMethod;

        // Bool
        public bool sumRange = true;//Sum over PPM range if true. Max over PPM range if false;

        // logging
        private static readonly ILog Log = LogCreator.ClassLogger();

        public class datapoint
        {
            public double mz { get; set; }
            public double I { get; set; }
        }

        // Raise flag if there is an issue
        public bool readRaw(string rawFilePath, MALDESImethod method, int shift)
        {
            // Get file info
            var rawFile = new FileInfo(rawFilePath);
            var jsonFile = new FileInfo($"{rawFilePath.Substring(0, rawFilePath.Length - 4)}.json");
            var mismatchFlag = false;

            // Pass over method
            currentMethod = method;

            // Start with new allData object
            allData = new List<timepoint>();

            // Load JSON
            List<scanInfo> info;
            if (File.Exists(jsonFile.FullName))
            {
                info = JSON.deserializeScanInfo(jsonFile.FullName);
                if (info.Count == 1) updateCompatibility(info);// Must be an item per spot
            }
            else
            {
                MessageBox.Show($@"{jsonFile.FullName} not found. Adding dummy version to continue.");
                info = CreateDummyJson(method);
            }

            // Read
            using (var reader = new XRawFileIO(rawFile.FullName))
            {
                // Pre scan files for shifts or errors
                if (PreScanFiles(method, info, reader, out int scanCountRaw, out int scanCount, out double timeout, ref mismatchFlag)) return true;

                // Export debugging
                var times = ExportTimesDebugging(scanCountRaw, reader, info, jsonFile);

                // Set up progressbar
                var prog = new ProgressForm();
                prog.progressBar1.Value = 0;
                prog.progressBar1.Step = 1;
                prog.Name = "Reading RAW files";
                prog.Show();
                prog.progressBar1.Maximum = info.Count;

                allData = new List<timepoint>();

                // Starting index of raw file is 1 indexed (not 0...)
                var rawNum = 1;
                var jsonIndex = 0;
                double tlast = times[0];
                for (var scanNum = 1; scanNum <= scanCount; scanNum++)
                {
                    // Debug fixing
                    if (scanNum == 10 * 24)
                    {
                        //rawNum = rawNum - 1;
                    }

                    // check timing. Increment (i.e., skip) if the timing between is the low latency timeout
                    bool success = reader.GetScanInfo(rawNum, out clsScanInfo scanInfo);
                    if (method.ScanMode == ScanMode.LowLatHS || method.ScanMode == ScanMode.Ludicrous)
                    {
                        /* 1/28/2022 I'm hoping this is taken care of in acquisition code!

                        // If you have a bunch of timeout low latencies at the start, the first may not match the timeout!
                        if (scanNum == 1 && Math.Abs(times[1] - times[0] - timeout) < 0.001)
                        {
                            rawNum++;
                            tlast = scanInfo.RetentionTime * 60;
                            success = reader.GetScanInfo(rawNum, out scanInfo);
                        }*/

                        var flag = false;
                        while (scanInfo.RetentionTime * 60 - tlast >= timeout - 0.001)
                        {
                            rawNum += 1;
                            tlast = scanInfo.RetentionTime * 60;
                            success = reader.GetScanInfo(rawNum, out scanInfo);

                            //flag = true;
                        }

                        // Debug... take one more on kinetics scan....
                        if (flag)
                        {
                            //rawNum += 1;
                            tlast = scanInfo.RetentionTime * 60;
                            success = reader.GetScanInfo(rawNum, out scanInfo);
                        }
                    }

                    // Shift
                    if (shift != 0 && scanNum == 1)
                    {
                        rawNum += shift;
                        success = reader.GetScanInfo(rawNum, out scanInfo);
                    }

                    /*

                     // Tune file

                    var TuneFile = reader.FileInfo.TuneMethods[0].Settings;
                    var otherInfo = reader.FileInfo;

                    //Timing
                    var asdf = reader.FileInfo.AcquisitionDate;
                    var tt = info[jsonIndex].TimeStamp;
                    reader.GetScanInfo(rawNum, out clsScanInfo ff);
                      */

                    // Get MS data
                    reader.GetScanData(rawNum, out double[] mztemp, out double[] intensitytemp);
                    reader.GetRetentionTime(rawNum, out double RT);

                    // Generate temporary MS data
                    var temp = new timepoint { I = intensitytemp, mz = mztemp, timeFromRaw = RT };

                    // Check for missed raw measurements and skip over json reading for those
                    if (mismatchFlag)
                    {
                        int checkIndex = jsonIndex + info.Select(x => x.spot).Max() + 1;
                        while (checkIndex < info.Count && RT * 60 > info[checkIndex].time)
                        {
                            jsonIndex++;
                            checkIndex = jsonIndex + info.Select(x => x.spot).Max() + 1;
                        }
                    }

                    // Add JSON data to the rest of the object entries
                    foreach (var item in info[jsonIndex].GetType().GetProperties()) temp.GetType().GetProperty(item.Name)?.SetValue(temp, item.GetValue(info[jsonIndex], null), null);

                    // Add filename if need
                    if (temp.Filename == null)
                    {
                        temp.Filename = Path.GetFileName(Path.GetFileNameWithoutExtension(rawFilePath));
                    }

                    // Add Data to list
                    allData.Add(temp);

                    // Update Progress bar
                    prog.progressBar1.PerformStep();

                    // Update raw tracking
                    rawNum++;
                    jsonIndex++;
                    tlast = scanInfo.RetentionTime * 60;
                }
                prog.Close();

                //var differences = times.Zip(times.Skip(1), (x, y) => y - x).ToList();
            }

            return false;
        }

        private static bool PreScanFiles(MALDESImethod method, List<scanInfo> info, XRawFileIO reader, out int scanCountRaw,
            out int scanCount, out double timeout, ref bool mismatchFlag)
        {
            // Compares JSON and .raw. In effect cuts of any extraneous collections at the end of the run.
            int scanCountJson;
            if (method == null)
            {
                scanCountJson = (info.Select(x => x.column).Max() - info.Select(x => x.column).Min() + 1)
                                * (info.Select(x => x.row).Max() - info.Select(x => x.row).Min() + 1)
                                * (info.Select(x => x.spot).Max() + 1);
            }
            else
            {
                scanCountJson = method.wellPlate.Count(x => x.clicked) * method.SpotsPerTrigger;
                if (scanCountJson == 0 && info.Count > 0)// error correction if the clicked scan was not saved
                {
                    scanCountJson = (info.Select(x => x.column).Max() - info.Select(x => x.column).Min() + 1)
                                    * (info.Select(x => x.row).Max() - info.Select(x => x.row).Min() + 1)
                                    * (info.Select(x => x.spot).Max() + 1);
                }
            }

            scanCountRaw = reader.GetNumScans();
            scanCount = Math.Min(scanCountRaw, scanCountJson);

            // Read tune...
            /*
                var mXRawFile = RawFileReaderAdapter.FileFactory(rawFile.FullName);
                mXRawFile.SelectInstrument(ThermoFisher.CommonCore.Data.Business.Device.MS, 1);
                var inn = mXRawFile.GetTuneDataCount();
                var tat = mXRawFile.GetTuneData(0);
                var sss = mXRawFile.GetTuneDataHeaderInformation();
                var ass = mXRawFile.RunHeaderEx;
                var ww = mXRawFile.GetCentroidStream(1,false);
              */

            //Aborted? Total number of scans recorded didn't match what was expected based on range of rows,columns,spots found.
            if (info.Count < scanCountJson)
            {
                MessageBox.Show(@"Info does not match .raw file. Scan was potentially aborted during run. Cannot continue.");
                timeout = 0;
                return true;
            }

            //List<double> diff_t = new List<double>();
            timeout = 666;
            if (scanCountJson == scanCountRaw) return false;

            // Correct for blank spectra from timeout
            if (method.ScanMode == ScanMode.LowLatHS || method.ScanMode == ScanMode.Ludicrous)
            {
                // Prescan for which are real data
                // Very last differential will always be from the timeout - use that to identify other timeouts and remove
                // Up to 3 or even 4 decimals is consistent
                reader.GetScanInfo(scanCountRaw, out clsScanInfo s1);
                reader.GetScanInfo(scanCountRaw - 1, out clsScanInfo s2);
                timeout = (s1.RetentionTime - s2.RetentionTime) * 60;
            }
            else
            {
                MessageBox.Show(
                    $@".raw file count {scanCountRaw} did not match .json file count {scanCountJson}.");

                if (scanCountJson > scanCountRaw) // missed an MS acquisition
                {
                    mismatchFlag = true;

                    // Adjust below by looking at raw times

                    //info.RemoveRange(0, scanCountJson - scanCountRaw);
                }
            }

            return false;
        }

        private static List<double> ExportTimesDebugging(int scanCountRaw, XRawFileIO reader, List<scanInfo> info, FileInfo jsonFile)
        {
            // Export time data for debugging
            var times = new List<double>();
            for (var i = 1; i < scanCountRaw + 1; i++)
            {
                reader.GetScanInfo(i, out clsScanInfo s);
                times.Add(s.RetentionTime * 60);
            }

            try
            {
                // Write
                File.WriteAllLines(
                    @"rawtimes.txt" // <<== Put the file name here
                    , times.Select(x => x.ToString(CultureInfo.InvariantCulture))
                );

                // Export for debug
                File.WriteAllLines(
                    @"jsontimes.txt" // <<== Put the file name here
                    , info.Select(t => t.time.ToString(CultureInfo.InvariantCulture))
                );
                File.SetAttributes("JSON.json", FileAttributes.Normal);
                File.Delete("JSON.json");
                File.Copy(jsonFile.FullName, "JSON.json");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return times;
        }

        private List<scanInfo> CreateDummyJson(MALDESImethod method)
        {
            var info = new List<scanInfo>();
            var moveRight = true;
            for (var i = 0; i < 16; i++)//Rows
            {
                for (var j = 0; j < 24; j++)//Columns
                {
                    int column;
                    if (moveRight)
                    {
                        column = j;
                    }
                    else
                    {
                        column = 24 - 1 - j;
                    }

                    bool result = method.wellPlate.Any(x =>
                    x.row == i && x.column == column && x.clicked);
                    if (!result)
                    {
                        continue;
                    }

                    for (var k = 0; k < method.SpotsPerTrigger; k++)
                    {
                        info.Add(new scanInfo { row = i, column = column, spot = k, time = info.Count });
                    }
                }
                moveRight = !moveRight;
            }

            return info;
        }

        public List<double> getChronogram(double mz = 191.0182, double ppm = 5)
        {
            // Bounds
            double min = mz - ppm * mz / 1e6;
            double max = mz + ppm * mz / 1e6;

            // Populate
            var chron = new List<double>();

            // Parallel.ForEach(allData, s =>
            foreach (var s in allData)
            {
                // Make a list of items so you can use Linq
                GetSinglePeakIntensity(s, min, max, chron);
            }//);

            return chron;
        }

        public void GetSinglePeakIntensity(timepoint s, double min, double max, List<double> chron)
        {
            var j = new List<datapoint>();
            for (var ind = 0; ind < s.I.Length; ind++)
            {
                if (s.mz[ind] > min - 1 && s.mz[ind] < max + 1) // Only add in range
                {
                    j.Add(new datapoint { I = s.I[ind], mz = s.mz[ind] });
                }
            }

            if (sumRange) // Use Linq for sum over range
            {
                var temp = j.Where(x => x.mz > min && x.mz < max);
                /*if (temp.Count()==0)
                {
                    // Issue arises when in profile and your window is within the peak but the nearest points fall outside of the window. This can give a false 0. Do we need to interpolate?
                }*/
                double sump = temp.Sum(x => x.I);
                chron.Add(sump);
            }
            else // Use Linq for max over range
            {
                double maxp = j.Where(x => x.mz > min && x.mz < max).Select(x => x.I).DefaultIfEmpty(0).Max();
                chron.Add(maxp);
            }
        }

        private void updateCompatibility(List<scanInfo> info)
        {
            // info[0].Ncolumns * info[0].Nrows * info[0].SpotsPerTrigger;
        }

        public enum analysisType
        {
            SM,

            PROD,

            IS,

            TIC,
        }

        // Type 0 is SM; Type 1 is PROD ; Type 2 is IS
        public void addAnalysis(double mz, double ppm, analysisType type)
        {
            // First get the chronogram
            var chron = getChronogram(mz, ppm);
            addAnalysis(chron, type);
        }

        // Type 0 is SM; Type 1 is PROD ; Type 2 is IS
        public void addAnalysis(List<double> chron, analysisType type)
        {
            for (var i = 0; i < chron.Count; i++)
            {
                allData[i].GetType().GetProperty(type.ToString())?.SetValue(allData[i], chron[i], null);
            }
        }
    }
}