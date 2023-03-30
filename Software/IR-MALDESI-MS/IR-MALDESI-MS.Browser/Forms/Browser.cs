using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomationEngineering.LoggingHelper;
using CsvHelper;
using CsvHelper.Configuration;
using Emgu.CV;
using Emgu.CV.Structure;
using IR_MALDESI.Browser.Methods;
using IR_MALDESI.Databasing;
using log4net;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using ThermoFisher.CommonCore.Data;
using static System.Double;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static IR_MALDESI.Databasing.wellPlate;
using Button = System.Windows.Forms.Button;
using ProgressBar = System.Windows.Forms.ProgressBar;

namespace IR_MALDESI.Browser.Forms
{
    public partial class Browser : Form
    {
        #region Variable declaration

        // logging
        private static readonly ILog Log = LogCreator.ClassLogger();

        // Root RawDataFile
        public string RootRawDataFile { get; set; } = "";

        // Forms
        private DebugForm _debugForm;

        // Default Plate size is 384
        private int numWells = 384;
        private int numRows = 16;
        private int numCols = 24;

        // Maximum number of wells to display
        private int maxNumSamples = 384;

        // Current selection
        public int CurrentRow, CurrentColumn;
        private int currentSpot;

        // Chronogram
        private List<double> TICchron;
        private List<double> prodEICchron;
        private List<double> smEICchron;
        private List<double> isEICcrhon;

        // Processing
        internal Processing Processing;

        // Database
        private Database db = new Database();

        // Data processing
        internal DataSaving DataSaving;

        // Re-run
        private List<wellPlateStatus> rerun;

        private List<string> rowWellAddress;

        private class BrowserFormData
        {
            public string TxtRange;
            public string TxtStartingMaterial;
            public string TxtInternalStandard;
            public List<string> ProdCombo;
        }

        #endregion Variable declaration

        #region Initialization and exit

        public Browser()
        {
            InitializeComponent();
        }

        private void Browser_Load(object sender, EventArgs e)
        {
            // Populate well address
            rowWellAddress = new List<string>
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
                "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF"
            };

            // Get prior form data / mass values
            if (File.Exists(@"browserFormData.json"))
            {
                var formData = JsonConvert.DeserializeObject<BrowserFormData>(File.ReadAllText(@"browserFormData.json"));
                txtRange.Text = formData.TxtRange;
                txtStartingMaterial.Text = formData.TxtStartingMaterial;
                txtInternalStandard.Text = formData.TxtInternalStandard;

                if (formData.ProdCombo.Count > 0)
                {
                    ProdCombo.Items.Clear();

                    foreach (string s in formData.ProdCombo) ProdCombo.Items.Add(s);
                }
            }

            // Initialize the analysis mode
            AnalysisMode.SelectedIndex = 0;

            // Initialize Data Grid
            var wp = new wellPlate();// Functions to deal with wellPlate drawing
            wp.InitializeDataGrid(dataGridView1, numRows, numCols);
            dataGridView1.Font = new Font("Arial", 6);

            // Set up re-run to be full size
            rerun = wp._wellPlateStatus;

            // Select first product item
            ProdCombo.SelectedIndex = 0;

            // Pass everything to data processing
            DataSaving = new DataSaving(this);

            // Move to top
            Top = 0;
        }

        private void Browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            var formData = new BrowserFormData
            {
                TxtRange = txtRange.Text,
                TxtStartingMaterial = txtStartingMaterial.Text,
                TxtInternalStandard = txtInternalStandard.Text,
                ProdCombo = ProdCombo.Items.Cast<string>().Select(item => item.ToString()).ToList()
            };

            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            using (var sw = new StreamWriter(@"browserFormData.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, formData);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exit Application
            Application.Exit();
        }

        #endregion Initialization and exit

        #region Processing

        private void loadDataFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Don't open again if already open
            if (Application.OpenForms.OfType<BatchForm>().Any()) return;

            var frm = new BatchForm(this);
            frm.Show();
            frm.Left = Left;
            frm.Top = Top + 751;
            if (frm.Path == null) frm.Close();
        }

        public void ShowFile(string rootRawDataFile)
        {
            // Convert if old database file there
            ConvertOldDatabase(rootRawDataFile);

            //txtProduct.Enabled = true;
            chkProduct.Enabled = true;
            if (LoadDataFiles(rootRawDataFile))
            {
                int row = Processing.allData.Select(x => x.row).Min();
                int column = Processing.allData.Where(y => y.row == row).Select(x => x.column).Min();
                WellClicked(row, column);
                int spots = Processing.allData.Select(x => x.spot).Max() + 1;
                double seconds = Processing.allData.Select(x => x.time).Max() / (Processing.allData.Count / spots) * 384;
                lblChromatogram.Text = $@"Chronogram: {seconds / 60:f2} minutes ({seconds:f2} seconds) per 384 well plate for {spots} spots/well";
            }
            else
            {
                MessageBox.Show(@"File did not open successfully!");
                return;
            }

            // Initialize Data Grid
            var method = db.GetMaldesiMethod();
            XMethod.ParseXMethodText(method.XMethod);// Rescans the method file in case you added something (like isProfile);
            var wp = new wellPlate();// Functions to deal with wellPlate drawing
            numWells = method.wellPlate.Count;
            maxNumSamples = method.wellPlate.Count;
            switch (numWells)
            {
                case 384: // 384

                    dataGridView1.Font = new Font("Arial", 6);
                    break;

                case 96: // 96

                    dataGridView1.Font = new Font("Arial", 10);
                    break;

                case 1536: // 1536

                    dataGridView1.Font = new Font("Arial", 3);
                    break;
            }
            numRows = method.wellPlate.Select(x => x.row).Max() + 1;
            numCols = method.wellPlate.Select(x => x.column).Max() + 1;
            wp.InitializeDataGrid(dataGridView1, numRows, numCols);

            // Display
            currentSpot = 0;
            updateDisplay();

            Text = $@"IR-MALDESI Data Browser - {rootRawDataFile.Substring(0, rootRawDataFile.Length - 4)}.raw";
        }

        private void ConvertOldDatabase(string rootRawDataFile)
        {
            if (!File.Exists($"{rootRawDataFile.Substring(0, rootRawDataFile.Length - 4)}.db2") &&
                File.Exists($"{rootRawDataFile.Substring(0, rootRawDataFile.Length - 4)}.db"))
            {
                // Set up progressbar
                var prog = new ProgressForm();
                prog.progressBar1.Value = 50;
                prog.progressBar1.Maximum = 100;
                prog.Show();
                Application.DoEvents();

                // Load current database, convert to db2, close current database, and delete the file
                db.LoadDatabase($"{rootRawDataFile.Substring(0, rootRawDataFile.Length - 4)}.db");
                db.Convertdb();
                db.CloseDatabase();
                File.Delete($"{rootRawDataFile.Substring(0, rootRawDataFile.Length - 4)}.db");

                prog.Close();
            }
        }

        // True means good...
        public bool LoadDataFiles(string rawDataFile1)
        {
            // Set flag
            const bool fileOpenOk = true;

            // Set rootdata file
            RootRawDataFile = rawDataFile1;
            Log.Debug($"Loading {rawDataFile1}");

            // New processing instance
            var sumRange = true;
            if (Processing != null) sumRange = Processing.sumRange;
            Processing = new Processing
            {
                sumRange = sumRange // Pass from old instance
            };

            // Load database
            if (File.Exists($"{RootRawDataFile.Substring(0, RootRawDataFile.Length - 4)}.db2")) db.LoadDatabase(
                $"{RootRawDataFile.Substring(0, RootRawDataFile.Length - 4)}.db2");

            // load the full dataset
            var method = db.GetMaldesiMethod();
            if (Processing.readRaw(rawDataFile1, method, int.Parse(Shift.Text))) // loaded data in processing.allData
            {
                return false;
            }

            // Proteinaceous
            if (AnalysisMode.SelectedIndex == 2)
            {
                var ss = Proteinaceous.LoadCSV(Path.GetFullPath(db.CurrentFile));

                // Transfer deconvoluted over to processing.allData
                for (var spotIndex = 0; spotIndex < Processing.allData.Count; spotIndex++)
                {
                    var timepoint = Processing.allData[spotIndex];
                    var mz = new List<double>();
                    var I = new List<double>();
                    for (var i = 0; i < ss.mass.Count; i++)// each mass
                    {
                        mz.Add(ss.mass[i]);
                        I.Add(ss.Intensity[i][spotIndex]);
                    }

                    // Pass over
                    timepoint.mz = mz.ToArray();
                    timepoint.I = I.ToArray();
                }
            }

            // Set up progressbar
            var prog = new ProgressForm();
            prog.progressBar1.Value = 1;
            prog.progressBar1.Step = 1;
            prog.Show();
            prog.progressBar1.Maximum = 4;

            // Calculate intensities
            CalculateIntensity(prog);

            // Clear mass spectrum chart
            lblSpectrum.Text = @"Mass spectrum:";
            hint.Visible = true;

            // Clear status labels
            ClearStatusLabels();

            // Close progress
            prog.Close();

            // Check the Product image
            //checkBoxProduct.Checked = true;

            // Done
            return fileOpenOk;
        }

