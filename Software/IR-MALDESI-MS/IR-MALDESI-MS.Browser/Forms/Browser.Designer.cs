using System.ComponentModel;
using System.Windows.Forms;
using OxyPlot.WindowsForms;

namespace IR_MALDESI.Browser.Forms
{
    partial class Browser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
            this.lblChromatogram = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barcodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPlateMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearPlateMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportExcelFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportRerunMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportSpectraToTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AnalysisMode = new System.Windows.Forms.ToolStripComboBox();
            this.groupBoxChrom = new System.Windows.Forms.GroupBox();
            this.chkInternalStandard = new System.Windows.Forms.CheckBox();
            this.chkStartingMaterial = new System.Windows.Forms.CheckBox();
            this.chkProduct = new System.Windows.Forms.CheckBox();
            this.chkChromTIC = new System.Windows.Forms.CheckBox();
            this.groupBoxMasses = new System.Windows.Forms.GroupBox();
            this.ProdCSV = new System.Windows.Forms.Button();
            this.ProdCombo = new System.Windows.Forms.ComboBox();
            this.ProdMinus = new System.Windows.Forms.Button();
            this.ProdPlus = new System.Windows.Forms.Button();
            this.sumOrMax = new System.Windows.Forms.Button();
            this.ISHLabel = new System.Windows.Forms.Label();
            this.SMHLabel = new System.Windows.Forms.Label();
            this.ProductMHLabel = new System.Windows.Forms.Label();
            this.txtInternalStandard = new System.Windows.Forms.TextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtStartingMaterial = new System.Windows.Forms.TextBox();
            this.PPMLabel = new System.Windows.Forms.Label();
            this.txtRange = new System.Windows.Forms.TextBox();
            this.groupBoxThresholds = new System.Windows.Forms.GroupBox();
            this.CheckCVCutoff = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCV = new System.Windows.Forms.TextBox();
            this.trackBarCV = new System.Windows.Forms.TrackBar();
            this.lblUB = new System.Windows.Forms.Label();
            this.lblLB = new System.Windows.Forms.Label();
            this.txtUB = new System.Windows.Forms.TextBox();
            this.txtLB = new System.Windows.Forms.TextBox();
            this.trackBarUB = new System.Windows.Forms.TrackBar();
            this.trackBarLB = new System.Windows.Forms.TrackBar();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.plusShift = new System.Windows.Forms.Button();
            this.minusShift = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Shift = new System.Windows.Forms.TextBox();
            this.checkBoxAverageSpots = new System.Windows.Forms.CheckBox();
            this.plusSpot = new System.Windows.Forms.Button();
            this.minusSpot = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSpots = new System.Windows.Forms.TextBox();
            this.checkBoxLog = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.heatmapLB = new System.Windows.Forms.Label();
            this.heatmapUB = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBoxHeatmap = new System.Windows.Forms.GroupBox();
            this.ThresholdLabel = new System.Windows.Forms.Label();
            this.threshold = new System.Windows.Forms.TextBox();
            this.CustomAdductLabel = new System.Windows.Forms.Label();
            this.CustomAdduct = new System.Windows.Forms.TextBox();
            this.checkBoxRatioTIC = new System.Windows.Forms.CheckBox();
            this.saveHeatmapBtn = new System.Windows.Forms.Button();
            this.checkBoxRatio = new System.Windows.Forms.CheckBox();
            this.checkBoxConv = new System.Windows.Forms.CheckBox();
            this.checkBoxIS = new System.Windows.Forms.CheckBox();
            this.checkBoxSM = new System.Windows.Forms.CheckBox();
            this.checkBoxProduct = new System.Windows.Forms.CheckBox();
            this.MSplot = new OxyPlot.WindowsForms.PlotView();
            this.lblSpectrum = new System.Windows.Forms.Label();
            this.hint = new System.Windows.Forms.Label();
            this.chronPlot = new OxyPlot.WindowsForms.PlotView();
            this.exportPlateMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBoxChrom.SuspendLayout();
            this.groupBoxMasses.SuspendLayout();
            this.groupBoxThresholds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarUB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLB)).BeginInit();
            this.groupBoxOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxHeatmap.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChromatogram
            // 
            this.lblChromatogram.AutoSize = true;
            this.lblChromatogram.Location = new System.Drawing.Point(18, 485);
            this.lblChromatogram.Name = "lblChromatogram";
            this.lblChromatogram.Size = new System.Drawing.Size(70, 13);
            this.lblChromatogram.TabIndex = 15;
            this.lblChromatogram.Text = "Chronogram: ";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5});
            this.statusStrip1.Location = new System.Drawing.Point(0, 690);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1380, 22);
            this.statusStrip1.TabIndex = 25;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(140, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AutoSize = false;
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(260, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(260, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.AutoSize = false;
            this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(260, 17);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(445, 17);
            this.toolStripStatusLabel5.Spring = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.AnalysisMode});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1380, 27);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataFilesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDataFilesToolStripMenuItem
            // 
            this.loadDataFilesToolStripMenuItem.Name = "loadDataFilesToolStripMenuItem";
            this.loadDataFilesToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.loadDataFilesToolStripMenuItem.Text = "Load Data File...";
            this.loadDataFilesToolStripMenuItem.Click += new System.EventHandler(this.loadDataFilesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barcodeToolStripMenuItem,
            this.loadPlateMapToolStripMenuItem,
            this.clearPlateMapToolStripMenuItem,
            this.exportPlateMapToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(72, 23);
            this.editToolStripMenuItem.Text = "Plate Map";
            // 
            // barcodeToolStripMenuItem
            // 
            this.barcodeToolStripMenuItem.Name = "barcodeToolStripMenuItem";
            this.barcodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.barcodeToolStripMenuItem.Text = "Edit Barcode";
            this.barcodeToolStripMenuItem.Click += new System.EventHandler(this.barcodeToolStripMenuItem_Click);
            // 
            // loadPlateMapToolStripMenuItem
            // 
            this.loadPlateMapToolStripMenuItem.Name = "loadPlateMapToolStripMenuItem";
            this.loadPlateMapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadPlateMapToolStripMenuItem.Text = "Load Plate Map";
            this.loadPlateMapToolStripMenuItem.Click += new System.EventHandler(this.loadPlateMapToolStripMenuItem_Click);
            // 
            // clearPlateMapToolStripMenuItem
            // 
            this.clearPlateMapToolStripMenuItem.Name = "clearPlateMapToolStripMenuItem";
            this.clearPlateMapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearPlateMapToolStripMenuItem.Text = "Clear Plate Map";
            this.clearPlateMapToolStripMenuItem.Click += new System.EventHandler(this.clearPlateMapToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugViewToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportExcelFileToolStripMenuItem,
            this.exportRerunMethodToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportSpectraToTextToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 23);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // debugViewToolStripMenuItem
            // 
            this.debugViewToolStripMenuItem.Name = "debugViewToolStripMenuItem";
            this.debugViewToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.debugViewToolStripMenuItem.Text = "Debug View...";
            this.debugViewToolStripMenuItem.Click += new System.EventHandler(this.debugViewToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
            // 
            // exportExcelFileToolStripMenuItem
            // 
            this.exportExcelFileToolStripMenuItem.Name = "exportExcelFileToolStripMenuItem";
            this.exportExcelFileToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exportExcelFileToolStripMenuItem.Text = "Export CSV File...";
            this.exportExcelFileToolStripMenuItem.Click += new System.EventHandler(this.exportExcelFileToolStripMenuItem_Click);
            // 
            // exportRerunMethodToolStripMenuItem
            // 
            this.exportRerunMethodToolStripMenuItem.Name = "exportRerunMethodToolStripMenuItem";
            this.exportRerunMethodToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exportRerunMethodToolStripMenuItem.Text = "Export Re-run Method";
            this.exportRerunMethodToolStripMenuItem.Click += new System.EventHandler(this.exportRerunMethodToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(189, 6);
            // 
            // exportSpectraToTextToolStripMenuItem
            // 
            this.exportSpectraToTextToolStripMenuItem.Name = "exportSpectraToTextToolStripMenuItem";
            this.exportSpectraToTextToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.exportSpectraToTextToolStripMenuItem.Text = "Export Spectra To Text";
            this.exportSpectraToTextToolStripMenuItem.Click += new System.EventHandler(this.exportSpectraToTextToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 23);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewHelpToolStripMenuItem
            // 
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.viewHelpToolStripMenuItem.Text = "View Help";
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // AnalysisMode
            // 
            this.AnalysisMode.Items.AddRange(new object[] {
            "Single m/z Across Plate",
            "Plate Map m/z",
            "Deconvoluted Protein",
            "Plate Map m/z (custom)"});
            this.AnalysisMode.Name = "AnalysisMode";
            this.AnalysisMode.Size = new System.Drawing.Size(150, 23);
            this.AnalysisMode.SelectedIndexChanged += new System.EventHandler(this.AnalysisMode_SelectedIndexChanged);
            this.AnalysisMode.Click += new System.EventHandler(this.AnalysisMode_Click_1);
            // 
            // groupBoxChrom
            // 
            this.groupBoxChrom.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBoxChrom.Controls.Add(this.chkInternalStandard);
            this.groupBoxChrom.Controls.Add(this.chkStartingMaterial);
            this.groupBoxChrom.Controls.Add(this.chkProduct);
            this.groupBoxChrom.Controls.Add(this.chkChromTIC);
            this.groupBoxChrom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBoxChrom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxChrom.Location = new System.Drawing.Point(416, 494);
            this.groupBoxChrom.Name = "groupBoxChrom";
            this.groupBoxChrom.Size = new System.Drawing.Size(404, 33);
            this.groupBoxChrom.TabIndex = 41;
            this.groupBoxChrom.TabStop = false;
            this.groupBoxChrom.Text = "Chronograms";
            // 
            // chkInternalStandard
            // 
            this.chkInternalStandard.AutoSize = true;
            this.chkInternalStandard.Location = new System.Drawing.Point(275, 13);
            this.chkInternalStandard.Name = "chkInternalStandard";
            this.chkInternalStandard.Size = new System.Drawing.Size(127, 17);
            this.chkInternalStandard.TabIndex = 47;
            this.chkInternalStandard.Tag = "3";
            this.chkInternalStandard.Text = "EIC Internal Standard";
            this.chkInternalStandard.UseVisualStyleBackColor = true;
            this.chkInternalStandard.CheckedChanged += new System.EventHandler(this.chkBoxChromatograms_CheckedChanged);
            // 
            // chkStartingMaterial
            // 
            this.chkStartingMaterial.AutoSize = true;
            this.chkStartingMaterial.Location = new System.Drawing.Point(147, 13);
            this.chkStartingMaterial.Name = "chkStartingMaterial";
            this.chkStartingMaterial.Size = new System.Drawing.Size(122, 17);
            this.chkStartingMaterial.TabIndex = 46;
            this.chkStartingMaterial.Tag = "2";
            this.chkStartingMaterial.Text = "EIC Starting Material";
            this.chkStartingMaterial.UseVisualStyleBackColor = true;
            this.chkStartingMaterial.CheckedChanged += new System.EventHandler(this.chkBoxChromatograms_CheckedChanged);
            // 
            // chkProduct
            // 
            this.chkProduct.AutoSize = true;
            this.chkProduct.Location = new System.Drawing.Point(58, 13);
            this.chkProduct.Name = "chkProduct";
            this.chkProduct.Size = new System.Drawing.Size(83, 17);
            this.chkProduct.TabIndex = 45;
            this.chkProduct.Tag = "1";
            this.chkProduct.Text = "EIC Product";
            this.chkProduct.UseVisualStyleBackColor = true;
            this.chkProduct.CheckedChanged += new System.EventHandler(this.chkBoxChromatograms_CheckedChanged);
            // 
            // chkChromTIC
            // 
            this.chkChromTIC.AutoSize = true;
            this.chkChromTIC.Checked = true;
            this.chkChromTIC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChromTIC.Location = new System.Drawing.Point(9, 13);
            this.chkChromTIC.Name = "chkChromTIC";
            this.chkChromTIC.Size = new System.Drawing.Size(43, 17);
            this.chkChromTIC.TabIndex = 44;
            this.chkChromTIC.Tag = "0";
            this.chkChromTIC.Text = "TIC";
            this.chkChromTIC.UseVisualStyleBackColor = true;
            this.chkChromTIC.CheckedChanged += new System.EventHandler(this.chkBoxChromatograms_CheckedChanged);
            // 
            // groupBoxMasses
            // 
            this.groupBoxMasses.Controls.Add(this.ProdCSV);
            this.groupBoxMasses.Controls.Add(this.ProdCombo);
            this.groupBoxMasses.Controls.Add(this.ProdMinus);
            this.groupBoxMasses.Controls.Add(this.ProdPlus);
            this.groupBoxMasses.Controls.Add(this.sumOrMax);
            this.groupBoxMasses.Controls.Add(this.ISHLabel);
            this.groupBoxMasses.Controls.Add(this.SMHLabel);
            this.groupBoxMasses.Controls.Add(this.ProductMHLabel);
            this.groupBoxMasses.Controls.Add(this.txtInternalStandard);
            this.groupBoxMasses.Controls.Add(this.btnProcess);
            this.groupBoxMasses.Controls.Add(this.txtStartingMaterial);
            this.groupBoxMasses.Controls.Add(this.PPMLabel);
            this.groupBoxMasses.Controls.Add(this.txtRange);
            this.groupBoxMasses.Location = new System.Drawing.Point(437, 328);
            this.groupBoxMasses.Name = "groupBoxMasses";
            this.groupBoxMasses.Size = new System.Drawing.Size(277, 154);
            this.groupBoxMasses.TabIndex = 42;
            this.groupBoxMasses.TabStop = false;
            this.groupBoxMasses.Text = "Expected Masses";
            // 
            // ProdCSV
            // 
            this.ProdCSV.Location = new System.Drawing.Point(242, 75);
            this.ProdCSV.Name = "ProdCSV";
            this.ProdCSV.Size = new System.Drawing.Size(29, 23);
            this.ProdCSV.TabIndex = 56;
            this.ProdCSV.Text = "***";
            this.ProdCSV.UseVisualStyleBackColor = true;
            this.ProdCSV.Click += new System.EventHandler(this.ProdCSV_Click);
            // 
            // ProdCombo
            // 
            this.ProdCombo.FormattingEnabled = true;
            this.ProdCombo.Items.AddRange(new object[] {
            "191.0182"});
            this.ProdCombo.Location = new System.Drawing.Point(135, 76);
            this.ProdCombo.Name = "ProdCombo";
            this.ProdCombo.Size = new System.Drawing.Size(75, 21);
            this.ProdCombo.TabIndex = 53;
            this.ProdCombo.SelectedIndexChanged += new System.EventHandler(this.ProdCombo_SelectedIndexChanged);
            // 
            // ProdMinus
            // 
            this.ProdMinus.Location = new System.Drawing.Point(108, 75);
            this.ProdMinus.Name = "ProdMinus";
            this.ProdMinus.Size = new System.Drawing.Size(19, 23);
            this.ProdMinus.TabIndex = 55;
            this.ProdMinus.Text = "-";
            this.ProdMinus.UseVisualStyleBackColor = true;
            this.ProdMinus.Click += new System.EventHandler(this.ProdMinus_Click);
            // 
            // ProdPlus
            // 
            this.ProdPlus.Location = new System.Drawing.Point(219, 75);
            this.ProdPlus.Name = "ProdPlus";
            this.ProdPlus.Size = new System.Drawing.Size(19, 23);
            this.ProdPlus.TabIndex = 53;
            this.ProdPlus.Text = "+";
            this.ProdPlus.UseVisualStyleBackColor = true;
            this.ProdPlus.Click += new System.EventHandler(this.ProdPlus_Click);
            // 
            // sumOrMax
            // 
            this.sumOrMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sumOrMax.Location = new System.Drawing.Point(130, 13);
            this.sumOrMax.Name = "sumOrMax";
            this.sumOrMax.Size = new System.Drawing.Size(73, 32);
            this.sumOrMax.TabIndex = 54;
            this.sumOrMax.Text = "Sum Range";
            this.sumOrMax.UseVisualStyleBackColor = true;
            this.sumOrMax.Click += new System.EventHandler(this.sumOrMax_Click);
            // 
            // ISHLabel
            // 
            this.ISHLabel.AutoSize = true;
            this.ISHLabel.Location = new System.Drawing.Point(7, 133);
            this.ISHLabel.Name = "ISHLabel";
            this.ISHLabel.Size = new System.Drawing.Size(114, 13);
            this.ISHLabel.TabIndex = 28;
            this.ISHLabel.Text = "Internal Standard MH+";
            // 
            // SMHLabel
            // 
            this.SMHLabel.AutoSize = true;
            this.SMHLabel.Location = new System.Drawing.Point(7, 107);
            this.SMHLabel.Name = "SMHLabel";
            this.SMHLabel.Size = new System.Drawing.Size(109, 13);
            this.SMHLabel.TabIndex = 27;
            this.SMHLabel.Text = "Starting Material MH+";
            // 
            // ProductMHLabel
            // 
            this.ProductMHLabel.AutoSize = true;
            this.ProductMHLabel.Location = new System.Drawing.Point(7, 81);
            this.ProductMHLabel.Name = "ProductMHLabel";
            this.ProductMHLabel.Size = new System.Drawing.Size(70, 13);
            this.ProductMHLabel.TabIndex = 26;
            this.ProductMHLabel.Text = "Product MH+";
            // 
            // txtInternalStandard
            // 
            this.txtInternalStandard.Location = new System.Drawing.Point(137, 129);
            this.txtInternalStandard.Name = "txtInternalStandard";
            this.txtInternalStandard.Size = new System.Drawing.Size(61, 20);
            this.txtInternalStandard.TabIndex = 25;
            this.txtInternalStandard.Text = "0";
            this.txtInternalStandard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtInternalStandard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(13, 13);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(92, 32);
            this.btnProcess.TabIndex = 45;
            this.btnProcess.Text = "Reprocess Data";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtStartingMaterial
            // 
            this.txtStartingMaterial.Location = new System.Drawing.Point(137, 103);
            this.txtStartingMaterial.Name = "txtStartingMaterial";
            this.txtStartingMaterial.Size = new System.Drawing.Size(61, 20);
            this.txtStartingMaterial.TabIndex = 24;
            this.txtStartingMaterial.Text = "145.0128";
            this.txtStartingMaterial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStartingMaterial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            // 
            // PPMLabel
            // 
            this.PPMLabel.AutoSize = true;
            this.PPMLabel.Location = new System.Drawing.Point(10, 55);
            this.PPMLabel.Name = "PPMLabel";
            this.PPMLabel.Size = new System.Drawing.Size(53, 13);
            this.PPMLabel.TabIndex = 53;
            this.PPMLabel.Text = "PPM (+/-)";
            // 
            // txtRange
            // 
            this.txtRange.Location = new System.Drawing.Point(135, 51);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(63, 20);
            this.txtRange.TabIndex = 52;
            this.txtRange.Text = "5";
            this.txtRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            // 
            // groupBoxThresholds
            // 
            this.groupBoxThresholds.Controls.Add(this.CheckCVCutoff);
            this.groupBoxThresholds.Controls.Add(this.label4);
            this.groupBoxThresholds.Controls.Add(this.txtCV);
            this.groupBoxThresholds.Controls.Add(this.trackBarCV);
            this.groupBoxThresholds.Controls.Add(this.lblUB);
            this.groupBoxThresholds.Controls.Add(this.lblLB);
            this.groupBoxThresholds.Controls.Add(this.txtUB);
            this.groupBoxThresholds.Controls.Add(this.txtLB);
            this.groupBoxThresholds.Controls.Add(this.trackBarUB);
            this.groupBoxThresholds.Controls.Add(this.trackBarLB);
            this.groupBoxThresholds.Location = new System.Drawing.Point(12, 328);
            this.groupBoxThresholds.Name = "groupBoxThresholds";
            this.groupBoxThresholds.Size = new System.Drawing.Size(408, 154);
            this.groupBoxThresholds.TabIndex = 43;
            this.groupBoxThresholds.TabStop = false;
            this.groupBoxThresholds.Text = "Thresholds";
            // 
            // CheckCVCutoff
            // 
            this.CheckCVCutoff.AutoSize = true;
            this.CheckCVCutoff.Checked = true;
            this.CheckCVCutoff.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckCVCutoff.Location = new System.Drawing.Point(72, 119);
            this.CheckCVCutoff.Name = "CheckCVCutoff";
            this.CheckCVCutoff.Size = new System.Drawing.Size(15, 14);
            this.CheckCVCutoff.TabIndex = 46;
            this.CheckCVCutoff.UseVisualStyleBackColor = true;
            this.CheckCVCutoff.CheckedChanged += new System.EventHandler(this.CheckCVCutoff_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "CV cutoff";
            // 
            // txtCV
            // 
            this.txtCV.Location = new System.Drawing.Point(324, 115);
            this.txtCV.Name = "txtCV";
            this.txtCV.ReadOnly = true;
            this.txtCV.Size = new System.Drawing.Size(42, 20);
            this.txtCV.TabIndex = 44;
            this.txtCV.Text = "100 %";
            this.txtCV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // trackBarCV
            // 
            this.trackBarCV.Location = new System.Drawing.Point(107, 103);
            this.trackBarCV.Maximum = 100;
            this.trackBarCV.Name = "trackBarCV";
            this.trackBarCV.Size = new System.Drawing.Size(200, 45);
            this.trackBarCV.TabIndex = 43;
            this.trackBarCV.TickFrequency = 10;
            this.trackBarCV.Value = 100;
            this.trackBarCV.Scroll += new System.EventHandler(this.TrackBarScroll);
            this.trackBarCV.MouseUp += new System.Windows.Forms.MouseEventHandler(this.updateDisplay);
            // 
            // lblUB
            // 
            this.lblUB.AutoSize = true;
            this.lblUB.Location = new System.Drawing.Point(17, 74);
            this.lblUB.Name = "lblUB";
            this.lblUB.Size = new System.Drawing.Size(70, 13);
            this.lblUB.TabIndex = 42;
            this.lblUB.Text = "Upper Bound";
            // 
            // lblLB
            // 
            this.lblLB.AutoSize = true;
            this.lblLB.Location = new System.Drawing.Point(17, 29);
            this.lblLB.Name = "lblLB";
            this.lblLB.Size = new System.Drawing.Size(70, 13);
            this.lblLB.TabIndex = 40;
            this.lblLB.Text = "Lower Bound";
            // 
            // txtUB
            // 
            this.txtUB.Location = new System.Drawing.Point(324, 70);
            this.txtUB.Name = "txtUB";
            this.txtUB.ReadOnly = true;
            this.txtUB.Size = new System.Drawing.Size(42, 20);
            this.txtUB.TabIndex = 39;
            this.txtUB.Text = "100 %";
            this.txtUB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLB
            // 
            this.txtLB.Location = new System.Drawing.Point(324, 25);
            this.txtLB.Name = "txtLB";
            this.txtLB.ReadOnly = true;
            this.txtLB.Size = new System.Drawing.Size(42, 20);
            this.txtLB.TabIndex = 37;
            this.txtLB.Text = "0 %";
            this.txtLB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // trackBarUB
            // 
            this.trackBarUB.Location = new System.Drawing.Point(107, 58);
            this.trackBarUB.Maximum = 100;
            this.trackBarUB.Name = "trackBarUB";
            this.trackBarUB.Size = new System.Drawing.Size(200, 45);
            this.trackBarUB.TabIndex = 36;
            this.trackBarUB.TickFrequency = 10;
            this.trackBarUB.Value = 100;
            this.trackBarUB.Scroll += new System.EventHandler(this.TrackBarScroll);
            this.trackBarUB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.updateDisplay);
            // 
            // trackBarLB
            // 
            this.trackBarLB.Location = new System.Drawing.Point(107, 13);
            this.trackBarLB.Maximum = 100;
            this.trackBarLB.Name = "trackBarLB";
            this.trackBarLB.Size = new System.Drawing.Size(200, 45);
            this.trackBarLB.TabIndex = 34;
            this.trackBarLB.TickFrequency = 10;
            this.trackBarLB.Scroll += new System.EventHandler(this.TrackBarScroll);
            this.trackBarLB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.updateDisplay);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.plusShift);
            this.groupBoxOptions.Controls.Add(this.minusShift);
            this.groupBoxOptions.Controls.Add(this.label5);
            this.groupBoxOptions.Controls.Add(this.Shift);
            this.groupBoxOptions.Controls.Add(this.checkBoxAverageSpots);
            this.groupBoxOptions.Controls.Add(this.plusSpot);
            this.groupBoxOptions.Controls.Add(this.minusSpot);
            this.groupBoxOptions.Controls.Add(this.label6);
            this.groupBoxOptions.Controls.Add(this.textBoxSpots);
            this.groupBoxOptions.Controls.Add(this.checkBoxLog);
            this.groupBoxOptions.Location = new System.Drawing.Point(459, 211);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(255, 98);
            this.groupBoxOptions.TabIndex = 44;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "Options";
            // 
            // plusShift
            // 
            this.plusShift.Location = new System.Drawing.Point(219, 48);
            this.plusShift.Name = "plusShift";
            this.plusShift.Size = new System.Drawing.Size(20, 20);
            this.plusShift.TabIndex = 62;
            this.plusShift.Text = "+";
            this.plusShift.UseVisualStyleBackColor = true;
            this.plusShift.Click += new System.EventHandler(this.plusShift_Click);
            // 
            // minusShift
            // 
            this.minusShift.Location = new System.Drawing.Point(161, 48);
            this.minusShift.Name = "minusShift";
            this.minusShift.Size = new System.Drawing.Size(20, 20);
            this.minusShift.TabIndex = 61;
            this.minusShift.Text = "-";
            this.minusShift.UseVisualStyleBackColor = true;
            this.minusShift.Click += new System.EventHandler(this.minusShift_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(166, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Spectra Shift";
            // 
            // Shift
            // 
            this.Shift.Enabled = false;
            this.Shift.Location = new System.Drawing.Point(186, 48);
            this.Shift.Name = "Shift";
            this.Shift.Size = new System.Drawing.Size(27, 20);
            this.Shift.TabIndex = 59;
            this.Shift.Text = "0";
            this.Shift.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxAverageSpots
            // 
            this.checkBoxAverageSpots.AutoSize = true;
            this.checkBoxAverageSpots.Location = new System.Drawing.Point(40, 37);
            this.checkBoxAverageSpots.Name = "checkBoxAverageSpots";
            this.checkBoxAverageSpots.Size = new System.Drawing.Size(96, 17);
            this.checkBoxAverageSpots.TabIndex = 58;
            this.checkBoxAverageSpots.Text = "Average Spots";
            this.checkBoxAverageSpots.UseVisualStyleBackColor = true;
            this.checkBoxAverageSpots.CheckedChanged += new System.EventHandler(this.checkBoxAverageSpots_CheckedChanged);
            // 
            // plusSpot
            // 
            this.plusSpot.Location = new System.Drawing.Point(100, 74);
            this.plusSpot.Name = "plusSpot";
            this.plusSpot.Size = new System.Drawing.Size(20, 20);
            this.plusSpot.TabIndex = 57;
            this.plusSpot.Text = "+";
            this.plusSpot.UseVisualStyleBackColor = true;
            this.plusSpot.Click += new System.EventHandler(this.plusSpot_Click);
            // 
            // minusSpot
            // 
            this.minusSpot.Location = new System.Drawing.Point(42, 74);
            this.minusSpot.Name = "minusSpot";
            this.minusSpot.Size = new System.Drawing.Size(20, 20);
            this.minusSpot.TabIndex = 56;
            this.minusSpot.Text = "-";
            this.minusSpot.UseVisualStyleBackColor = true;
            this.minusSpot.Click += new System.EventHandler(this.minusSpot_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Current Spot";
            // 
            // textBoxSpots
            // 
            this.textBoxSpots.Enabled = false;
            this.textBoxSpots.Location = new System.Drawing.Point(67, 74);
            this.textBoxSpots.Name = "textBoxSpots";
            this.textBoxSpots.Size = new System.Drawing.Size(27, 20);
            this.textBoxSpots.TabIndex = 54;
            this.textBoxSpots.Text = "1";
            this.textBoxSpots.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxLog
            // 
            this.checkBoxLog.AutoSize = true;
            this.checkBoxLog.Location = new System.Drawing.Point(40, 14);
            this.checkBoxLog.Name = "checkBoxLog";
            this.checkBoxLog.Size = new System.Drawing.Size(71, 17);
            this.checkBoxLog.TabIndex = 5;
            this.checkBoxLog.Text = "LogScale";
            this.checkBoxLog.UseVisualStyleBackColor = true;
            this.checkBoxLog.CheckedChanged += new System.EventHandler(this.checkBoxLog_CheckedChanged);
            // 
            // heatmapLB
            // 
            this.heatmapLB.AutoSize = true;
            this.heatmapLB.Location = new System.Drawing.Point(116, 315);
            this.heatmapLB.Name = "heatmapLB";
            this.heatmapLB.Size = new System.Drawing.Size(21, 13);
            this.heatmapLB.TabIndex = 47;
            this.heatmapLB.Text = "0%";
            // 
            // heatmapUB
            // 
            this.heatmapUB.AutoSize = true;
            this.heatmapUB.Location = new System.Drawing.Point(346, 315);
            this.heatmapUB.Name = "heatmapUB";
            this.heatmapUB.Size = new System.Drawing.Size(33, 13);
            this.heatmapUB.TabIndex = 47;
            this.heatmapUB.Text = "100%";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.CausesValidation = false;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersHeight = 16;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 35);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 44;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(430, 274);
            this.dataGridView1.TabIndex = 48;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // groupBoxHeatmap
            // 
            this.groupBoxHeatmap.Controls.Add(this.ThresholdLabel);
            this.groupBoxHeatmap.Controls.Add(this.threshold);
            this.groupBoxHeatmap.Controls.Add(this.CustomAdductLabel);
            this.groupBoxHeatmap.Controls.Add(this.CustomAdduct);
            this.groupBoxHeatmap.Controls.Add(this.checkBoxRatioTIC);
            this.groupBoxHeatmap.Controls.Add(this.saveHeatmapBtn);
            this.groupBoxHeatmap.Controls.Add(this.checkBoxRatio);
            this.groupBoxHeatmap.Controls.Add(this.checkBoxConv);
            this.groupBoxHeatmap.Controls.Add(this.checkBoxIS);
            this.groupBoxHeatmap.Controls.Add(this.checkBoxSM);
            this.groupBoxHeatmap.Controls.Add(this.checkBoxProduct);
            this.groupBoxHeatmap.Location = new System.Drawing.Point(459, 35);
            this.groupBoxHeatmap.Name = "groupBoxHeatmap";
            this.groupBoxHeatmap.Size = new System.Drawing.Size(255, 170);
            this.groupBoxHeatmap.TabIndex = 49;
            this.groupBoxHeatmap.TabStop = false;
            this.groupBoxHeatmap.Text = "Heatmap";
            // 
            // ThresholdLabel
            // 
            this.ThresholdLabel.AutoSize = true;
            this.ThresholdLabel.Location = new System.Drawing.Point(158, 105);
            this.ThresholdLabel.Name = "ThresholdLabel";
            this.ThresholdLabel.Size = new System.Drawing.Size(54, 13);
            this.ThresholdLabel.TabIndex = 59;
            this.ThresholdLabel.Text = "Threshold";
            this.ThresholdLabel.Visible = false;
            // 
            // threshold
            // 
            this.threshold.Location = new System.Drawing.Point(135, 128);
            this.threshold.Name = "threshold";
            this.threshold.Size = new System.Drawing.Size(100, 20);
            this.threshold.TabIndex = 58;
            this.threshold.Text = "1e4";
            this.threshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.threshold.Visible = false;
            // 
            // CustomAdductLabel
            // 
            this.CustomAdductLabel.AutoSize = true;
            this.CustomAdductLabel.Location = new System.Drawing.Point(26, 105);
            this.CustomAdductLabel.Name = "CustomAdductLabel";
            this.CustomAdductLabel.Size = new System.Drawing.Size(79, 13);
            this.CustomAdductLabel.TabIndex = 57;
            this.CustomAdductLabel.Text = "Custom Adduct";
            this.CustomAdductLabel.Visible = false;
            // 
            // CustomAdduct
            // 
            this.CustomAdduct.Location = new System.Drawing.Point(15, 128);
            this.CustomAdduct.Name = "CustomAdduct";
            this.CustomAdduct.Size = new System.Drawing.Size(100, 20);
            this.CustomAdduct.TabIndex = 56;
            this.CustomAdduct.Text = "0";
            this.CustomAdduct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CustomAdduct.Visible = false;
            // 
            // checkBoxRatioTIC
            // 
            this.checkBoxRatioTIC.AutoSize = true;
            this.checkBoxRatioTIC.Location = new System.Drawing.Point(44, 134);
            this.checkBoxRatioTIC.Name = "checkBoxRatioTIC";
            this.checkBoxRatioTIC.Size = new System.Drawing.Size(91, 17);
            this.checkBoxRatioTIC.TabIndex = 55;
            this.checkBoxRatioTIC.Text = "Product / TIC";
            this.checkBoxRatioTIC.UseVisualStyleBackColor = true;
            this.checkBoxRatioTIC.CheckedChanged += new System.EventHandler(this.CheckboxHeatmap);
            // 
            // saveHeatmapBtn
            // 
            this.saveHeatmapBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveHeatmapBtn.Location = new System.Drawing.Point(172, 12);
            this.saveHeatmapBtn.Name = "saveHeatmapBtn";
            this.saveHeatmapBtn.Size = new System.Drawing.Size(51, 40);
            this.saveHeatmapBtn.TabIndex = 54;
            this.saveHeatmapBtn.Text = "Save\r\nImage";
            this.saveHeatmapBtn.UseVisualStyleBackColor = true;
            this.saveHeatmapBtn.Click += new System.EventHandler(this.saveHeatmapBtn_Click);
            // 
            // checkBoxRatio
            // 
            this.checkBoxRatio.AutoSize = true;
            this.checkBoxRatio.Location = new System.Drawing.Point(44, 111);
            this.checkBoxRatio.Name = "checkBoxRatio";
            this.checkBoxRatio.Size = new System.Drawing.Size(155, 17);
            this.checkBoxRatio.TabIndex = 4;
            this.checkBoxRatio.Text = "Product / Internal Standard";
            this.checkBoxRatio.UseVisualStyleBackColor = true;
            this.checkBoxRatio.CheckedChanged += new System.EventHandler(this.CheckboxHeatmap);
            // 
            // checkBoxConv
            // 
            this.checkBoxConv.AutoSize = true;
            this.checkBoxConv.Location = new System.Drawing.Point(44, 88);
            this.checkBoxConv.Name = "checkBoxConv";
            this.checkBoxConv.Size = new System.Drawing.Size(152, 17);
            this.checkBoxConv.TabIndex = 3;
            this.checkBoxConv.Text = "% Conversion (SM to Prod)";
            this.checkBoxConv.UseVisualStyleBackColor = true;
            this.checkBoxConv.CheckedChanged += new System.EventHandler(this.CheckboxHeatmap);
            // 
            // checkBoxIS
            // 
            this.checkBoxIS.AutoSize = true;
            this.checkBoxIS.Location = new System.Drawing.Point(44, 65);
            this.checkBoxIS.Name = "checkBoxIS";
            this.checkBoxIS.Size = new System.Drawing.Size(107, 17);
            this.checkBoxIS.TabIndex = 2;
            this.checkBoxIS.Text = "Internal Standard";
            this.checkBoxIS.UseVisualStyleBackColor = true;
            this.checkBoxIS.CheckedChanged += new System.EventHandler(this.CheckboxHeatmap);
            // 
            // checkBoxSM
            // 
            this.checkBoxSM.AutoSize = true;
            this.checkBoxSM.Location = new System.Drawing.Point(44, 42);
            this.checkBoxSM.Name = "checkBoxSM";
            this.checkBoxSM.Size = new System.Drawing.Size(102, 17);
            this.checkBoxSM.TabIndex = 1;
            this.checkBoxSM.Text = "Starting Material";
            this.checkBoxSM.UseVisualStyleBackColor = true;
            this.checkBoxSM.CheckedChanged += new System.EventHandler(this.CheckboxHeatmap);
            // 
            // checkBoxProduct
            // 
            this.checkBoxProduct.AutoSize = true;
            this.checkBoxProduct.Checked = true;
            this.checkBoxProduct.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProduct.Location = new System.Drawing.Point(44, 19);
            this.checkBoxProduct.Name = "checkBoxProduct";
            this.checkBoxProduct.Size = new System.Drawing.Size(63, 17);
            this.checkBoxProduct.TabIndex = 0;
            this.checkBoxProduct.Text = "Product";
            this.checkBoxProduct.UseVisualStyleBackColor = true;
            this.checkBoxProduct.CheckedChanged += new System.EventHandler(this.CheckboxHeatmap);
            // 
            // MSplot
            // 
            this.MSplot.Location = new System.Drawing.Point(720, 44);
            this.MSplot.Name = "MSplot";
            this.MSplot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.MSplot.Size = new System.Drawing.Size(648, 457);
            this.MSplot.TabIndex = 50;
            this.MSplot.Text = "plotView1";
            this.MSplot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.MSplot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.MSplot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // lblSpectrum
            // 
            this.lblSpectrum.AutoSize = true;
            this.lblSpectrum.Location = new System.Drawing.Point(725, 25);
            this.lblSpectrum.Name = "lblSpectrum";
            this.lblSpectrum.Size = new System.Drawing.Size(83, 13);
            this.lblSpectrum.TabIndex = 14;
            this.lblSpectrum.Text = "Mass Spectrum:";
            // 
            // hint
            // 
            this.hint.AutoSize = true;
            this.hint.Location = new System.Drawing.Point(1163, 35);
            this.hint.Name = "hint";
            this.hint.Size = new System.Drawing.Size(205, 65);
            this.hint.TabIndex = 51;
            this.hint.Text = "Hold left click over point to see values\r\nHold right click to pan\r\nPress and drag" +
    " scrollwheel to zoom region\r\nDouble click scrollwheel to reset view\r\nZoom into P" +
    "rod, SM, and IS to see range";
            this.hint.Visible = false;
            // 
            // chronPlot
            // 
            this.chronPlot.Location = new System.Drawing.Point(12, 501);
            this.chronPlot.Name = "chronPlot";
            this.chronPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.chronPlot.Size = new System.Drawing.Size(1260, 199);
            this.chronPlot.TabIndex = 52;
            this.chronPlot.Text = "chronPlot";
            this.chronPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.chronPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.chronPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // exportPlateMapToolStripMenuItem
            // 
            this.exportPlateMapToolStripMenuItem.Name = "exportPlateMapToolStripMenuItem";
            this.exportPlateMapToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportPlateMapToolStripMenuItem.Text = "Export Plate Map";
            this.exportPlateMapToolStripMenuItem.Click += new System.EventHandler(this.exportPlateMapToolStripMenuItem_Click);
            // 
            // Browser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1380, 712);
            this.Controls.Add(this.hint);
            this.Controls.Add(this.groupBoxHeatmap);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.heatmapUB);
            this.Controls.Add(this.heatmapLB);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.groupBoxThresholds);
            this.Controls.Add(this.groupBoxMasses);
            this.Controls.Add(this.groupBoxChrom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.lblChromatogram);
            this.Controls.Add(this.lblSpectrum);
            this.Controls.Add(this.chronPlot);
            this.Controls.Add(this.MSplot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1300, 726);
            this.Name = "Browser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IR-MALDESI Data Browser (Modified from DESI MS Browser 2.0)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Browser_FormClosing);
            this.Load += new System.EventHandler(this.Browser_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxChrom.ResumeLayout(false);
            this.groupBoxChrom.PerformLayout();
            this.groupBoxMasses.ResumeLayout(false);
            this.groupBoxMasses.PerformLayout();
            this.groupBoxThresholds.ResumeLayout(false);
            this.groupBoxThresholds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarUB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLB)).EndInit();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxHeatmap.ResumeLayout(false);
            this.groupBoxHeatmap.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label lblChromatogram;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private MenuStrip menuStrip1;
        internal ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadDataFilesToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem viewHelpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private GroupBox groupBoxChrom;
        private CheckBox chkInternalStandard;
        private CheckBox chkStartingMaterial;
        private CheckBox chkProduct;
        private CheckBox chkChromTIC;
        private GroupBox groupBoxMasses;
        private Label ISHLabel;
        private Label SMHLabel;
        private Label ProductMHLabel;
        internal TextBox txtInternalStandard;
        internal TextBox txtStartingMaterial;
        private GroupBox groupBoxThresholds;
        private Label lblUB;
        private Label lblLB;
        private TextBox txtUB;
        private TextBox txtLB;
        private TrackBar trackBarUB;
        private TrackBar trackBarLB;
        private GroupBox groupBoxOptions;
        private Button btnProcess;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private Label PPMLabel;
        internal TextBox txtRange;
        private ToolTip toolTip1;
		private Label heatmapLB;
		private Label heatmapUB;
		private ToolStripMenuItem exportExcelFileToolStripMenuItem;
        internal DataGridView dataGridView1;
        private GroupBox groupBoxHeatmap;
        internal CheckBox checkBoxIS;
        internal CheckBox checkBoxSM;
        internal CheckBox checkBoxProduct;
        internal CheckBox checkBoxRatio;
        internal CheckBox checkBoxConv;
        internal CheckBox checkBoxLog;
        private TextBox textBoxSpots;
        private Label label6;
        private Button plusSpot;
        private Button minusSpot;
        private CheckBox checkBoxAverageSpots;
        private Button saveHeatmapBtn;
        private Button sumOrMax;
        private PlotView MSplot;
        private Label lblSpectrum;
        private Label hint;
        private PlotView chronPlot;
        private ToolStripMenuItem debugViewToolStripMenuItem;
        internal CheckBox checkBoxRatioTIC;
        private Label label4;
        private TextBox txtCV;
        private TrackBar trackBarCV;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exportRerunMethodToolStripMenuItem;
        private Button ProdMinus;
        private Button ProdPlus;
        public ComboBox ProdCombo;
        private Button ProdCSV;
        private Button plusShift;
        private Button minusShift;
        private Label label5;
        private TextBox Shift;
        public ToolStripComboBox AnalysisMode;
        private Label CustomAdductLabel;
        private TextBox CustomAdduct;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem barcodeToolStripMenuItem;
        private Label ThresholdLabel;
        private TextBox threshold;
        private CheckBox CheckCVCutoff;
        private ToolStripMenuItem loadPlateMapToolStripMenuItem;
        private ToolStripMenuItem clearPlateMapToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exportSpectraToTextToolStripMenuItem;
        private ToolStripMenuItem exportPlateMapToolStripMenuItem;
    }
}

