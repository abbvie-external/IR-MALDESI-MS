using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutomationEngineering.LoggingHelper;
using Emgu.CV;
using Emgu.CV.Structure;
using log4net;
using Sparcs.TelemetryFile;
using Sparcs.TelemetryFile.ResultTables;
using static IR_MALDESI.Databasing.MSprotocol;

namespace IR_MALDESI.Databasing
{
    public class Database
    {
        // create public telemetry store
        private TelemetryStore _ts;
        public ResultTable Table;
        public string CurrentFile;

        // logging
        private static readonly ILog Log = LogCreator.ClassLogger();

        // Path is is the direct name of the file that the database is in
        public void CreateDatabase(string file, MALDESImethod method, int sampleRate)
        {
            // Error about sqlite dll? Make sure sparcs.telemetryfile is installed in project
            if (File.Exists(file))
            {
                file += "_2.db2";
            }

            _ts = TelemetryStore.Create(file);
            Table = _ts.Results.Create("MS Capture", new[] { "Row", "Column", "Webcam0", "Webcam1", "time", "readyOut", "startOut", "startIn", "cTrapOut", "Laser", "TimeStamp" });

            //Table.CreateIndex(new ResultTableIndex("Frame"));

            // Metadata
            Table.Metadata.Create("DAQ Sample Rate", sampleRate);// hardcode to start...
            SetMaldesiMethod(method);

            Log.Debug($"Creating database {file}");

            CurrentFile = file;
        }

        public void WriteDatabase(int row, int column, Image<Bgr, byte> webcam0, Image<Bgr, byte> webcam1, List<waveForm> waveform, double time)
        {
            // Convert to new
            uint[] t = waveform.Select(x => x.time).ToArray();
            int[] ro = waveform.Select(x => x.readyOut).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            int[] so = waveform.Select(x => x.startOut).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            int[] si = waveform.Select(x => x.startIn).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            int[] ct = waveform.Select(x => x.cTrapOpen).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            byte[] l = waveform.Select(x => x.Laser).ToArray();

            Table.QueueResultRow(new object[] { row, column, webcam0.Convert<Gray, byte>(), webcam1.Convert<Gray, byte>(), t, ro, so, si, ct, l, time });
        }

        // Only allows so you can write to other than the current table
        private void writeDatabase(int row, int column, Image<Bgr, byte> webcam0, Image<Bgr, byte> webcam1,
            List<waveForm> waveform, double time, ResultTable currentTable)
        {
            // Convert to new
            int[] ro = waveform.Select(x => x.readyOut).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            int[] so = waveform.Select(x => x.startOut).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            int[] si = waveform.Select(x => x.startIn).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            int[] ct = waveform.Select(x => x.cTrapOpen).ToList().ConvertAll(x => x ? 1 : 0).ToArray();
            byte[] l = waveform.Select(x => x.Laser).ToArray();

            //null some to reduce data
            so = null;//not recorded now

            currentTable.QueueResultRow(new object[] { row, column, webcam0.Convert<Gray, byte>(), webcam1.Convert<Gray, byte>(), ro, so, si, ct, l, time });
        }

        // Flag raised if databased not loaded
        public bool LoadDatabase(string fileIn)
        {
            CurrentFile = fileIn;
            Log.Debug($"Loading {fileIn} database");

            if (_ts != null && !Path.GetDirectoryName(_ts.FilePath).Equals(CurrentFile))
            {
                CloseDatabase(false);
                _ts = null;
            }

            //Image capture table including frame number, image, and capture info
            if (_ts == null)
            {
                if (File.Exists(CurrentFile))
                {
                    try
                    {
                        _ts = TelemetryStore.OpenExisting(CurrentFile);
                        Table = _ts.Results.RetrieveByName("MS Capture");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.YesNo);
                    }
                }
                else
                {
                    CreateDatabase(CurrentFile, new MALDESImethod(), 50000);
                }
                return false;
            }

            if (string.Equals(CurrentFile, _ts.FilePath)) return false;
            if (File.Exists(CurrentFile))
            {
                _ts = TelemetryStore.OpenExisting(CurrentFile);
                Table = _ts.Results.RetrieveByName("MS Capture");
                return false;
            }
            return true;
        }

        public void CloseDatabase(bool wait = true)
        {
            if (_ts == null) return;
            Log.Debug($"Closing database {CurrentFile}");

            if (wait)
            {
                _ts.Close();
            }
            else
            {
                // send off in a task or else it holds up the software
                Task.Run(() =>
                {
                    var copy = _ts;
                    if (copy != null) copy.Close();
                });
            }
        }

        public ResultRow PullObject(int row, int column)
        {
            var t = Table.RetrieveResultRows($"row = {row} and column = {column}");
            if (t.Count > 0)
            {
                return t.Single();
            }

            t = Table.RetrieveResultRows($"row = {row}");
            if (t.Count > 0)
            {
                return t.First();
            }

            return Table.RetrieveResultRows().FirstOrDefault();
        }

        public void Convertdb()
        {
            var ts2 = TelemetryStore.Create($"{CurrentFile}2");

            var rtWrite = ts2.Results.Create("MS Capture", new[] { "Row", "Column", "Webcam0", "Webcam1", "readyOut", "startOut", "startIn", "cTrapOut", "Laser", "TimeStamp" });

            // metadata
            rtWrite.Metadata.Create("DAQ Sample Rate", 50000);// hardcode to start...

            // Convert over
            var allRowsLazyRead = Table.RetrieveResultRowsLazy();

            //var allRowsLazyRead = Table.RetrieveResultRows();

            foreach (var row in allRowsLazyRead)
            {
                // Read old with name
                var r = row.GetColumn<int>("Row");
                var c = row.GetColumn<int>("Column");
                var wc0 = row.GetColumn<Image<Bgr, byte>>("Webcam0");
                var wc1 = row.GetColumn<Image<Bgr, byte>>("Webcam1");
                var tstamp = row.GetColumn<double>("TimeStamp");
                var wf = row.GetColumn<List<waveForm>>("WaveForm");

                writeDatabase(r, c, wc0, wc1, wf, tstamp, rtWrite);
            }

            ts2.Flush();
            ts2.Close();
        }

        #region Metadata

        public void SetPlateMap(List<HTTPQuery.PlateMap> platemap)
        {
            Table.Metadata.Create("PlateMap", platemap);
        }

        public List<HTTPQuery.PlateMap> GetPlateMap()
        {
            return Table.Metadata.Retrieve<List<HTTPQuery.PlateMap>>("PlateMap");
        }

        public void SetMaldesiMethod(MALDESImethod method)
        {
            // Updates to latest version
            Table.Metadata.Create("Method", method);
        }

        public MALDESImethod GetMaldesiMethod()
        {
            var method = new MALDESImethod();
            if (Table == null) return method;

            try
            {
                method = Table.Metadata.Retrieve<MALDESImethod>("Method");
            }
            catch
            {
                // Changed the definition of method at some point, so need to use an older version for this data
                var temp = Table.Metadata.Retrieve<MALDESImethoddb2>("Method");

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

        #endregion Metadata
    }
}