        private void CalculateIntensity(ProgressForm prog)
        {
            // Get the method
            var method = db.GetMaldesiMethod();

            // Get mass addition
            double adductAddition = 0; double hydrogenAddition = 0;
            switch (method.XMethod.Polarity)
            {
                case "positive":
                    hydrogenAddition = 1.008;
                    adductAddition = 22.990;
                    break;

                case "Positive":
                    hydrogenAddition = 1.008;
                    adductAddition = 22.990;
                    break;

                case "negative":
                    hydrogenAddition = -1.008;
                    adductAddition = 34.969;
                    break;

                case "Negative":
                    hydrogenAddition = -1.008;
                    adductAddition = 34.969;
                    break;

                default:// Assume positive and throw error to log
                    Log.Error("No polarity found in method. Defaulting to positive mode.");
                    hydrogenAddition = -1.008;
                    adductAddition = 34.969;
                    break;
            }

            switch (AnalysisMode.SelectedIndex)
            {
                case 0://standard
                    {
                        StandardAnalysis(prog);
                        break;
                    }
                case 1:// plate map compound search
                    {
                        // Get the method
                        bool positiveMode = method.XMethod.Polarity.ToLower() == "positive";

                        // Mass references
                        TryParse(CustomAdduct.Text, out double customAdduct);
                        if (positiveMode)
                        {
                            hydrogenAddition = 1.008;
                            adductAddition = 22.990;
                        }
                        else
                        {
                            hydrogenAddition = -1.008;
                            adductAddition = 34.969;
                        }

                        PlateMapAnalysis(method, hydrogenAddition, adductAddition, customAdduct, Parse(txtRange.Text), true, false);
                        break;
                    }

                case 2:// proteinaceous deconvolution
                    {
                        // Mass references
                        hydrogenAddition = Parse(ProdCombo.SelectedItem.ToString());// Actually referencer mass

                        PlateMapAnalysis(method, hydrogenAddition, 0, 0, Parse(txtRange.Text), false, false);

                        break;
                    }
                case 3: // custom analysis
                    {
                        CustomAnalysis(method, Parse(txtRange.Text), hydrogenAddition, prog);
                        break;
                    }
            }
        }

        private void CustomAnalysis(MALDESImethod method, double window, double _hydrogenAddition,
            ProgressForm progressForm)
        {
            // Pull the plate map
            if (PullPlateMap(method, out var list)) return;

            // Start building string
            var csv = new StringBuilder();

            // Progress form
            progressForm.progressBar1.Value = 2;

            // Go through list
            var lockObj = new object();
            Parallel.ForEach(list, item =>
                   {
                       // Get window
                       var mz = (double)item.MMW;
                       mz += _hydrogenAddition;
                       double min = mz - window * mz / 1e6;
                       double max = mz + window * mz / 1e6;

                       // Gets r and c from list, but this is't correct in ASMS analysis
                       int r = rowWellAddress.IndexOf(item.WELL_ADDRESS.Substring(0, 1));
                       int c = int.Parse(item.WELL_ADDRESS.Substring(1, item.WELL_ADDRESS.Length - 1)) - 1;

                       // Get signal intensities
                       var signal = new List<double>();
                       var signalSpectrum = Processing.allData.Where(x => x.row == r && x.column == c).ToList();
                       foreach (var temp in signalSpectrum)
                       {
                           Processing.GetSinglePeakIntensity(temp, min, max, signal);
                       }

                       // Get noise intensities
                       var noise = new List<double>();
                       var noiseSpectrum = Processing.allData.Where(x => x.row != r || x.column != c).ToList();
                       foreach (var temp in noiseSpectrum)
                       {
                           Processing.GetSinglePeakIntensity(temp, min, max, noise);
                       }

                       // Ratio
                       double s = signal.Where(x => x > 0).DefaultIfEmpty(0).Average();
                       double n = noise.Where(x => x > 0).DefaultIfEmpty(0).Average();
                       double rat = s / n;
                       if (s == 0)
                       {
                           rat = 0;
                       }
                       if (n == 0 && s > 0)
                       {
                           rat = 999;
                       }

                       // Find wells with same mass
                       var matchs = list.Where(x => Math.Round((double)x.MMW, 3) == Math.Round((double)item.MMW, 3)).ToList();
                       string m = "";
                       if (matchs.Count > 1)
                       {
                           m = string.Join(" ", matchs.Select(x => x.WELL_ADDRESS));
                       }

                       // Add data
                       lock (lockObj)
                       {
                           csv.AppendLine($"{item.WELL_ADDRESS}," +
                                          $"{item.ANUMBER}," +
                                          $"{item.MMW}," +
                                          $"{signal.Where(x => x > 0).DefaultIfEmpty(0).Average():F1}," +
                                          $"{StandardDeviation(signal):F1}," +
                                          $"{signal.Count(x => x > 0)}," +
                                          $"{noise.Where(x => x > 0).DefaultIfEmpty(0).Average():F1}," +
                                          $"{StandardDeviation(noise.Where(x => x > 0).DefaultIfEmpty(0)):F1}," +
                                          $"{noise.Count(x => x > 0)}," +
                                          $"{rat:F3}," +
                                          $"{m}");
                       }
                       /*
                       if (progressForm.InvokeRequired) BeginInvoke(new Action(() => { progressForm.progressBar1.PerformStep(); }));
                       else progressForm.progressBar1.PerformStep();*/
                   });

            // Sort because of parallel
            //csv.AppendLine("\r\n");
            var items = new List<string>(csv.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            items.Sort();
            csv = new StringBuilder(string.Join("\r\n", items.ToArray()));

            // Start naming
            csv.Insert(0, $"Well Address,A number,MMW,signal>0 average,signal std,signal>0 N,noise>0 average,noise>0 std,noise>0 N,S/B, similar mmw \r\n");

            //    write data
            var parts = new FileInfo(RootRawDataFile);
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

            string filename = Path.GetFileName(Path.GetFileNameWithoutExtension(RootRawDataFile) + ".csv"); ;
            File.WriteAllText(Path.Combine(path, filename), csv.ToString());
        }

        private void exportSpectraToTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int N = 1;
            foreach (var timepoint in Processing.allData)
            {
                // Start building string
                var csv = new StringBuilder();

                // m/z
                csv.AppendLine(string.Join(",", timepoint.mz));

                // Intensity
                csv.AppendLine(string.Join(",", timepoint.I));

                // Write
                File.WriteAllText($"Spectra{N:D5}.csv", csv.ToString());

                N++;
            }
        }

