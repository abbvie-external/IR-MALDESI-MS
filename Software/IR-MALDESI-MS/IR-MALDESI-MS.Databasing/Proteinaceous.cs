using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;

namespace IR_MALDESI.Databasing
{
    public static class Proteinaceous
    {
        public class DeconvolutedSpectrum
        {
            public List<double> mass { get; set; }
            public List<List<double>> Intensity { get; set; }
        }

        public static DeconvolutedSpectrum LoadCSV(string startPath = "c:\\")
        {
            // Select file
            var temp = new DeconvolutedSpectrum();
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = startPath;
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Select the .csv file with deconvoluted spectra";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    using (var reader = new StreamReader(filePath))
                    {
                        // See header for lengths
                        var header = reader.ReadLine().Split(',').ToList();
                        int scanind = header.FindIndex(x => x == "Scan#1");
                        temp.Intensity = new List<List<double>>();
                        temp.mass = new List<double>();

                        while (reader.Peek() != -1)
                        {
                            string[] s = reader.ReadLine()?.Split(',');

                            temp.mass.Add(double.Parse(s[0]));
                            temp.Intensity.Add(s.ToList().GetRange(scanind, s.Length - scanind).Select(double.Parse).ToList());
                        }
                    }
                }
                else return temp;
            }

            return temp;
        }
    }
}