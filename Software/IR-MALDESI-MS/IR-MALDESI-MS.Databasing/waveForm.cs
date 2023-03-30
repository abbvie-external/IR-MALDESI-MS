using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR_MALDESI.Databasing
{
    public class waveForm
    {
        public uint time { get; set; }
        public bool readyOut { get; set; }
        public bool startOut { get; set; } // Don't record unless you're using this! Disk space!
        public bool startIn { get; set; }
        public bool cTrapOpen { get; set; } // Don't record unless you're using this! Disk space!
        public byte Laser { get; set; }

        public waveForm DeepCopy()
        {
            // Shallow copy
            var temp = (waveForm)MemberwiseClone();

            // Return it
            return temp;
        }
    }
}