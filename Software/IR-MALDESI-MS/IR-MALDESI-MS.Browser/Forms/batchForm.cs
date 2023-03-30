using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CsvHelper;
using IR_MALDESI.Databasing;
using WK.Libraries.BetterFolderBrowserNS;

namespace IR_MALDESI.Browser.Forms
{
    public partial class BatchForm : Form
    {
        private readonly Browser _core;

        public string Path { get; private set; }

        public BatchForm(Browser temp)
        {
            _core = temp;
            InitializeComponent();
        }

        private void batchForm_Load(object sender, EventArgs e)
        {
            setPath.PerformClick();
        }

        private void setPath_Click(object sender, EventArgs e)
        {
            var fbd = new BetterFolderBrowser();
            fbd.RootFolder = "C:\\MALDESI_SHARE";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Path = fbd.SelectedPath;
            }

            // Show path
            pathLabel.Text = Path;

            // Populate list
            restore.PerformClick();
        }

        private void load_Click(object sender, EventArgs e)
        {
            if (list.SelectedIndex >= 0) _core.ShowFile(System.IO.Path.Combine(Path, list.SelectedItem.ToString()));
        }

        private void remove_Click(object sender, EventArgs e)
        {
            // Remove
            int index = list.SelectedIndex;

            //list.Items.RemoveAt(list.SelectedIndex);

            var original = list.SelectedItems.OfType<object>().ToList();
            foreach (object l in original)
            {
                list.Items.Remove(l);
            }

            // Where to leave marker
            if (list.Items.Count > 0)
            {
                if (index > 0) list.SelectedIndex = index - 1;
                else
                {
                    list.SelectedIndex = 0;
                }
            }
        }

        private void restore_Click(object sender, EventArgs e)
        {
            if (Path == null) return;

            // Get files
            var files = Directory.GetFiles(Path, "*.raw").ToList();

            // Sort by time
            var time = new List<double>();
            foreach (string[] split in files.Select(f => f.Substring(f.Length - 21, 17).Split('_')))
            {
                if (split.Length != 4)
                {
                    time.Add(0);
                }
                else time.Add(double.Parse(split[0]) * 365 + double.Parse(split[1]) * 31 + double.Parse(split[2]) + double.Parse(split[3]) / 1e6);
            }
            var sorted = time
                .Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key)
                .ToList();
            var idx = sorted.Select(x => x.Value).ToList();

