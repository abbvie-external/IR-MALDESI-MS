using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR_MALDESI.Databasing
{
    public class Calibrations
    {
        public double LoadX { get; set; }
        public double LoadY { get; set; }
        public double LoadZ { get; set; }
        public int LoadTwisterZ { get; set; }
        public int LoadTwisterTheta { get; set; }

        public int ConveyorTwisterZ { get; set; }
        public int ConveyorTwisterTheta { get; set; }
        public double LoadConveyorXOffset { get; set; }

        public int BarcodeTwisterZ { get; set; }
        public int BarcodeTwisterTheta { get; set; }

        public double RestX { get; set; }
        public double RestY { get; set; }
        public double RestZ { get; set; }

        public double A1X { get; set; }
        public double A1Y { get; set; }

        public double P24X { get; set; }
        public double P24Y { get; set; }

        public int minCycleTime { get; set; }

        public bool simulation { get; set; }
    }
}