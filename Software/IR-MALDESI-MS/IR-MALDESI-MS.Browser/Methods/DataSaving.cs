using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using CsvHelper;
using IR_MALDESI.Databasing;

namespace IR_MALDESI.Browser.Methods
{
    internal class DataSaving
    {
        public Forms.Browser _Browser;

        // Well Addressing
        private List<string> RowWellAddress = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF" };
        private List<int> ColumnWellAddress = Enumerable.Range(1, 48).ToList();

        public DataSaving(Forms.Browser temp)
        {
            _Browser = temp;
        }

        /// <summary>
        /// Corrections to well output in generateWriteData
        /// </summary>
        /// <param name="averageSpots"></param>
        /// <param name="ProductNumber"></param>
        public void saveDataPumpTSV(bool averageSpots, int ProductNumber)
        {
            // Average spots if required
            var _csvOutput = generateWriteData(averageSpots);

            // Rows and columns in full well plate (not just selected)
            int rows = _Browser.Processing.currentMethod.wellPlate.Select(x => x.row).Max() + 1;
            int columns = _Browser.Processing.currentMethod.wellPlate.Select(x => x.column).Max() + 1;

            // Make sure all wells are shown
            var r = RowWellAddress.ToList(); r.RemoveRange(rows, r.Count - rows);
            var c = ColumnWellAddress.ToList(); c.RemoveRange(columns, c.Count - columns);
            for (var a = 0; a < r.Count; a++)
            {
                for (var b = 0; b < c.Count; b++)
                {
                    // Fills in 99 if well is not there!
                    if (_csvOutput.Where(x => x.wellAddress == r[a] + c[b]).Count() == 0)
                    {
                        var item = new csvOutput();
                        item.Filename = _csvOutput[0].Filename;
                        item.wellAddress = r[a] + c[b];
                        item.row = a + 1;
                        item.column = b + 1;
                        item.PercentConversion = 99;
                        _csvOutput.Add(item);
                    }
                }
            }

            // Sort output....
            _csvOutput = _csvOutput.OrderBy(x => x.row).ThenBy(x => x.column).ToList();

            // Limit everything below a certain point to 0
            foreach (var item in _csvOutput)
            {
                if (item.PercentConversion < 1e-4) item.PercentConversion = 0;
            }

            // Make folder (if already there this is ignored)
            var parts = new FileInfo(_Browser.RootRawDataFile);
            string path = null;
            if (parts.DirectoryName.Equals("M:\\") || parts.DirectoryName.Equals("C:\\MALDESI_SHARE"))
            {
                path = Path.Combine(parts.DirectoryName, "DataPump");
            }
            else
            {
                //return; // don't pump it up
                path = Path.Combine(parts.DirectoryName, "DataPump");
            }
            Directory.CreateDirectory(path);

            // Write TSV
            string filename = _csvOutput[0].Filename;
            if (averageSpots) filename =
                $"{filename.Substring(0, filename.Length - 4)}_Product{ProductNumber}_datapump.txt";
            else filename = $"{filename.Substring(0, filename.Length - 4)}_Product{ProductNumber}.txt";
            using (var writer = new StreamWriter(Path.Combine(path, filename)))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Register mapping for order of columns in Excel
                csv.Configuration.Delimiter = "\t";
                csv.Configuration.RegisterClassMap<tsvOutputMAP>();
                csv.WriteRecords(_csvOutput);
            }
        }

        public void saveDotmatics(bool averageSpots, int ProductNumber)
        {
            // Make folder (if already there this is ignored)
            var parts = new FileInfo(_Browser.RootRawDataFile);
            string path = null;
            if (parts.DirectoryName.Equals("M:\\") || parts.DirectoryName.Equals("C:\\MALDESI_SHARE"))
            {
                path = Path.Combine(parts.DirectoryName, "Dotmatics");
            }
            else
            {
                //return; // don't pump it up
                path = Path.Combine(parts.DirectoryName, "Dotmatics");
            }
            Directory.CreateDirectory(path);

            // Just copy from prior folders so there's no discrepancy
            string temp = Path.GetFileName(_Browser.RootRawDataFile);
            temp = temp.Substring(0, temp.Length - 4);
            string Source = _Browser.RootRawDataFile.Substring(0, _Browser.RootRawDataFile.Length - 4);
            Source = Path.Combine(Source, $"Product{ProductNumber}", $"{temp}_averageconversionmapDOTMATICS.csv");
            string Destination = Path.Combine(path, $"{temp}_Product{ProductNumber}.csv");

            if (File.Exists(Destination)) File.Delete(Destination);//Remove old version first
            File.Copy(Source, Destination);
        }