            // Populate
            list.Items.Clear();
            foreach (int i in idx)
            {
                var fi = new FileInfo(files[i]);
                list.Items.Add(fi.Name);
            }
        }

        private void batchCombine_Click(object sender, EventArgs e)
        {
            // Selected export folder
            var fbd = new BetterFolderBrowser();
            fbd.RootFolder = Path;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
            }
            else return;

            // First perform a batch export
            batchExportMethod(false);

            // Combine!
            var selected = list.SelectedIndices.Cast<int>().ToList();
            if (!selected.Any()) return;
            list.ClearSelected();
            var _csvOutput = new List<csvOutput>();
            foreach (int i in selected)
            {
                // Load just average for now
                string f1 = System.IO.Path.GetFileNameWithoutExtension(list.Items[i].ToString());
                string f = System.IO.Path.Combine(Path, f1, "Product1", $"{f1}_average.csv");

                // Load csv
                var rows = File.ReadAllLines(f).Select(x => x.Split(',')).ToList();

                // Find time from name and add in
                double t0 = double.Parse(f1.Substring(f1.Length - 6, 2)) * 3600 +
                            double.Parse(f1.Substring(f1.Length - 4, 2)) * 60 +
                            double.Parse(f1.Substring(f1.Length - 2, 2));

                // Start with 2nd and append
                for (var index = 1; index < rows.Count; index++)
                {
                    string[] row = rows[index];

                    _csvOutput.Add(
                        new csvOutput
                        {
                            Filename = row[0],
                            wellAddress = row[1],
                            Time = double.Parse(row[2]) + t0,
                            Time24hours = double.Parse(row[3]),
                            PPM = double.Parse(row[4]),
                            IntensityCalculation = row[5],
                            StartingMaterialmz = double.Parse(row[6]),
                            StartingMaterial = double.Parse(row[7]),

                            //StartingMaterialCV = double.Parse(row[8]),
                            Productmz = double.Parse(row[9]),
                            Product = double.Parse(row[10]),

                            // ProductCV = double.Parse(row[11]),
                            InternalStandardmz = double.Parse(row[12]),
                            InternalStandard = double.Parse(row[13]),

                            //InternalStandardCV = double.Parse(row[14]),
                            TIC = double.Parse(row[15]),
                            PercentConversion = double.Parse(row[16]),
                            PercentConversionCV = double.Parse(row[17]),

                            //ProductToInternalStandard = double.Parse(row[18]),
                            ProductToTIC = double.Parse(row[19]),
                            row = int.Parse(row[20]),
                            column = int.Parse(row[21]),
                            spot = int.Parse(row[22]),

                            //ESISolventName = row[23],
                            ESISolventFlowRate = int.Parse(row[24]),
                            AlignmentLaserState = bool.Parse(row[25]),
                            PulsesPerBurst = int.Parse(row[26]),
                            BurstsPerSpot = int.Parse(row[27]),
                            SpotsPerTrigger = int.Parse(row[28]),
                            DelayAfterTrigger = int.Parse(row[29]),
                            DelayAfterCtrapOpen = int.Parse(row[30]),
                            DelayBetweenBursts = int.Parse(row[31]),
                            DelayBetweenSpots = int.Parse(row[32]),
                        }
                        );
                }
            }

            // Subtract off minimum time...
            double t = _csvOutput.Min(x => x.Time);
            foreach (var csvOutput in _csvOutput)
            {
                csvOutput.Time -= t;
            }

            // Sort by well address
            _csvOutput = _csvOutput.OrderBy(x => x.wellAddress).ThenBy(x => x.Time).ToList();

            // Write CSV
            using (var writer = new StreamWriter(System.IO.Path.Combine(fbd.SelectedPath, "Kinetics.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Register mapping for order of columns in Excel
                csv.Configuration.RegisterClassMap<csvOutputMAP>();
                csv.WriteRecords(_csvOutput);
            }
        }

        private void batchExport_Click(object sender, EventArgs e)
        {
            batchExportMethod();
        }

        private void batchExportMethod(bool clearList = true)
        {
            var selected = list.SelectedIndices.Cast<int>().ToList();
            list.ClearSelected();

            // Can't handle more than one for plate map
            if (_core.ProdCombo.Items.Count > 1 && _core.AnalysisMode.SelectedIndex >= 1)
            {
                MessageBox.Show(@"This mode cannot deal with multiple reference masses at this time. Please remove all but the desired before continuing.");
                return;
            }

            foreach (int i in selected)
            {
                // Show which file you are on
                list.SelectedIndex = i;

                var productNumber = 0;
                foreach (object p in _core.ProdCombo.Items)
                {
                    // Handle plate map version...
                    if (productNumber > 0 && _core.AnalysisMode.SelectedIndex >= 1) continue;

                    // Change current product item
                    _core.ProdCombo.SelectedItem = p;
                    productNumber++;

                    // Load the file (gets the current displayed mass values
                    bool success = _core.LoadDataFiles(System.IO.Path.Combine(Path, list.Items[i].ToString()));

                    if (success)
                    {
                        switch (_core.AnalysisMode.SelectedIndex)
                        {
                            case 0:

                                // Save individual spots and averaged spots
                                _core.DataSaving.saveCSV(false, productNumber);
                                _core.DataSaving.saveCSV(true, productNumber); // This changes allData unfortunately

                                // Datapump
                                _core.DataSaving.saveDataPumpTSV(true, productNumber);

                                // Dotmatics
                                _core.DataSaving.saveDotmatics(true, productNumber);
                                break;

                            case 1:
                                _core.DataSaving.SavePlateMapHits(true, false);

                                break;

                            case 2:
                                _core.DataSaving.SavePlateMapHits(true, true);

                                break;
                        }
                    }
                }
            }

            if (clearList) list.ClearSelected();
        }

        private void openFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Path);
        }
    }
}