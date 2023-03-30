using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR_MALDESI.Databasing
{
    public class timepoint : scanInfo
    {
        public double[] mz { get; set; }
        public double[] I { get; set; }
        public double timeFromRaw { get; set; }

        public double SM { get; set; }
        public double PROD { get; set; }
        public double IS { get; set; }
        public double TIC { get; set; }

        public timepoint DeepCopy()
        {
            // Shallow copy
            var temp = (timepoint)MemberwiseClone();

            //temp.ESISolventName = string.Copy(ESISolventName);
            temp.TimeStamp = string.Copy(TimeStamp);
            temp.ScanMode = ScanMode;
            if (XMethod != null) temp.XMethod = new XMethod();

            // Return it
            return temp;
        }
    }
}