        private void StandardAnalysis(ProgressForm prog)
        {
            // Get TIC and display on chart
            TICchron = Processing.getChronogram(100, 1e8); //So wide a range it gets everything
            Processing.addAnalysis(TICchron, Processing.analysisType.TIC);
            prog.progressBar1.PerformStep();

            // Get EIC for Product and display on chart
            TryParse(txtRange.Text, out double massRange);
            TryParse(ProdCombo.SelectedItem.ToString(), out double productMass);
            toolTip1.SetToolTip(chkProduct,
                $"{productMass * (1 - massRange / 1e6)}-{productMass * (1 + massRange / 1e6)}");
            toolTip1.SetToolTip(ProdCombo,
                $"{productMass * (1 - massRange / 1e6)}-{productMass * (1 + massRange / 1e6)}");
            prodEICchron = Processing.getChronogram(productMass, massRange);
            Processing.addAnalysis(prodEICchron, Processing.analysisType.PROD);
            prog.progressBar1.PerformStep();

            // Get EIC for Starting Material and display on chart
            TryParse(txtStartingMaterial.Text, out double startingMaterialMass);
            toolTip1.SetToolTip(chkStartingMaterial,
                $"{startingMaterialMass * (1 - massRange / 1e6)}-{startingMaterialMass * (1 + massRange / 1e6)}");
            toolTip1.SetToolTip(txtStartingMaterial,
                $"{startingMaterialMass * (1 - massRange / 1e6)}-{startingMaterialMass * (1 + massRange / 1e6)}");
            smEICchron = Processing.getChronogram(startingMaterialMass, massRange);
            Processing.addAnalysis(smEICchron, Processing.analysisType.SM);
            prog.progressBar1.PerformStep();

            // Get EIC for Internal Standard and display on chart
            TryParse(txtInternalStandard.Text, out double internalStandardMass);
            toolTip1.SetToolTip(chkInternalStandard,
                $"{internalStandardMass * (1 - massRange / 1e6)}-{internalStandardMass * (1 + massRange / 1e6)}");
            toolTip1.SetToolTip(txtInternalStandard,
                $"{internalStandardMass * (1 - massRange / 1e6)}-{internalStandardMass * (1 + massRange / 1e6)}");
            isEICcrhon = Processing.getChronogram(internalStandardMass, massRange);
            Processing.addAnalysis(isEICcrhon, Processing.analysisType.IS);
            prog.progressBar1.PerformStep();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="method"></param>
        /// <param name="_hydrogenAddition"></param>
        /// <param name="adductAddition"></param>
        /// <param name="customAdduct"></param>
        /// <param name="window"></param>
        /// <param name="windowIsPPM">True is window is in PPM, and false if window in Da</param>
        private void PlateMapAnalysis(MALDESImethod method, double _hydrogenAddition, double adductAddition, double customAdduct, double window, bool windowIsPPM, bool ASMSsignaltoNoise)
        {
            // Try to get barcode from name
            if (method.Barcode.IsNullOrEmpty())
            {
                try
                {
                    method.Barcode = method.Filename.Substring(0, method.Filename.IndexOf("_", StringComparison.Ordinal));
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            // Pull the plate map
            if (PullPlateMap(method, out var list)) return;

            // Loop through all spectra
            var Hydrogen = new List<double>();
            var Adduct = new List<double>();
            var CustomAdduct = new List<double>();
            var NewOrder = new List<timepoint>(); // If there are multiple compounds per well, this reorders processing.allData
            for (var i = 0; i < Processing.allData.Count; i++) // Loop through all collected spectra
            {
                var CurrentSpectrum = Processing.allData[i];

                // Get one or all compounds depending on ASMSsignaltoNoise
                var allCompoundsInWell = ASMSsignaltoNoise
                    ? list
                    : list.Where(x => x.WELL_ADDRESS == $"{rowWellAddress[CurrentSpectrum.row]}{CurrentSpectrum.column + 1:F0}").ToList(); // Only compound in well

                // Make deep copy...
                for (var index = 0; index < allCompoundsInWell.Count; index++) allCompoundsInWell[index] = allCompoundsInWell[index].DeepCopy();

                // If allCompounds is 0 length, it means we have data, but the well was empty. Just create a blank...
                if (allCompoundsInWell.Count == 0)
                {
                    allCompoundsInWell.Add(new HTTPQuery.PlateMap
                    {
                        MMW = 0, //something random with no intensity
                        ANUMBER = "Empty",
                    });
                }

                // Loop through all compounds in well
                for (var index = 0; index < allCompoundsInWell.Count; index++)
                {
                    var temp = CurrentSpectrum.DeepCopy();// Deep copy so it isn't changed
                    var item = allCompoundsInWell[index];

                    // Find row and column for this item
                    if (item.WELL_ADDRESS == null)
                    {
                        item.WELL_ADDRESS = $"{rowWellAddress[CurrentSpectrum.row]}{CurrentSpectrum.column + 1:F0}";
                    }

                    // Gets r and c from list, but this is't correct in ASMS analysi
                    int r = rowWellAddress.IndexOf(item.WELL_ADDRESS.Substring(0, 1));
                    int c = int.Parse(item.WELL_ADDRESS.Substring(1, allCompoundsInWell.First().WELL_ADDRESS.Length - 1)) - 1;

                    // Update 10/7/2022
                    r = CurrentSpectrum.row;
                    c = CurrentSpectrum.column;

                    temp.row = r;
                    temp.column = c;

                    if (item.MMW == 0 && !windowIsPPM)// Protein mode with no compound or missing compound MW
                    {
                        item.MMW = -100000;
                        item.MW = -100000;
                    }

                    // If missing MMW
                    if (item.MMW == null)
                    {
                        item.MMW = 0;
                        item.MW = 0;
                    }

                    // Change Filename to have A-number
                    temp.Filename = item.ANUMBER;

                    // Get specified Hydrogen intensity (or bound protein for proteinaceous analysis)
                    var mz = (double)item.MMW;
                    mz += _hydrogenAddition;
                    double min = mz, max = mz;
                    if (windowIsPPM)
                    {
                        min = mz - window * mz / 1e6;
                        max = mz + window * mz / 1e6;
                    }
                    else
                    {
                        min = mz - window;
                        max = mz + window;
                    }
                    Processing.GetSinglePeakIntensity(temp, min, max, Hydrogen);

                    // Get specified Adduct intensity (or unbound protein for proteinaceous)
                    mz = (double)item.MMW;
                    mz += adductAddition;
                    if (windowIsPPM)
                    {
                        min = mz - window * mz / 1e6;
                        max = mz + window * mz / 1e6;
                    }
                    else
                    {
                        mz = _hydrogenAddition;// Protein reference mass
                        min = mz - window;
                        max = mz + window;
                    }
                    Processing.GetSinglePeakIntensity(temp, min, max, Adduct);

                    // specified custom adduct intensity
                    mz = (double)item.MMW;
                    mz += customAdduct;
                    if (windowIsPPM)
                    {
                        min = mz - window * mz / 1e6;
                        max = mz + window * mz / 1e6;
                    }
                    else
                    {
                        min = 0;
                        max = 1e8;// calculate TEC
                    }
                    Processing.GetSinglePeakIntensity(temp, min, max, CustomAdduct);
                    if (CustomAdduct.Last() == PositiveInfinity)
                    {
                    }

                    // See if hit
                    bool success = TryParse(threshold.Text, out double thresh);
                    if (!success) thresh = 0;
                    if (!windowIsPPM)// Protein binding
                    {
                        if (Hydrogen.LastOrDefault() > thresh) temp.AlignmentLaserState = true; // Using this to transfer hit info

                        // TEC normalize the unbound
                        CustomAdduct[CustomAdduct.Count - 1] = Adduct.Last() / CustomAdduct.Last() * 100;
                    }
                    else
                    {
                        if (Hydrogen.LastOrDefault() > thresh ||
                            Adduct.LastOrDefault() > thresh ||
                            CustomAdduct.LastOrDefault() > thresh)
                            temp.AlignmentLaserState = true; // Using this to transfer hit info
                    }

                    // Deal with multiple compounds per well
                    NewOrder.Add(temp);
                }
            }

            // Transfer full thing to all data
            Processing.allData = NewOrder;

            // Adds to AllData
            Processing.addAnalysis(Hydrogen, Processing.analysisType.PROD);
            Processing.addAnalysis(Adduct, Processing.analysisType.SM);
            Processing.addAnalysis(CustomAdduct, Processing.analysisType.IS);

            // Additional step if doing ASMS signal to noise analysis
            if (ASMSsignaltoNoise)
            {
                CompileSignalToNoiseResults(NewOrder, list);
            }
        }

        private void CompileSignalToNoiseResults(List<timepoint> newOrder, List<HTTPQuery.PlateMap> list)
        {
            List<timepoint> finalOrder = new List<timepoint>();

            // Loop through new order but skip number of compounds to get to next one
            for (var i = 0; i < newOrder.Count; i += list.Count) // Loop through all collected spectra
            {
                // Find actual compound in well
                var allCompoundsInWell = list.Where(x => x.WELL_ADDRESS == $"{rowWellAddress[newOrder[i].row]}{newOrder[i].column + 1:F0}").ToList();
                if (allCompoundsInWell.Count == 0)
                {
                    allCompoundsInWell.Add(new HTTPQuery.PlateMap
                    {
                        MMW = 0, //something random with no intensity
                        ANUMBER = "Empty",
                    });
                }

                // Get current spectra with analysis across compounds
                var cur = newOrder.GetRange(i, list.Count);

                // Find which one has the actual compound of interest
                var signal = cur.Where(x => x.Filename == allCompoundsInWell[0].ANUMBER).ToList();
                if (signal.Count == 0)
                {
                    // Just take first cur and make it what you need
                    cur[0].Filename = "Empty";
                    cur[0].SM = 0;
                    cur[0].IS = 0;
                    cur[0].PROD = 0;

                    // Copy data to output
                    finalOrder.Add(cur[0]);
                    continue;
                }
                int index = newOrder.IndexOf(signal.First());

                // Find noise
                var noise = cur.Where(x => x.Filename != allCompoundsInWell[0].ANUMBER).ToList();

                // Convert analysis to signal to noise....
                int ii = i / list.Count;
                double test = signal.First().IS / StandardDeviation(noise.Select(x => x.IS));
                if (test < 3 && test > 0)
                {
                    double sstd = StandardDeviation(noise.Select(x => x.IS));
                }

                newOrder[index].PROD = signal.First().PROD / StandardDeviation(noise.Select(x => x.PROD));
                newOrder[index].SM = signal.First().SM / StandardDeviation(noise.Select(x => x.SM));
                newOrder[index].IS = signal.First().IS / StandardDeviation(noise.Select(x => x.IS));

                // Copy data to output
                finalOrder.Add(newOrder[index]);
            }

            // transfer finalorder to alldata
            Processing.allData = finalOrder;
        }

        public static double StandardDeviation(IEnumerable<double> values)
        {
            double avg = values.Average();
            double std = Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
            if (std == 0)
            {
                std = 0.0001;// make it an arbitrary small number
            }
            return std;
        }

        private bool PullPlateMap(MALDESImethod method, out List<HTTPQuery.PlateMap> list)
        {
            // Try loading RAS plate from local database file
            list = db.GetPlateMap();
            list = null;

            // If not already in database, try load from web service
            if (list == null || list.Count == 0)
            {
                // Try webservice
                Log.Debug($"Query plate map for {method.Barcode}");
                var webService = new HTTPQuery();
                list = webService.Query(method.Barcode);

                // Couldn't find database entry from barcode
                if (list.Count == 0)
                {
                    var result =
                        MessageBox.Show(
                            @"Could not load plate map from web service! Please verify the barcode (Plate Map => Edit Barcode). Would you like to manually load a plate map?",
                            @"Web Service Load Error", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        list = ManualPlateMapLoad(db.CurrentFile);
                    }
                    else return true;

                    if (list.Count == 0) return true;
                }

                // Store ras list
                db.SetPlateMap(list);
            }

            return false;
        }

        public class PlateMapTemp
        {
            public string WELL_ADDRESS { get; set; }

            public double? MMW { get; set; }
            public string ANUMBER { get; set; }
        }

        private static List<HTTPQuery.PlateMap> ManualPlateMapLoad(string p = "c:\\")
        {
            // Lists
            var records = new List<PlateMapTemp>();
            var list = new List<HTTPQuery.PlateMap>();

            // Select file
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = p;
                openFileDialog.Filter = @"All files (*.*)|*.*|csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    // Configure without header
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = false,
                    };

                    try
                    {
                        using (var reader = new StreamReader(filePath))
                        using (var csv = new CsvReader(reader, config))
                        {
                            records = csv.GetRecords<PlateMapTemp>().ToList();
                        }
                    }
                    catch
                    {
                        MessageBox.Show(@"Please check plate map is in correct format! First column: Well address. Second column: MMW. Third column: Anumber");
                        return list;
                    }
                }
                else return list;
            }

            // convert records to list
            foreach (var r in records)
            {
                if (r.MMW == null)
                {
                    r.MMW = 0;
                }

                if (r.WELL_ADDRESS == "")
                {
                    r.WELL_ADDRESS = "A1";
                }

                if (r.MMW == 169.085127)
                {
                    r.MMW = 2;
                }
                list.Add(new HTTPQuery.PlateMap { WELL_ADDRESS = r.WELL_ADDRESS, ANUMBER = r.ANUMBER, MMW = r.MMW });
            }

            return list;
        }

