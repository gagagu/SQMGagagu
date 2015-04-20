using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{
    public class Sensors_Item
    {
        // position array
        public SqmPosition position { get; set; }
        // Name of sensor, not existent by null
        public string name { get; set; }
        // text of sensor, not exitent by null
        public string text { get; set; }

        // is 1 by rectangle shape, not exitent by ellipse
        public int rectangular { get; set; }
        // AXIS A, not existent by 50
        public double a { get; set; }
        // AXIS B, not existent by 50
        public double b { get; set; }
        // ANGLE not existent by 0
        public double angle { get; set; }

        //Timer is 1 by Timeout, not existent by Countdown
        public int interruptable { get; set; }
        // Timeout Min, not exitent by 0
        public double timeoutMin { get; set; }
        // Timeout Min, not existent by 0
        public double timeoutMid { get; set; }
        // Timeout Max, not existent by 0
        public double timeoutMax { get; set; }

        // sets the trigger type, is not existent by none
        // EAST G (guard by oppfor)
        public string type { get; set; }

        // sets the activation by, is not existent by none
        public string activationBy { get; set; }
        
        // Repeading Trigger, ist not exitent by "once"
        public int repeating { get; set; }

        // Sets the Activation type, is not existent  by PRESENT
        public string activationType { get; set; }
        // Extra conditions, not existent by "this"
        public string expCond { get; set; }
        // On Activation Script, not existent by null
        public string expActiv { get; set; }
        // On Deactivation Script, not existent by null
        public string expDesactiv { get; set; }
        // Effects
        public Effects effects { get; set; }
    }
}
