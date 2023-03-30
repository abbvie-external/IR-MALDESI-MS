using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR_MALDESI.Databasing
{
    public class scanInfo
    {
        // Filename
        public string Filename { get; set; }

        // Laser
        public bool AlignmentLaserState { get; set; }
        public int PulsesPerBurst { get; set; }
        public int BurstsPerSpot { get; set; }
        public int SpotsPerTrigger { get; set; }
        public int DelayAfterTrigger { get; set; }
        public int DelayAfterCtrapOpen { get; set; }
        public int DelayBetweenBursts { get; set; }
        public int DelayBetweenSpots { get; set; }
        public int CounterN { get; set; }
        public int DelayCounterN { get; set; }

        // Syringe

        public int ESIValveIndex { get; set; }

        public string ESISolventName { get; set; }

        public double ESISolventFlowRate { get; set; }

        // General
        public string TimeStamp { get; set; }// Time stamp for start of run

        public double time { get; set; }// Time in seconds from start of run
        public int row { get; set; }
        public int column { get; set; }
        public int spot { get; set; }
        public int timepoint { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        // Plate format
        public int PlateFormat { get; set; }

        public bool ScanAcrossColumnsFirst { get; set; }

        // Post scan
        public bool postScanPullBack { get; set; }

        public bool postScanStandby { get; set; }// true means standby, false is on

        // TEC
        public double TECactualTemp { get; set; }

        public double laserZ { get; set; }

        // Xcalibur method
        //public string Xmethod { get; set; }
        public XMethod XMethod { get; set; }

        public ScanMode ScanMode { get; set; }

        public scanInfo DeepCopy()
        {
            // Shallow copy
            var temp = (scanInfo)MemberwiseClone();

            //temp.ESISolventName = string.Copy(ESISolventName);
            temp.TimeStamp = string.Copy(TimeStamp);
            temp.ScanMode = ScanMode;
            if (XMethod != null) temp.XMethod = new XMethod();

            // Return it
            return temp;
        }
    }

    public class scanInfodb2
    {
        // Filename
        public string Filename { get; set; }

        // Laser
        public bool AlignmentLaserState { get; set; }
        public int PulsesPerBurst { get; set; }
        public int BurstsPerSpot { get; set; }
        public int SpotsPerTrigger { get; set; }
        public int DelayAfterTrigger { get; set; }
        public int DelayAfterCtrapOpen { get; set; }
        public int DelayBetweenBursts { get; set; }
        public int DelayBetweenSpots { get; set; }
        public int CounterN { get; set; }
        public int DelayCounterN { get; set; }

        // Syringe

        public int ESIValveIndex { get; set; }

        public string ESISolventName { get; set; }

        public double ESISolventFlowRate { get; set; }

        // General
        public string TimeStamp { get; set; }// Time stamp for start of run

        public double time { get; set; }// Time in seconds from start of run
        public int row { get; set; }
        public int column { get; set; }
        public int spot { get; set; }
        public int timepoint { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        // Plate format
        public int PlateFormat { get; set; }

        public bool ScanAcrossColumnsFirst { get; set; }

        // Post scan
        public bool postScanPullBack { get; set; }

        public bool postScanStandby { get; set; }// true means standby, false is on

        // TEC
        public double TECactualTemp { get; set; }

        public double laserZ { get; set; }

        // Xcalibur method
        public string Xmethod { get; set; }

        //public XMethod XMethod { get; set; }

        public ScanMode ScanMode { get; set; }

        public scanInfo DeepCopy()
        {
            // Shallow copy
            var temp = (scanInfo)MemberwiseClone();

            //temp.ESISolventName = string.Copy(ESISolventName);
            temp.TimeStamp = string.Copy(TimeStamp);
            temp.ScanMode = ScanMode;

            //if (XMethod != null) temp.XMethod = new XMethod();

            // Return it
            return temp;
        }
    }
}