        public void saveCSV(bool averageSpots, int ProductNumber)
        {
            // Make based folder (if already there this is ignored)
            string path = _Browser.RootRawDataFile.Substring(0, _Browser.RootRawDataFile.Length - 4);
            Directory.CreateDirectory(path);

            // Add in product number
            path = Path.Combine(path, $"Product{ProductNumber}");
            Directory.CreateDirectory(path);

            // Get filename
            string filename = Path.GetFileName(_Browser.RootRawDataFile);
            if (averageSpots) filename = $"{filename.Substring(0, filename.Length - 4)}_average.csv";
            else filename = $"{filename.Substring(0, filename.Length - 4)}.csv";

            // Average spots if required
            var _csvOutput = generateWriteData(averageSpots, path, filename);

            // Write CSV
            using (var writer = new StreamWriter(Path.Combine(path, filename)))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Register mapping for order of columns in Excel
                csv.Configuration.RegisterClassMap<csvOutputMAP>();
                csv.WriteRecords(_csvOutput);
            }
        }

        internal double CalculateCV(IEnumerable<double> values)
        {
            double standardDeviation = 0;
            double avg = 1;
            var copy = values.Where(x => x >= 0).ToList();// ignore nan
            if (copy.Any())
            {
                // Compute the average.
                avg = copy.Average();//ignore nan

                // Perform the Sum of (value-avg)_2_2.
                double sum = copy.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.
                standardDeviation = Math.Sqrt(sum / (values.Count() - 1));
            }

            return standardDeviation / avg;
        }

        private List<csvOutput> generateWriteData(bool averageSpots, string path = "", string filename = "")
        {
            // Average spots if required
            var writeData = new List<timepoint>();
            var copy = new List<timepoint>();

            // Rows and columns in full well plate (not just selected)
            int rows = _Browser.Processing.currentMethod.wellPlate.Select(x => x.row).Max() + 1;
            int columns = _Browser.Processing.currentMethod.wellPlate.Select(x => x.column).Max() + 1;

            if (averageSpots)
            {
                // Deep copy
                foreach (var c in _Browser.Processing.allData)
                {
                    var clone = new timepoint();
                    foreach (var item in c.GetType().GetProperties()) clone.GetType().GetProperty(item.Name).SetValue(clone, item.GetValue(c, null), null);
                    copy.Add(clone);
                }

                // Calculate averages
                double[] Prod = copy.GroupBy(d => d.row + d.column * columns).ToDictionary(g => g.Key, g => g.Average(d => d.PROD)).Values.ToArray();
                double[] IS = copy.GroupBy(d => d.row + d.column * columns).ToDictionary(g => g.Key, g => g.Average(d => d.IS)).Values.ToArray();
                double[] SM = copy.GroupBy(d => d.row + d.column * columns).ToDictionary(g => g.Key, g => g.Average(d => d.SM)).Values.ToArray();
                double[] TIC = copy.GroupBy(d => d.row + d.column * columns).ToDictionary(g => g.Key, g => g.Average(d => d.TIC)).Values.ToArray();

                // Start populating writeData
                writeData = copy.Where(x => x.spot == 0).ToList();
                if (writeData.Count < Prod.Length) { writeData = copy.Where(x => x.spot == 1).ToList(); };// dropped first spot...

                // Transfer over calculated values
                var i = 0;
                foreach (var w in writeData)
                {
                    w.PROD = Prod[i];
                    w.IS = IS[i];
                    w.SM = SM[i];
                    w.TIC = TIC[i];
                    i++;
                }
            }
            else writeData = _Browser.Processing.allData;

            // Arrange the csv output
            var _csvOutput = new List<csvOutput>();
            foreach (var p in writeData)
            {
                var temp = new csvOutput();
                temp.Filename = Path.GetFileName(p.Filename);
                temp.Time = Math.Round(p.timeFromRaw * 60, 3);// Math.Round(p.time/60,2);//minutes
                temp.Time24hours = Math.Round(double.Parse(p.TimeStamp.Substring(11, 2)) + double.Parse(p.TimeStamp.Substring(13, 2)) / 60, 2);//p.TimeStamp;

                if (p.row == 5 && p.column == 23)
                {
                }

                //temp.wellAddress = (char)(65 + p.row) + (p.column + 1).ToString();
                temp.wellAddress = $"{RowWellAddress[p.row]}{ColumnWellAddress[p.column]}";
                temp.PPM = double.Parse(_Browser.txtRange.Text);
                temp.StartingMaterialmz = double.Parse(_Browser.txtStartingMaterial.Text);
                temp.StartingMaterial = p.SM;
                temp.InternalStandardmz = double.Parse(_Browser.txtInternalStandard.Text);
                temp.InternalStandard = p.IS;
                temp.Productmz = double.Parse(_Browser.ProdCombo.SelectedItem.ToString());
                temp.Product = p.PROD;
                temp.TIC = p.TIC;
                temp.ProductToInternalStandard = p.PROD / p.IS;
                temp.ProductToTIC = p.PROD / p.TIC;
                temp.row = p.row + 1;// For other users
                temp.column = p.column + 1;// For other users
                temp.spot = p.spot + 1;// For other users
                temp.ESISolventName = p.ESISolventName;
                temp.ESISolventFlowRate = p.ESISolventFlowRate;
                temp.AlignmentLaserState = p.AlignmentLaserState;
                temp.PulsesPerBurst = p.PulsesPerBurst;
                temp.BurstsPerSpot = p.BurstsPerSpot;
                temp.SpotsPerTrigger = p.SpotsPerTrigger;
                temp.DelayAfterTrigger = p.DelayAfterTrigger;
                temp.DelayAfterCtrapOpen = p.DelayAfterCtrapOpen;
                temp.DelayBetweenBursts = p.DelayBetweenBursts;
                temp.DelayBetweenSpots = p.DelayBetweenSpots;

                // Find and correct NaN as desired by team
                if (p.PROD + p.SM == 0) temp.PercentConversion = 99;
                else temp.PercentConversion = p.PROD / (p.PROD + p.SM);

                // Note about intensity calculation
                if (_Browser.Processing.sumRange) temp.IntensityCalculation = "Sum over Range";
                else temp.IntensityCalculation = "Max over Range";

                // CV...
                if (averageSpots)
                {
                    //var pp = processing.allData.Where(x => x.row == temp.row - 1 && x.column == temp.column - 1).Select(y => y.PROD ).ToList();
                    //var ss = processing.allData.Where(x => x.row == temp.row - 1 && x.column == temp.column - 1).Select(y => y.SM).ToList();

                    // Percent Conversion
                    var vals = _Browser.Processing.allData.Where(x => x.row == temp.row - 1 && x.column == temp.column - 1).Select(y => y.PROD / (y.SM + y.PROD)).ToList();
                    temp.PercentConversionCV = CalculateCV(vals) * 100;// stdev of only 1 value is 0

                    // SM
                    vals = _Browser.Processing.allData.Where(x => x.row == temp.row - 1 && x.column == temp.column - 1).Select(y => y.SM).ToList();
                    temp.StartingMaterialCV = CalculateCV(vals) * 100;// stdev of only 1 value is 0

                    // Product
                    vals = _Browser.Processing.allData.Where(x => x.row == temp.row - 1 && x.column == temp.column - 1).Select(y => y.PROD).ToList();
                    temp.ProductCV = CalculateCV(vals) * 100;// stdev of only 1 value is 0

                    // IS
                    vals = _Browser.Processing.allData.Where(x => x.row == temp.row - 1 && x.column == temp.column - 1).Select(y => y.IS).ToList();
                    temp.InternalStandardCV = CalculateCV(vals) * 100;// stdev of only 1 value is 0
                }

                _csvOutput.Add(temp);
            }

            // Put together plate map...
            if (averageSpots && path.Equals("") == false)
            {
                for (var i = 0; i < 7; i++)
                {
                    string filepath = Path.Combine(path, filename.Substring(0, filename.Length - 4));
                    switch (i)
                    {
                        case 0: filepath += "productmap.csv"; break;
                        case 1: filepath += "internalstandardmap.csv"; break;
                        case 2: filepath += "startingmaterialmap.csv"; break;
                        case 3: filepath += "conversionmap.csv"; break;
                        case 4: filepath += "productstandardratiomap.csv"; break;
                        case 5: filepath += "productTICratiomap.csv"; break;
                        case 6: filepath += "conversionmapDOTMATICS.csv"; break;
                    }

                    saveCSVheatmap(filepath, writeData, i);
                }
            }

            return _csvOutput;
        }

        private void saveCSVheatmap(string filepath, List<timepoint> writeData, int type)
        {
            // Rows and columns in full well plate (not just selected)
            int rows = _Browser.Processing.currentMethod.wellPlate.Select(x => x.row).Max() + 1;
            int columns = _Browser.Processing.currentMethod.wellPlate.Select(x => x.column).Max() + 1;

            using (var writer = new StreamWriter(new FileStream(filepath,
                 FileMode.Create, FileAccess.Write)))
            {
                if (type != 6) writer.WriteLine("sep=,");
                for (var i = 0; i < rows + 1; i++)
                {
                    var s = "";
                    for (var j = 0; j < columns; j++)
                    {
                        if (i == 0 && type != 6)// Header
                        {
                            if (j == 0) s += " ,";
                            if (j <= columns) s += $"{j + 1},";
                            else s += i.ToString();
                        }
                        else// Actual data
                        {
                            if (j == 0 && type != 6) s += $"{(char)(64 + i)},";// Header
                            if (j <= columns)
                            {
                                IEnumerable<timepoint> temp;
                                if (type != 6) temp = writeData.Where(x => x.row == i - 1 && x.column == j);
                                else temp = writeData.Where(x => x.row == i && x.column == j);

                                var pass = "";
                                if (temp.Count() != 0)
                                {
                                    var data = temp.First();

                                    switch (type)
                                    {
                                        case 0:
                                            pass = data.PROD.ToString();
                                            break;

                                        case 1:
                                            pass = data.IS.ToString();
                                            break;

                                        case 2:
                                            pass = data.SM.ToString();
                                            break;

                                        case 3:
                                            pass = (data.PROD / (data.PROD + data.SM)).ToString();
                                            break;

                                        case 4:
                                            pass = (data.PROD / data.IS).ToString();
                                            break;

                                        case 5:
                                            pass = (data.PROD / data.TIC).ToString();
                                            break;

                                        case 6:// Dotmatics
                                            pass = (data.PROD / (data.PROD + data.SM)).ToString();
                                            break;
                                    }
                                    pass = pass.Replace("∞", "INF");
                                }
                                s += $"{pass},";
                            }
                            else s += i.ToString();
                        }
                    }
                    writer.WriteLine(s);
                }
            }
        }

        internal void saveHeatmap()
        {
            // Make folder (if already there this is ignored)
            string path = _Browser.RootRawDataFile.Substring(0, _Browser.RootRawDataFile.Length - 4);
            Directory.CreateDirectory(path);

            // Get fullName
            string fullName = Path.Combine(path, Path.GetFileName(path));
            if (_Browser.checkBoxProduct.Checked) fullName += "_Product";
            if (_Browser.checkBoxIS.Checked) fullName += "_InternalStandard";
            if (_Browser.checkBoxSM.Checked) fullName += "_StartingMaterial";
            if (_Browser.checkBoxConv.Checked) fullName += "_Conversion";
            if (_Browser.checkBoxRatio.Checked) fullName += "_ProductToInternalStandard";
            if (_Browser.checkBoxRatioTIC.Checked) fullName += "_ProductToTIC";
            if (_Browser.checkBoxLog.Checked) fullName += "_Log10";
            fullName += ".png";

            // Bring form to front
            _Browser.BringToFront();

            // Take screenshot
            var bounds = new Rectangle();
            bounds.X = _Browser.dataGridView1.Left + _Browser.Left + 8;
            bounds.Y = _Browser.dataGridView1.Top + _Browser.Top + _Browser.fileToolStripMenuItem.Height + 13;
            bounds.Width = _Browser.dataGridView1.Width;
            bounds.Height = _Browser.dataGridView1.Height + 17;

            // Save as bitmap
            using (var bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, new Size(bounds.Width, bounds.Height));
                }
                bitmap.Save(fullName, ImageFormat.Png);
            }
        }

        public void SavePlateMapHits(bool averageSpots, bool ProteinBinding)
        {
            // Rows and columns in full well plate (not just selected)
            int rows = _Browser.Processing.currentMethod.wellPlate.Select(x => x.row).Max() + 1;
            int columns = _Browser.Processing.currentMethod.wellPlate.Select(x => x.column).Max() + 1;

            // Make sure all wells are shown
            var r = RowWellAddress.ToList(); r.RemoveRange(rows, r.Count - rows);
            var c = ColumnWellAddress.ToList(); c.RemoveRange(columns, c.Count - columns);

            // Average spots if required
            //var _csvOutput = generateWriteData(averageSpots);
            var temp = new List<csvOutput>();
            foreach (var timepoint in _Browser.Processing.allData)
            {
                var item = new csvOutput
                {
                    Filename = timepoint.Filename,
                    wellAddress = r[timepoint.row] + c[timepoint.column],
                    AlignmentLaserState = timepoint.AlignmentLaserState,
                    Product = timepoint.PROD,
                    StartingMaterial = timepoint.SM,
                    InternalStandard = timepoint.IS,
                    row = timepoint.row,
                    column = timepoint.column,
                };
                temp.Add(item);
            }

            for (var a = 0; a < r.Count; a++)
            {
                for (var b = 0; b < c.Count; b++)
                {
                    if (temp.Any(x => x.wellAddress == r[a] + c[b])) continue;

                    // Fills in 99 if well is not there!
                    var item = new csvOutput
                    {
                        Filename = "Empty",
                        wellAddress = r[a] + c[b],
                        row = a,// Zero indexed
                        column = b,// Zero indexed
                        PercentConversion = 99
                    };
                    temp.Add(item);
                }
            }

            // Average all of same A number
            var _csvOutput = new List<csvOutput>();
            foreach (string s in temp.Select(x => x.Filename).Distinct())
            {
                var SameANumber = temp.Where(x => x.Filename == s).ToList();// could be different well
                var DistinctWellAddress = SameANumber.Select(x => x.wellAddress).Distinct();// Find distinct wells with same A number

                foreach (string s1 in DistinctWellAddress)
                {
                    var subset = SameANumber.Where(x => x.wellAddress == s1).ToList();

                    _csvOutput.Add
                    (
                        new csvOutput
                        {
                            Filename = s,
                            wellAddress = subset.First().wellAddress,
                            AlignmentLaserState = subset.Any(x => x.AlignmentLaserState),
                            Product = (int)subset.Select(x => x.Product).Average(),
                            StartingMaterial = (int)subset.Select(x => x.StartingMaterial).Average(),
                            InternalStandard = (int)subset.Select(x => x.InternalStandard).Average(),
                            row = subset.First().row,
                            column = subset.First().column,
                            /* bugs out if all zero
                            Product = all.Where(x => x.Product > 0).Select(x => x.Product).Average(),
                            StartingMaterial = all.Where(x => x.StartingMaterial > 0).Select(x => x.StartingMaterial).Average(),
                            InternalStandard = all.Where(x => x.InternalStandard > 0).Select(x => x.InternalStandard).Average(),
                            */
                        });
                }
            }

            // Sort output....
            _csvOutput = _csvOutput.OrderBy(x => x.row).ThenBy(x => x.column).ToList();

            // Limit everything below a certain point to 0
            foreach (var item in _csvOutput)
            {
                if (item.PercentConversion < 1e-4) item.PercentConversion = 0;
            }

            // Make folder (if already there this is ignored)
            var parts = new FileInfo(_Browser.RootRawDataFile);
            string path = null;
            if (parts.DirectoryName.Equals("M:\\") || parts.DirectoryName.Equals("C:\\MALDESI_SHARE"))
            {
                path = Path.Combine(parts.DirectoryName, "PlateMapHits");
            }
            else
            {
                path = Path.Combine(parts.DirectoryName, "PlateMapHits");
            }
            Directory.CreateDirectory(path);

            // Write TSV
            string filename = Path.GetFileName(Path.GetFileNameWithoutExtension(_Browser.RootRawDataFile));
            if (averageSpots) filename = $"{filename.Substring(0, filename.Length - 4)}.txt";
            else filename = $"{filename.Substring(0, filename.Length - 4)}.txt";
            using (var writer = new StreamWriter(Path.Combine(path, filename)))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Register mapping for order of columns in Excel
                csv.Configuration.Delimiter = "\t";
                if (ProteinBinding) csv.Configuration.RegisterClassMap<ProteinaceousOutputMAP>();
                else csv.Configuration.RegisterClassMap<CYPOutputMAP>();
                csv.WriteRecords(_csvOutput);
            }
        }
    }
}