using System.ComponentModel;
using System.Windows.Forms;
using Emgu.CV.UI;
using OxyPlot.WindowsForms;

namespace IR_MALDESI.Browser.Forms
{
    partial class DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            this.camera1 = new Emgu.CV.UI.ImageBox();
            this.camera0 = new Emgu.CV.UI.ImageBox();
            this.waveForm = new OxyPlot.WindowsForms.PlotView();
            this.metadata = new System.Windows.Forms.ListBox();
            this.metadata2 = new System.Windows.Forms.ListBox();
            this.exportCSV = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.camera1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera0)).BeginInit();
            this.SuspendLayout();
            // 
            // camera1
            // 
            this.camera1.Location = new System.Drawing.Point(266, 3);
            this.camera1.Name = "camera1";
            this.camera1.Size = new System.Drawing.Size(260, 260);
            this.camera1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.camera1.TabIndex = 52;
            this.camera1.TabStop = false;
            this.camera1.Click += new System.EventHandler(this.camera1_Click);
            // 
            // camera0
            // 
            this.camera0.Location = new System.Drawing.Point(0, 3);
            this.camera0.Name = "camera0";
            this.camera0.Size = new System.Drawing.Size(260, 260);
            this.camera0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.camera0.TabIndex = 53;
            this.camera0.TabStop = false;
            // 
            // waveForm
            // 
            this.waveForm.Location = new System.Drawing.Point(0, 268);
            this.waveForm.Name = "waveForm";
            this.waveForm.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.waveForm.Size = new System.Drawing.Size(526, 278);
            this.waveForm.TabIndex = 54;
            this.waveForm.Text = "plotView1";
            this.waveForm.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.waveForm.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.waveForm.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // metadata
            // 
            this.metadata.FormattingEnabled = true;
            this.metadata.HorizontalScrollbar = true;
            this.metadata.Location = new System.Drawing.Point(0, 552);
            this.metadata.Name = "metadata";
            this.metadata.Size = new System.Drawing.Size(260, 160);
            this.metadata.TabIndex = 55;
            // 
            // metadata2
            // 
            this.metadata2.FormattingEnabled = true;
            this.metadata2.HorizontalScrollbar = true;
            this.metadata2.Location = new System.Drawing.Point(265, 553);
            this.metadata2.Name = "metadata2";
            this.metadata2.Size = new System.Drawing.Size(260, 160);
            this.metadata2.TabIndex = 56;
            // 
            // exportCSV
            // 
            this.exportCSV.Location = new System.Drawing.Point(451, 524);
            this.exportCSV.Name = "exportCSV";
            this.exportCSV.Size = new System.Drawing.Size(75, 23);
            this.exportCSV.TabIndex = 57;
            this.exportCSV.Text = "Export CSV";
            this.exportCSV.UseVisualStyleBackColor = true;
            this.exportCSV.Click += new System.EventHandler(this.exportCSV_Click);
            // 
            // debugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(537, 712);
            this.Controls.Add(this.exportCSV);
            this.Controls.Add(this.metadata2);
            this.Controls.Add(this.metadata);
            this.Controls.Add(this.waveForm);
            this.Controls.Add(this.camera0);
            this.Controls.Add(this.camera1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "debugForm";
            this.Text = "debugForm";
            this.Load += new System.EventHandler(this.debugForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.camera1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera0)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public ImageBox camera1;
        public ImageBox camera0;
        public PlotView waveForm;
        public ListBox metadata;
        public ListBox metadata2;
        private Button exportCSV;
    }
}