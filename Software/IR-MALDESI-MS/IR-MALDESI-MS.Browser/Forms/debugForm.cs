using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace IR_MALDESI.Browser.Forms
{
    public partial class DebugForm : Form
    {
        private Browser _core;

        public List<string> csv;

        public DebugForm(Browser temp)
        {
            InitializeComponent();
            _core = temp;
        }

        private void debugForm_Load(object sender, EventArgs e)
        {
            _core.WellClicked(_core.CurrentRow, _core.CurrentColumn);
        }

        private void camera1_Click(object sender, EventArgs e)
        {
        }

        private void exportCSV_Click(object sender, EventArgs e)
        {
            int N = waveForm.Model.Series.Count;

            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "debug.csv");
            using (var writer = new StreamWriter(new FileStream(filepath,
                FileMode.Create, FileAccess.Write)))
            {
                foreach (string s in csv)
                {
                    writer.WriteLine($"{s},");
                }
            }

            //Process.Start(@"c:\temp");
        }
    }
}