using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR_MALDESI.Databasing
{
    public class MALDESImethod : scanInfo
    {
        // Method name
        public string MaldesiMethodName { get; set; }

        // Barcode
        public string Barcode { get; set; }

        // Well plate map
        public List<wellPlate.wellPlateStatus> wellPlate = new List<wellPlate.wellPlateStatus>();

        // kinetics
        public bool kinetics { get; set; }

        public double timepointMinutesDelay { get; set; }

        // in well raster
        public bool inWellRaster { get; set; }

        public double inWellSeparation { get; set; }

        // Servo speed (rev/second)
        public double servoSpeed { get; set; }

        // Plate slot
        public int plateSlot { get; set; }

        // Delid?
        public bool removeLid { get; set; }

        public MALDESImethod DeepCopy()
        {
            // Shallow copy
            var temp = (MALDESImethod)MemberwiseClone();

            // Deep transfer for strings, classes, and lists
            temp.ESISolventName = string.Copy(ESISolventName);
            temp.TimeStamp = string.Copy(TimeStamp);
            temp.ScanMode = ScanMode;
            if (XMethod != null)
            {
                temp.XMethod = XMethod;
            }

            // Well plate
            temp.wellPlate = new List<wellPlate.wellPlateStatus>(wellPlate);

            // Return it
            return temp;
        }
    }

    public class MALDESImethoddb2 : scanInfodb2
    {
        // Method name
        public string MaldesiMethodName { get; set; }

        // Well plate map
        public List<wellPlate.wellPlateStatus> wellPlate = new List<wellPlate.wellPlateStatus>();

        // kinetics
        public bool kinetics { get; set; }

        public double timepointMinutesDelay { get; set; }

        // in well raster
        public bool inWellRaster { get; set; }

        public double inWellSeparation { get; set; }

        // Servo speed (rev/second)
        public double servoSpeed { get; set; }

        // Plate slot
        public int plateSlot { get; set; }

        // Delid?
        public bool removeLid { get; set; }

        public MALDESImethoddb2 DeepCopy()
        {
            // Shallow copy
            var temp = (MALDESImethoddb2)MemberwiseClone();

            temp.ESISolventName = string.Copy(ESISolventName);
            temp.TimeStamp = string.Copy(TimeStamp);
            temp.ScanMode = ScanMode;

            //if (XMethod != null) temp.XMethod = new XMethod();

            // Well plate
            temp.wellPlate = new List<wellPlate.wellPlateStatus>(wellPlate);

            // Return it
            return temp;
        }
    }
}