        #endregion Processing

        #region Display

        private void DisplaySpectrum(int row, int column, int spot = 0, bool clear = true)
        {
            // Check if out of bounds
            if (row > Processing.allData.Select(x => x.row).Max()) return;
            if (column > Processing.allData.Select(x => x.column).Max()) return;

            // Get average spectrum (HARD BECAUSE X POINTS COULD BE DIFFERENT)
            /*
			List<timepoint> multiple = processing.allData.Where(x => (x.row == row) && (x.column == column)).ToList();
			timepoint single = new timepoint();
			foreach (var m in multiple)
			{
			}*/

            // Get first spectrum
            timepoint single = null;
            try
            {
                single = Processing.allData.First(x => x.row == row && x.column == column && x.spot == 0);
            }
            catch
            {
                single = Processing.allData.First(x => x.row == row && x.column == column);
            }

            // Plot axes
            var model = new PlotModel { Title = "" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "m/z", Minimum = single.mz.Min(), Maximum = single.mz.Max() });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Intensity" });
            model.PlotAreaBorderColor = OxyColors.Transparent;

            // Line type
            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 1.5,
                Color = OxyColor.FromRgb(0, 0, 0) // black
            };

            // Plot data
            for (var i = 0; i < single.mz.Length; i++) lineSeries.Points.Add(new DataPoint(single.mz[i], single.I[i]));
            model.Series.Add(lineSeries);

            // PPM
            if (!TryParse(txtRange.Text, out double ppm))
            {
                // Set default value as 5 ppm if nothing there
                txtRange.Text = @"5";
                ppm = 5;
            }

            // Product
            if (!TryParse(ProdCombo.SelectedItem.ToString(), out double Prod))
            {
                ProdCombo.Text = @"0";
            }

            // Internal standard
            if (!TryParse(txtInternalStandard.Text, out double IS))
            {
                txtInternalStandard.Text = @"0";
            }

            // Starting material
            if (!TryParse(txtStartingMaterial.Text, out double SM))
            {
                txtStartingMaterial.Text = @"0";
            }
            double maxVal = single.I.Max();

            // Product
            if (Prod > 0)
            {
                var series = new AreaSeries
                {
                    Color = OxyColor.FromRgb(0, 0xFF, 0) // green
                };
                series.Points.Add(new DataPoint(Prod * (1 - ppm / 1e6), maxVal));
                series.Points.Add(new DataPoint(Prod * (1 + ppm / 1e6), maxVal));
                model.Series.Add(series);
            }

            // SM
            if (SM > 0)
            {
                var series = new AreaSeries
                {
                    Color = OxyColor.FromRgb(0xFF, 0, 0) // red
                };
                series.Points.Add(new DataPoint(SM * (1 - ppm / 1e6), maxVal));
                series.Points.Add(new DataPoint(SM * (1 + ppm / 1e6), maxVal));
                model.Series.Add(series);
            }

            // IS
            if (IS > 0)
            {
                var series = new AreaSeries
                {
                    Color = OxyColor.FromRgb(0, 0, 0xFF) // blue
                };
                series.Points.Add(new DataPoint(Prod * (1 - ppm / 1e6), maxVal));
                series.Points.Add(new DataPoint(Prod * (1 + ppm / 1e6), maxVal));
                model.Series.Add(series);
            }

            // Update model
            MSplot.Model = model;
        }

        private void DisplayChromatogram(int row, int column, int spot = 0)
        {
            if (Processing == null)
            {
                MessageBox.Show(@"Please load data first!");
                return;
            }

            // Max value for shading
            double maxVal = 0;
            int index = Processing.allData.FindIndex(x => x.row == row && x.column == column);// Finds first index

            // Plot axes
            var model = new PlotModel { Title = "" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time (Seconds)" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Intensity" });
            model.LegendTitle = "Legend";
            model.LegendPosition = LegendPosition.RightTop;
            model.PlotAreaBorderColor = OxyColors.Transparent;

            // TIC
            if (chkChromTIC.Checked && TICchron != null)
            {
                // Line type
                var lineSeries = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 1.5,
                    Color = OxyColor.FromRgb(0, 0, 0), // Black
                    Title = "TIC"
                };

                // Add data
                var i = 0;
                foreach (double p in TICchron) { lineSeries.Points.Add(new DataPoint(Processing.allData[i].time, p)); i++; };
                model.Series.Add(lineSeries);

                // Update maxVal
                maxVal = Math.Max(maxVal, TICchron.Max());
            }

            // Product
            if (chkProduct.Checked && prodEICchron != null)
            {
                // Line type
                var lineSeries = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 1.5,
                    Color = OxyColor.FromRgb(0, 0xFF, 0), // Green
                    Title = "Prod EIC"
                };

                // Add data
                var i = 0;
                foreach (double p in prodEICchron) { lineSeries.Points.Add(new DataPoint(Processing.allData[i].time, p)); i++; };
                model.Series.Add(lineSeries);

                // Update maxVal
                maxVal = Math.Max(maxVal, prodEICchron.Max());
            }

            // SM
            if (chkStartingMaterial.Checked && smEICchron != null)
            {
                // Line type
                var lineSeries = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 1.5,
                    Color = OxyColor.FromRgb(0xFF, 0, 0), // red
                    Title = "SM EIC"
                };

                // Add data
                var i = 0;
                foreach (double p in smEICchron) { lineSeries.Points.Add(new DataPoint(Processing.allData[i].time, p)); i++; };
                model.Series.Add(lineSeries);

                // Update maxVal
                maxVal = Math.Max(maxVal, smEICchron.Max());
            }

            // Internal Standard
            if (chkInternalStandard.Checked && isEICcrhon != null)
            {
                // Line type
                var lineSeries = new LineSeries
                {
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 1.5,
                    Color = OxyColor.FromRgb(0, 0, 0xFF), // blue
                    Title = "IS EIC"
                };

                // Add data
                var i = 0;
                foreach (double p in isEICcrhon) { lineSeries.Points.Add(new DataPoint(Processing.allData[i].time, p)); i++; };
                model.Series.Add(lineSeries);

                // Update maxVal
                maxVal = Math.Max(maxVal, isEICcrhon.Max());
            }

            // Highlight current spot
            var series = new AreaSeries();
            int Nspots = Processing.allData.Select(x => x.spot).Max();
            series.Color = OxyColor.FromRgb(0xFF, 0, 0);// red
            double window = Processing.allData.Select(x => x.time).Max() / 250;
            window = 1;
            series.Points.Add(new DataPoint(Processing.allData[index].time - window, maxVal));
            series.Points.Add(new DataPoint(Processing.allData[index].time + window, maxVal));

            //if (index + 2 * Nspots < processing.allData.Count) series.Points.Add(new OxyPlot.DataPoint(processing.allData[index+2*Nspots].time, maxVal));
            model.Series.Add(series);

            // Update chart
            chronPlot.Model = model;

            // Update currents
            CurrentRow = row;
            CurrentColumn = column;
        }

        private void chkBoxChromatograms_CheckedChanged(object sender, EventArgs e)
        {
            DisplayChromatogram(CurrentRow, CurrentColumn, currentSpot);
        }

        private void updateDisplay(object sender, MouseEventArgs e)
        {
            updateDisplay();
        }

        private void updateDisplay()
        {
            if (checkBoxProduct.Checked) DisplayHeatmap(displayType.PROD);
            if (checkBoxSM.Checked) DisplayHeatmap(displayType.SM);
            if (checkBoxIS.Checked) DisplayHeatmap(displayType.IS);
            if (checkBoxConv.Checked) DisplayHeatmap(displayType.CONVERSION);
            if (checkBoxRatio.Checked) DisplayHeatmap(displayType.PRODtoIS);
            if (checkBoxRatioTIC.Checked) DisplayHeatmap(displayType.PRODtoTIC);
        }

        private Color GetHeatMapColor(double perCent)
        {
            if (perCent == -666)// interupt signal
            {
                return Color.FromArgb(255, 255, 0, 0);
            }

            // Colormap from matlab
            var map = new int[64, 3]{
                {62,38,168},
                {64,42,180},
                {66,46,192},
                {68,50,203},
                {69,55,213},
                {70,60,222},
                {71,65,229},
                {71,71,235},
                {72,77,240},
                {72,82,244},
                {71,88,248},
                {70,94,251},
                {69,99,253},
                {66,105,254},
                {62,111,255},
                {56,117,254},
                {50,124,252},
                {47,129,250},
                {46,135,247},
                {45,140,243},
                {43,145,239},
                {39,151,235},
                {37,155,232},
                {35,160,229},
                {32,165,227},
                {28,169,223},
                {24,173,219},
                {18,177,214},
                { 8,181,208},
                { 1,184,202},
                { 2,186,195},
                {11,189,189},
                {25,191,182},
                {36,193,174},
                {44,196,167},
                {49,198,159},
                {55,200,151},
                {63,202,142},
                {74,203,132},
                {87,204,122},
                   {100,205,111},
                   {114,205,100},
                   {129,204,89},
                   {143,203,78},
                   {157,201,67},
                   {171,199,57},
                   {185,196,49},
                   {197,194,42},
                   {209,191,39},
                   {220,189,41},
                   {230,187,45},
                   {240,186,54},
                   {248,186,61},
                   {254,190,60},
                   {254,195,56},
                   {254,201,52},
                   {252,207,48},
                   {250,214,45},
                   {247,220,42},
                   {245,227,39},
                   {245,233,36},
                   {246,239,32},
                   {247,245,27},
                   {249,251,21}};

            // Rescale perCent by trackbar upper and lower bounds
            perCent -= trackBarLB.Value;
            perCent /= (double)(trackBarUB.Value - trackBarLB.Value) / 100;

            var index = (int)(perCent / 100 * 63);
            if (index > 63) index = 63;
            if (index < 0) index = 0;

            return Color.FromArgb(255, map[index, 0], map[index, 1], map[index, 2]);
        }

        public enum displayType
        {
            PROD,

            SM,

            IS,

            CONVERSION,

            PRODtoIS,

            PRODtoTIC
        }

        private void DisplayHeatmap(displayType type)
        {
            if (Processing == null)
            {
                MessageBox.Show(@"Please load data first!");
                return;
            }

            double[] values = null;
            double maxVal;
            if (AnalysisMode.SelectedIndex == 2)// protein mode
            {
                switch (type)
                {
                    case displayType.PROD:
                        maxVal = Processing.allData.Select(x => x.PROD).Max();
                        values = Processing.allData.GroupBy(d => d.row + d.column * numCols)
                            .ToDictionary(g => g.Key, g => g.Sum(d => 100 * d.PROD / maxVal)).Values.ToArray();
                        break;

                    case displayType.IS:
                        maxVal = Processing.allData.Select(x => x.IS).Max();
                        values = Processing.allData.GroupBy(d => d.row + d.column * numCols)
                            .ToDictionary(g => g.Key, g => g.Sum(d => 100 * d.IS / maxVal)).Values.ToArray();
                        break;
                }

                var WellIndex = Processing.allData.Select(d => d.row * numCols + d.column).Distinct().ToList();

                //   var sub = processing.allData.Distinct(y => y.)  //.Select(x => x).ToList();// Just get first spot values

                var index = 0;
                foreach (double v in values)
                {
                    var val = (int)v;

                    // Stay within bounds
                    if (val > 100) val = 100;
                    if (val < 0) val = 0;

                    var wp = new wellPlate();// Functions to deal with wellPlate drawing
                    int r = WellIndex[index] / 24;
                    int c = WellIndex[index] % 24;
                    /* Don't need to flip with the above WellIndex
                    if (r % 2 != 0)
                    {
                        c = 23 - c;
                    }*/

                    wp.SetWellColor(r, c, v > maxNumSamples ? Color.FromArgb(255, 255, 255) : GetHeatMapColor(val), dataGridView1);
                    index += 1;
                }

                return;
            }
            else
            {
                // Calculate parameters... Gotta process by spot, this only take first 384

                switch (type)
                {
                    case displayType.PROD:
                        maxVal = Processing.allData.Select(x => x.PROD).Max();
                        if (checkBoxAverageSpots.Checked)
                            values = Processing.allData.GroupBy(d => d.row + d.column * numCols).ToDictionary(g => g.Key, g => g.Average(d => 100 * d.PROD / maxVal)).Values.ToArray();
                        else
                            values = Processing.allData.Where(x => x.spot == currentSpot).Select(x => 100 * x.PROD / maxVal).ToArray();
                        break;

                    case displayType.SM:
                        maxVal = Processing.allData.Select(x => x.SM).Max();
                        if (checkBoxAverageSpots.Checked)
                            values = Processing.allData.GroupBy(d => d.row + d.column * numCols).ToDictionary(g => g.Key, g => g.Average(d => 100 * d.SM / maxVal)).Values.ToArray();
                        else
                            values = Processing.allData.Where(x => x.spot == currentSpot).Select(x => 100 * x.SM / maxVal).ToArray();
                        break;

                    case displayType.IS:
                        maxVal = Processing.allData.Select(x => x.IS).Max();
                        if (checkBoxAverageSpots.Checked)
                            values = Processing.allData.GroupBy(d => d.row + d.column * numCols)
                                .ToDictionary(g => g.Key, g => g.Average(d => 100 * d.IS / maxVal)).Values.ToArray();
                        else
                            values = Processing.allData.Where(x => x.spot == currentSpot)
                                .Select(x => 100 * x.IS / maxVal).ToArray();
                        break;

                    case displayType.CONVERSION:
                        if (checkBoxAverageSpots.Checked)
                            values = Processing.allData.GroupBy(d => d.row + d.column * numCols)
                                .ToDictionary(g => g.Key, g => g.Average(d => 100 * d.PROD / (d.SM + d.PROD))).Values
                                .ToArray();
                        else
                            values = Processing.allData.Where(x => x.spot == currentSpot)
                                .Select(x => 100 * x.PROD / (x.SM + x.PROD)).ToArray();
                        break;

                    case displayType.PRODtoIS:
                        if (checkBoxAverageSpots.Checked)
                            values = Processing.allData.GroupBy(d => d.row + d.column * numCols)
                                .ToDictionary(g => g.Key, g => g.Average(d => 100 * d.PROD / d.IS)).Values.ToArray();
                        else
                            values = Processing.allData.Where(x => x.spot == currentSpot)
                                .Select(x => 100 * x.PROD / x.IS).ToArray();
                        break;

                    case displayType.PRODtoTIC:
                        if (checkBoxAverageSpots.Checked)
                            values = Processing.allData.GroupBy(d => d.row + d.column * 24)
                                .ToDictionary(g => g.Key, g => g.Average(d => 100 * d.PROD / d.TIC)).Values.ToArray();
                        else
                            values = Processing.allData.Where(x => x.spot == currentSpot)
                                .Select(x => 100 * x.PROD / x.TIC).ToArray();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }

            // If logscale
            if (checkBoxLog.Checked)
            {
                try
                {
                    maxVal = values.Select(x => Math.Log10(x)).Max();
                    double minVal = values.Where(x => x > 0).Select(x => Math.Log10(x)).Min();//values.Select(x => Math.Log(x)).Min();
                    values = values.Select(x => 100 * (Math.Log10(x) - minVal) / (maxVal - minVal)).ToArray();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.YesNo); };
            }

            // Clear selection in rerun
            foreach (var r in rerun) r.clicked = false;

            // Calculate success
            var N = 0;
            var subset = Processing.allData.Where(x => x.spot == 0).Select(x => x).ToList();// Just get first spot values
            foreach (double v in values)
            {
                var val = (int)v;

                /*// Threshold by track bar (NOT NEEDED)
				if (val > trackBarUB.Value) val = trackBarUB.Value;
				if (val < trackBarLB.Value) val = trackBarLB.Value;*/

                // Stay within bounds
                if (val > 100) val = 100;
                if (val < 0) val = 0;

                // Percent conversion
                var pc = Processing.allData.Where(x => x.row == subset[N].row && x.column == subset[N].column).Select(y => y.PROD / (y.SM + y.PROD)).ToList();

                // Threshold by CV
                double cv = DataSaving.CalculateCV(pc);// stdev of only 1 value is 0
                if (CheckCVCutoff.Checked && cv >= (double)trackBarCV.Value / 100)
                {
                    rerun.First(x => x.row == subset[N].row && x.column == subset[N].column).clicked = true;// Flip the rerun flag to true
                    val = -666;//triggers color interupt
                }

                var wp = new wellPlate();// Functions to deal with wellPlate drawing
                wp.SetWellColor(subset[N].row, subset[N].column, v > maxNumSamples ? Color.FromArgb(255, 255, 255) : GetHeatMapColor(val), dataGridView1);
                N += 1;
            }
        }

        // A1 is (row,column) (0,0)
        public void WellClicked(int row, int column)
        {
            // Check if data is loaded
            if (Processing == null)
            {
                MessageBox.Show(@"Please load data first!");
                return;
            }

            // get method from db
            var method = db.GetMaldesiMethod();//db.Table.Metadata.Retrieve<MALDESImethod>("Method");

            if (method == null)
            {
                // Check if out of bounds
                if (row > Processing.allData.Select(x => x.row).Max()) return;
                if (row < Processing.allData.Select(x => x.row).Min()) return;
                if (column > Processing.allData.Select(x => x.column).Max()) return;
                if (column < Processing.allData.Select(x => x.column).Min()) return;
            }
            else
            {
                if (Processing.allData.Count(x => x.column == column && x.row == row) == 0) return;
            }

            // Find well of interest
            string wellCoordinates = (char)(65 + row) + (column + 1).ToString();
            dataGridView1.Rows[row].Cells[column].Selected = false;

            // Display Spectrum
            DisplaySpectrum(row, column, currentSpot);

            // Display label
            lblSpectrum.Text = $@"Average mass spectrum for well {wellCoordinates}  ";

            // Display Chromatogram
            DisplayChromatogram(row, column, currentSpot);

            // Get data for this well
            var single = new timepoint();
            if (checkBoxAverageSpots.Checked)
            {
                single.SM = Processing.allData.Where(x => x.row == row && x.column == column).Select(x => x.SM).Average();
                single.PROD = Processing.allData.Where(x => x.row == row && x.column == column).Select(x => x.PROD).Average();
                single.IS = Processing.allData.Where(x => x.row == row && x.column == column).Select(x => x.IS).Average();
                single.TIC = Processing.allData.Where(x => x.row == row && x.column == column).Select(x => x.TIC).Average();
            }
            else
            {
                try
                {
                    single = Processing.allData.First(x => x.row == row && x.column == column && x.spot == currentSpot);
                }
                catch
                {
                    single = Processing.allData.First(x => x.row == row && x.column == column);
                }
            }

            if (single != null)
            {
                // Display Status Labels
                toolStripStatusLabel1.Text = $@"{wellCoordinates}: ";// + spectrum.Description;
                toolStripStatusLabel2.Text = $@"Prod Intensity{single.PROD:0.000E+00}";
                toolStripStatusLabel3.Text = $@"SM Intensity{single.SM:0.000E+00}";
                toolStripStatusLabel4.Text = $@"IS Intensity{single.IS:0.000E+00}";

                // Display calculated values
                toolStripStatusLabel5.Text =
                    $@"Ratio Prod/IS = {(single.PROD / single.IS):0.000},  % Conv SM-Prod = {(100 * single.PROD / (single.PROD + single.SM)):0}%";
            }

            if (_debugForm != null && !_debugForm.IsDisposed)
            {
                ShowDebugData(row, column);
            }
        }

        private void ShowDebugData(int row, int column)
        {
            // Info pull
            var info = Processing.allData.FirstOrDefault(x => x.row == row && x.column == column && x.spot == 0);
            if (info == null) return;

            // Get the method
            var method = db.GetMaldesiMethod();

            // Laser Info to list box
            _debugForm.metadata.Items.Clear();
            _debugForm.metadata.Items.Add($"Alignment Laser State : {info.AlignmentLaserState}");
            _debugForm.metadata.Items.Add($"Pulses Per Burst : {info.PulsesPerBurst}");
            _debugForm.metadata.Items.Add($"Bursts Per Spot : {info.BurstsPerSpot}");
            _debugForm.metadata.Items.Add($"Spots Per Trigger : {info.SpotsPerTrigger}");

            //_debugForm.metadata.Items.Add("Delay After Trigger : " + info.DelayAfterTrigger);
            _debugForm.metadata.Items.Add($"Delay After Ctrap Open : {info.DelayAfterCtrapOpen}");
            _debugForm.metadata.Items.Add($"Delay Between Bursts : {info.DelayBetweenBursts}");
            _debugForm.metadata.Items.Add($"Delay Between Spots : {info.DelayBetweenSpots}");
            _debugForm.metadata.Items.Add($"Counter N: {info.CounterN}");
            _debugForm.metadata.Items.Add($"Delay Counter N (ms): {info.DelayCounterN}");
            _debugForm.metadata.Items.Add($"Low Latency Handshake: {(method.ScanMode == ScanMode.LowLatHS)}");
            _debugForm.metadata.Items.Add($"Scan Mode: {method.ScanMode}");
            _debugForm.metadata.Items.Add($"Servo Speed (mm/s) {method.servoSpeed * 2.02}");

            // Add in Xmethod info
            _debugForm.metadata2.Items.Clear();
            if (method.XMethod.File != null)
            {
                _debugForm.metadata2.Items.Add($"IT : {method.XMethod.MaximumIT}");
                _debugForm.metadata2.Items.Add($"Microscans : {method.XMethod.Microscans}");
                _debugForm.metadata2.Items.Add($"Polarity : {method.XMethod.Polarity}");
                _debugForm.metadata2.Items.Add($"Resolution : {method.XMethod.Resolution}");
                _debugForm.metadata2.Items.Add($"ScanRange : {method.XMethod.ScanRange}");
                _debugForm.metadata2.Items.Add($"Tunefile : {method.XMethod.Tunefile}");
            }

            // Other Info to list box
            _debugForm.metadata2.Items.Add($"Flow Rate (ul/min) : {info.ESISolventFlowRate}");
            _debugForm.metadata2.Items.Add($"ESISolventName : {info.ESISolventName}");
            _debugForm.metadata2.Items.Add($"Chill Plate Temperature : {info.TECactualTemp}C");
            _debugForm.metadata2.Items.Add($"Laser Focus Z : {info.laserZ}");
            _debugForm.metadata2.Items.Add($"X : {info.x}");
            _debugForm.metadata2.Items.Add($"Y : {info.y}");
            try { _debugForm.metadata2.Items.Add($"Z : {info.z}"); } catch { };//Added this later
            _debugForm.metadata2.Items.Add($"Time : {info.time}s");
            _debugForm.metadata2.Items.Add($"Method : {method.XMethod.FullFile}");

            // Database pull
            var o = db.PullObject(row, column);
            if (o == null) return;

            // Show images
            _debugForm.camera0.Image = o.GetColumn<Image<Gray, byte>>("Webcam0");
            _debugForm.camera1.Image = o.GetColumn<Image<Gray, byte>>("Webcam1");

            // Get waveforms
            int[] ro = o.GetColumn<int[]>("readyOut");
            int[] si = o.GetColumn<int[]>("startIn");
            int[] so = o.GetColumn<int[]>("startOut");
            int[] ct = null;
            uint[] t = null;
            try
            {
                ct = o.GetColumn<int[]>("cTrapOut");
                t = o.GetColumn<uint[]>("time");
            }
            catch { }
            byte[] l = o.GetColumn<byte[]>("Laser");
            var wf = new List<waveForm>();
            var sampleRate = db.Table.Metadata.Retrieve<int>("DAQ Sample Rate");
            if (sampleRate == 0) sampleRate = 50000;//if not written...
            for (var i = 0; i < ro.Length; i++)
            {
                if (t == null) wf.Add(new waveForm { Laser = l[i], readyOut = ro[i] == 1, startIn = si[i] == 1, startOut = false, cTrapOpen = false, time = (uint)i * (1000000 / (uint)sampleRate) });
                else wf.Add(new waveForm { Laser = l[i], readyOut = ro[i] == 1, startIn = si[i] == 1, startOut = so[i] == 1, cTrapOpen = ct[i] == 1, time = t[i] });
            }

            // Generate plot axis
            var model = new PlotModel { Title = "" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time (milliseconds)" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Voltage" });
            model.LegendTitle = "Legend";
            model.LegendPosition = LegendPosition.LeftTop;
            model.PlotAreaBorderColor = OxyColors.Transparent;

            // Start In
            var l0 = new LineSeries
            {
                //l0.MarkerType = MarkerType.Circle;
                MarkerSize = 0.1,
                Color = OxyColor.FromRgb(0, 0xFF, 0), // green
                Title = "Start In"
            };

            // MS Ready
            var l1 = new LineSeries
            {
                //l1.MarkerType = MarkerType.Circle;
                MarkerSize = 0.1,
                Color = OxyColor.FromRgb(0, 0, 0), // Black
                Title = "Ready Out"
            };

            // Laser
            var l2 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 1.5,
                Color = OxyColor.FromRgb(0xFF, 0, 0), //Red
                Title = "Laser"
            };

            // C trap
            var l3 = new LineSeries
            {
                //l3.MarkerType = MarkerType.Circle;
                MarkerSize = 1.5,
                Color = OxyColor.FromRgb(0, 0, 0xFF), //blue
                Title = "C Trap"
            };

            // Start Out
            var l4 = new LineSeries
            {
                //l4.MarkerType = MarkerType.Circle;
                MarkerSize = 1.5,
                Color = OxyColor.FromRgb(0, 0xFF, 0xFF), //cyan
                Title = "Start Out"
            };

            // Add data
            var Last = 666;
            double to = 0;
            var plast = wf[0];
            var csv = new List<string>();
            foreach (var p in wf)
            {
                if (Last == 666)// This is the first time point, so grab the time as a reference
                {
                    to = p.time;
                }
                /*else
				{
					if ((p.readyOut ? 1 : 0) != Last)
					{
						l1.Points.Add(new DataPoint((p.time - to) / 1000, Last));
					}
					*/

                // Last timepoint, but carried out to just before this one. So the curves appear correct
                l0.Points.Add(new DataPoint((p.time - to) / 1000 - 0.0001, plast.startIn ? 0.1 : 1.1));// FLip logic
                l1.Points.Add(new DataPoint((p.time - to) / 1000 - 0.0001, plast.readyOut ? 1 : 0));
                l2.Points.Add(new DataPoint((p.time - to) / 1000 - 0.0001, (double)plast.Laser / 255));
                l4.Points.Add(new DataPoint((p.time - to) / 1000 - 0.0001, plast.startOut ? 0.95 : -0.05));
                l3.Points.Add(new DataPoint((p.time - to) / 1000 - 0.0001, plast.cTrapOpen ? 0.05 : 1.05));// FLip logic
                csv.Add($"{(p.time - to) / 1000 - 0.0001},{(plast.startIn ? 0.1 : 1.1)},{(plast.readyOut ? 1 : 0)},{(double)plast.Laser / 255},{(plast.startOut ? 0.95 : -0.05)},{(plast.cTrapOpen ? 0.05 : 1.05)}");

                // This timepoint
                l0.Points.Add(new DataPoint((p.time - to) / 1000, p.startIn ? 0.1 : 1.1));// FLip logic
                l1.Points.Add(new DataPoint((p.time - to) / 1000, p.readyOut ? 1 : 0));
                l2.Points.Add(new DataPoint((p.time - to) / 1000, (double)p.Laser / 255));
                l4.Points.Add(new DataPoint((p.time - to) / 1000, p.startOut ? 0.95 : -0.05));
                l3.Points.Add(new DataPoint((p.time - to) / 1000, p.cTrapOpen ? 0.05 : 1.05));// FLip logic
                csv.Add($"{(p.time - to) / 1000},{(p.startIn ? 0.1 : 1.1)},{(p.readyOut ? 1 : 0)},{(double)p.Laser / 255},{(p.startOut ? 0.95 : -0.05)},{(p.cTrapOpen ? 0.05 : 1.05)}");
                plast = p;

                //}
                Last = p.readyOut ? 1 : 0;
            }
            _debugForm.csv = csv;
            model.Series.Add(l0);
            model.Series.Add(l1);
            model.Series.Add(l2);
            model.Series.Add(l3);
            model.Series.Add(l4);

            // Pound to form
            _debugForm.waveForm.Model = model;

            // Bring to front
            _debugForm.BringToFront();
        }

        #endregion Display

        #region Data Saving

        private void exportExcelFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure to reanalyze just in case
            LoadDataFiles(RootRawDataFile);
            DisplayChromatogram(CurrentRow, CurrentColumn, currentSpot);

            switch (AnalysisMode.SelectedIndex)
            {
                case 0:

                    // CSV outpus
                    DataSaving.saveCSV(false, ProdCombo.SelectedIndex + 1);
                    DataSaving.saveCSV(true, ProdCombo.SelectedIndex + 1);// This changes allData unfortunately

                    // Data pump
                    DataSaving.saveDataPumpTSV(true, ProdCombo.SelectedIndex + 1);

                    // Dotmatics
                    DataSaving.saveDotmatics(true, ProdCombo.SelectedIndex + 1);
                    break;

                case 1:

                    DataSaving.SavePlateMapHits(true, false);

                    break;

                case 2:
                    DataSaving.SavePlateMapHits(true, true);
                    break;
            }
        }

        #endregion Data Saving

        #region Checkboxes

        private void CheckboxHeatmap(object sender, EventArgs e)
        {
            // Deregister
            checkBoxProduct.CheckedChanged -= CheckboxHeatmap;
            checkBoxSM.CheckedChanged -= CheckboxHeatmap;
            checkBoxIS.CheckedChanged -= CheckboxHeatmap;
            checkBoxConv.CheckedChanged -= CheckboxHeatmap;
            checkBoxRatio.CheckedChanged -= CheckboxHeatmap;
            checkBoxRatioTIC.CheckedChanged -= CheckboxHeatmap;

            // Uncheck all
            checkBoxProduct.Checked = false;
            checkBoxSM.Checked = false;
            checkBoxIS.Checked = false;
            checkBoxConv.Checked = false;
            checkBoxRatio.Checked = false;
            checkBoxRatioTIC.Checked = false;

            // Recheck only the current one
            ((CheckBox)sender).Checked = true;

            // Reregister
            checkBoxProduct.CheckedChanged += CheckboxHeatmap;
            checkBoxSM.CheckedChanged += CheckboxHeatmap;
            checkBoxIS.CheckedChanged += CheckboxHeatmap;
            checkBoxConv.CheckedChanged += CheckboxHeatmap;
            checkBoxRatio.CheckedChanged += CheckboxHeatmap;
            checkBoxRatioTIC.CheckedChanged += CheckboxHeatmap;

            // Update image
            updateDisplay();
        }

        private void checkBoxLog_CheckedChanged(object sender, EventArgs e)
        {
            updateDisplay();
        }

        private void checkBoxAverageSpots_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked) textBoxSpots.Text = "";
            else textBoxSpots.Text = (currentSpot + 1).ToString();
            updateDisplay();
        }

        #endregion Checkboxes

        #region Other form interactions

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display Help
            var message = "Help:                                    \n\n";
            message += " 1. Menu File, Load Data Files...\n";
            message += " 2. Enter expected mass for Product, Starting Material, and Internal Standard.\n";
            message += " 3. Adjust sliders as required.\n";
            message += " 4. Left-Click on well to view MS.\n";
            message += " \n";
            MessageBox.Show(message, @"IR-MALDESI Browser");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display About Box
            MessageBox.Show($@"IR-MALDESI Browser                " +
                            $@"(C) 2022 AbbVie Inc.  " +
                            $@"Based on:    " +
                            $@"DESI MS Data Browser by Philip A. Searle");
        }

        private void ClearStatusLabels()
        {
            // Clear all status labels
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripStatusLabel5.Text = "";
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((Button)sender).Name} clicked.");

            //Check if anything loaded
            if (RootRawDataFile.IsNullOrEmpty())
            {
                MessageBox.Show(@"Please load a file first!");
                return;
            }

            // Check if
            if (ProdCombo.SelectedIndex == -1)
            {
                if (ProdCombo.Text == "") ProdCombo.Text = "0";
                ProdCombo.Items.Add(ProdCombo.Text);
                ProdCombo.SelectedIndex = ProdCombo.Items.Count - 1;
            }

            // Reload datafiles
            LoadDataFiles(RootRawDataFile);
            DisplayChromatogram(CurrentRow, CurrentColumn, currentSpot);

            // Update image
            updateDisplay();
        }

        private void TrackBarScroll(object sender, EventArgs e)
        {
            // Textbox
            txtLB.Text = $@"{trackBarLB.Value} %";
            txtUB.Text = $@"{trackBarUB.Value} %";
            txtCV.Text = $@"{trackBarCV.Value} %";

            // Heatmap bounds
            heatmapLB.Text = $@"{trackBarLB.Value} %";
            heatmapUB.Text = $@"{trackBarUB.Value} %";

            Invalidate();
        }

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Process data when enter key pressed on Product, StartingMaterial, InternalStandard masses or Noise Level
            if (e.KeyCode != Keys.Enter) return;

            //Check if anything loaded
            if (RootRawDataFile.IsNullOrEmpty())
            {
                MessageBox.Show(@"Please load a file first!");
                return;
            }

            // Reload datafiles
            LoadDataFiles(RootRawDataFile);
            DisplayChromatogram(CurrentRow, CurrentColumn, currentSpot);
        }

        private void minusSpot_Click(object sender, EventArgs e)
        {
            if (checkBoxAverageSpots.Checked == false)
            {
                currentSpot -= 1;
                if (currentSpot < 0) currentSpot = 0;
                textBoxSpots.Text = (currentSpot + 1).ToString();
                updateDisplay();
            }
        }

        private void plusSpot_Click(object sender, EventArgs e)
        {
            if (checkBoxAverageSpots.Checked == false)
            {
                currentSpot += 1;
                int max = Processing.allData.Select(x => x.spot).Max();
                if (currentSpot > max) currentSpot = max;
                textBoxSpots.Text = (currentSpot + 1).ToString();
                updateDisplay();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            int wellNum = e.RowIndex * numCols + e.ColumnIndex;//0 indexing   // + 1;
            WellClicked(e.RowIndex, e.ColumnIndex);
        }

        private void saveHeatmapBtn_Click(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((Button)sender).Name} clicked.");

            DataSaving.saveHeatmap();
        }

        private void batchViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new BatchForm(this);
            frm.Show();
        }

        private void debugViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_debugForm == null || _debugForm.IsDisposed)
            {
                _debugForm = new DebugForm(this);
                _debugForm.Show();
                _debugForm.Top = Top;
                _debugForm.Left = Left + Width - _debugForm.Width;
            }
        }

        private void convertDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            db.Convertdb();
        }

        private void exportRerunMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMaldesImethod();
        }

        // Return true flag if issue
        private bool SaveMaldesImethod()
        {
            // Try to read method from database
            var method = db.GetMaldesiMethod();

            if (method == null)// Return method by other means...
            {
                method = new MALDESImethod();

                // Info pull
                var info = Processing.allData.First();

                // Populate
                method.AlignmentLaserState = info.AlignmentLaserState;
                method.PulsesPerBurst = info.PulsesPerBurst;
                method.BurstsPerSpot = info.BurstsPerSpot;
                method.SpotsPerTrigger = info.SpotsPerTrigger;
                method.DelayAfterTrigger = info.DelayAfterTrigger;
                method.DelayAfterCtrapOpen = info.DelayAfterCtrapOpen;
                method.DelayBetweenBursts = info.DelayBetweenBursts;
                method.DelayBetweenSpots = info.DelayBetweenSpots;

                // Buffer
                method.ESISolventName = info.ESISolventName;
                method.ESISolventFlowRate = info.ESISolventFlowRate;

                // Laser position
                method.laserZ = info.laserZ;

                // Z plate position
                method.z = info.z; // _axygen.Z;

                method.TimeStamp = DateTime.Now.ToString("yyyy_MM_dd_HHmm");

                // Chiller temp
                method.TECactualTemp = info.TECactualTemp;

                // Kinetics
                method.kinetics = false;//for now
                if (method.kinetics)
                {
                    //method.timepoint = int.Parse(_manualControlForm.Timepoints.Text);
                    //method.timepointMinutesDelay = double.Parse(_manualControlForm.minimumWaitTime.Text);
                }
                else
                {
                    method.timepoint = 1;
                    method.timepointMinutesDelay = 0;
                }

                /*

				// In well raster
				method.inWellRaster = _manualControlForm.inWellRasterCheck.Checked;
				method.inWellSeparation = double.Parse(_manualControlForm.inWellRasterSeparation.Text);

				// Post scan
				method.postScanPullBack = _manualControlForm.postScanPullBack.Checked;

				// Get well plate
				method.wellPlate = _manualControlForm._wellPlate._wellPlateStatus;
				*/
            }

            // Update well plate status based on current CV threshold
            method.wellPlate = rerun;

            // Write if needed
            var path = $"{RootRawDataFile.Substring(0, RootRawDataFile.Length - 22)}_methodRedo";
            if (path.Substring(Math.Max(0, path.Length - 5)).Equals(".json") == false) path += ".json";
            JSON.serializeMALDESImethod(path, method);

            return false;
        }

        private void ProdPlus_Click(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((Button)sender).Name} clicked.");

            ProdCombo.Items.Add(ProdCombo.Text);
        }

        private void ProdMinus_Click(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((Button)sender).Name} clicked.");

            ProdCombo.Items.Remove(ProdCombo.SelectedItem);
        }

        private void ProdCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ProdCSV_Click(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((Button)sender).Name} clicked.");

            // Open CSV file
            string filePath = null;
            string fileContent = null;
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\radosax\Desktop\browser example";
                openFileDialog.Filter = @"All files (*.*)|*.*|csv files (*.csv)|*.csv|Text files (*.txt*)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (var reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
                else return;
            }

            // Populate list
            ProdCombo.Items.Clear();
            string[] stringSeparators = { "\r\n" };
            foreach (string s in fileContent.Split(stringSeparators, StringSplitOptions.None))
            {
                if (!s.IsNullOrEmpty()) ProdCombo.Items.Add(s);
            }

            // Go to first one
            ProdCombo.SelectedIndex = 0;
        }

        private void plusShift_Click(object sender, EventArgs e)
        {
            Shift.Text = (int.Parse(Shift.Text) + 1).ToString();
            btnProcess.PerformClick();
        }

        private void minusShift_Click(object sender, EventArgs e)
        {
            Shift.Text = (int.Parse(Shift.Text) - 1).ToString();
            btnProcess.PerformClick();
        }

        private void AnalysisMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool state;
            switch (((ToolStripComboBox)sender).SelectedIndex)
            {
                case 0:
                    state = false;
                    threshold.Visible = state;
                    ThresholdLabel.Visible = state;
                    CustomAdduct.Visible = state;
                    CustomAdductLabel.Visible = state;

                    state = true;
                    SMHLabel.Visible = state;
                    ISHLabel.Visible = state;
                    ProductMHLabel.Visible = state;
                    ProductMHLabel.Text = "Product MH+";
                    ProdMinus.Visible = state;
                    ProdPlus.Visible = state;
                    ProdCSV.Visible = state;
                    ProdCombo.Visible = state;
                    txtStartingMaterial.Visible = state;
                    txtInternalStandard.Visible = state;
                    PPMLabel.Location = new Point(10, 55);
                    PPMLabel.Text = "PPM (+/-)";

                    // Heatmap display
                    checkBoxProduct.Text = "Product";
                    checkBoxSM.Text = "Starting Material";
                    checkBoxIS.Text = "Internal Standard";
                    checkBoxConv.Visible = state;
                    checkBoxRatio.Visible = state;
                    checkBoxRatioTIC.Visible = state;
                    checkBoxSM.Visible = state;
                    checkBoxIS.Visible = state;

                    break;

                case 1:

                    state = false;
                    SMHLabel.Visible = state;
                    ISHLabel.Visible = state;
                    ProductMHLabel.Visible = state;
                    ProdMinus.Visible = state;
                    ProdPlus.Visible = state;
                    ProdCSV.Visible = state;
                    ProdCombo.Visible = state;
                    txtStartingMaterial.Visible = state;
                    txtInternalStandard.Visible = state;
                    PPMLabel.Location = new Point(80, 55);
                    PPMLabel.Text = "PPM (+/-)";

                    // Heatmap display
                    checkBoxProduct.Text = "Hydrogen";
                    checkBoxSM.Text = "Adduct";
                    checkBoxIS.Text = "Custom Adduct";
                    checkBoxConv.Visible = state;
                    checkBoxRatio.Visible = state;
                    checkBoxRatioTIC.Visible = state;

                    state = true;
                    threshold.Visible = state;
                    ThresholdLabel.Visible = state;
                    CustomAdduct.Visible = state;
                    CustomAdductLabel.Visible = state;

                    break;

                case 2:// Deconvoluted protein

                    state = false;
                    SMHLabel.Visible = state;
                    ISHLabel.Visible = state;
                    ProdCSV.Visible = state;
                    txtStartingMaterial.Visible = state;
                    txtInternalStandard.Visible = state;
                    PPMLabel.Location = new Point(10, 55);
                    PPMLabel.Text = "Da Window (+/-)";
                    CustomAdduct.Visible = state;
                    CustomAdductLabel.Visible = state;

                    // Heatmap display
                    checkBoxProduct.Text = "Bound Protein";
                    checkBoxIS.Text = "Unbound Protein / TIC";
                    checkBoxSM.Visible = state;

                    // checkBoxIS.Visible = state;
                    checkBoxConv.Visible = state;
                    checkBoxRatio.Visible = state;
                    checkBoxRatioTIC.Visible = state;

                    state = true;
                    threshold.Visible = state;
                    ThresholdLabel.Visible = state;
                    ProductMHLabel.Visible = state;
                    ProductMHLabel.Text = "Reference Mass";
                    ProdCombo.Visible = state;
                    ProdMinus.Visible = state;
                    ProdPlus.Visible = state;

                    break;

                default: break;
            }

            if (Processing != null)
            {
                btnProcess.PerformClick();
            }
        }

        private void AnalysisMode_Click(object sender, EventArgs e)
        {
            btnProcess.PerformClick();
        }

        #region Plate Map

        private void barcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new EditBarcode(db);
            frm.Show();
        }

        private void loadPlateMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // See if a db is open
            if (db.CurrentFile == null)
            {
                MessageBox.Show(@"Please load data first!");
                return;
            }

            // Store plate map list
            db.SetPlateMap(ManualPlateMapLoad());
        }

        private void clearPlateMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // See if a db is open
            if (db.CurrentFile == null)
            {
                MessageBox.Show(@"Please load data first!");
                return;
            }

            // Clear
            db.SetPlateMap(new List<HTTPQuery.PlateMap>());
        }

        private void exportPlateMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // See if a db is open
            if (db.CurrentFile == null)
            {
                MessageBox.Show(@"Please load data first!");
                return;
            }

            // Get current plate map
            var map = db.GetPlateMap();

            // Write CSV
            var parts = new FileInfo(RootRawDataFile);
            using (var writer = new StreamWriter(Path.Combine("", "")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Register mapping for order of columns in Excel
                csv.Configuration.RegisterClassMap<HTTPQuery.csvPlateMap>();
                csv.WriteRecords(map);
            }
        }

        #endregion Plate Map

        private void CheckCVCutoff_CheckedChanged(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((CheckBox)sender).Name} clicked.");

            if (CheckCVCutoff.Checked)
            {
                trackBarCV.Visible = true;
                txtCV.Visible = true;
            }
            else
            {
                trackBarCV.Visible = false;
                txtCV.Visible = false;
            }

            updateDisplay();
        }

        private void AnalysisMode_Click_1(object sender, EventArgs e)
        {
        }

        private void sumOrMax_Click(object sender, EventArgs e)
        {
            // Logging
            Log.Info($"{Name} {((Button)sender).Name} clicked.");

            if (sumOrMax.Text.Equals("Sum Range"))
            {
                Processing.sumRange = false;
                sumOrMax.Text = @"Max Range";
            }
            else
            {
                Processing.sumRange = true;
                sumOrMax.Text = @"Sum Range";
            }
            btnProcess.PerformClick();
        }

        #endregion Other form interactions

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw scale for heatmap
            base.OnPaint(e);

            // size
            float xPos = 140;
            float yPos = 315;
            float width = 200;  // multiple of 100
            float height = 10;

            // Draw black outline
            var pen = new Pen(Color.Black);
            e.Graphics.DrawRectangle(pen, xPos, yPos, width + 1, height + 1);

            // Draw color map from 0-99%
            int N = trackBarUB.Value - trackBarLB.Value;
            for (int i = trackBarLB.Value; i <= trackBarUB.Value; i++)
            {
                var solidBrush = new SolidBrush(GetHeatMapColor(i));
                e.Graphics.FillRectangle(solidBrush, xPos + 1 + (i - trackBarLB.Value) * width / N, yPos + 1, width / N, height);
            }
        }
    }
}