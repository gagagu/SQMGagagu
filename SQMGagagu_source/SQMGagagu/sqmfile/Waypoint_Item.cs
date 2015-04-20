using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{
    public class Waypoint_Item
    {
        public string name { get; set; }

        // position array
        public SqmPosition position { get; set; }

        // Select Type: MOVE,DESTROY,GETIN,SAD,JOIN,LEADER,GETOUT,CYCLE,LOAD,UNLOAD,TR UNLOAD,HOLD,SENTRY,GUARD,TALK,SCRIPTED,SUPPORT,GETIN NEAREST,DISMISS,LOITER, not visible by "MOVE"
        public string type { get; set; }

        // Placement Radius, not visible by 0
        public double placement { get; set; }
        // Completition Radius, not visible by 0
        public double completitionRadius { get; set; }

        // Timeout, not visible by 0
        public double timeoutMin { get; set; }
        public double timeoutMid { get; set; }
        public double timeoutMax { get; set; }

        // name of waypoint, not visible by null
        public string text { get; set; }

        // description of waypoint, not visible by null
        public string description { get; set; }

        // combat mode: "NO CHANGE","BLUE" (never fire), "GREEN" (hold fire), "WHITE" (hold fire engage at will), "YELLOW" (open fire), "RED" (open fire, engage at will), not visible by "No CHANGE"
        public string combatMode { get; set; }

        // NO CHANGE, COLUMN, STAG COLUMN,WEDGE,ECH LEFT,ECH RIGHT,VEE,LINE,DIAMOND,FILE, not visible by "No CHANGE"
        public string formation { get; set; }

        // NO CHANGE, LIMITED, NORMAL, FÙLL, not visible by "No CHANGE"
        public string speed { get; set; }

        // behavior: NO CHANGE, CARELESS,SAFE,AWARE,COMBAT,STEALTH, not visible by "No CHANGE"
        public string combat { get; set; }

        // Extra conditions, not existent by "true"
        public string expCond { get; set; }

        // On Activation Script, not existent by null
        public string expActiv { get; set; }

        // Script, not existent by null
        public string script { get; set; }

        // Show waypoint: NEVER, CADET, ALWAYS, not shown by CADET
        public string showWP { get; set; }

        //Effects
        public Effects effects { get; set; }
    }


}
