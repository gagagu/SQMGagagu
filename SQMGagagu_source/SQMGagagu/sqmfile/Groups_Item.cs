using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    public class Groups_Item
    {

        // Which Side (WEST,EAST,GUER,CIV,LOGIC)
        public string side { get; set; }
        public Vehicles vehicles { get; set; }
        public Waypoints waypoints { get; set; }

        public Groups_Item()
        {
            vehicles = new Vehicles();
            waypoints = new Waypoints();
        }

        public string ToClassString()
        {
            
            string vh = vehicles.ToClassString(3);
            if (string.IsNullOrEmpty(vh))
            {
                return "";
            }
            else
            {
                StringBuilder retval = new StringBuilder();
                retval.AppendLine("\t\t\tside=\"" + side + "\";");
                retval.AppendLine(vh);

                string wp = waypoints.ToClassString(3);
                if(!string.IsNullOrEmpty(wp))
                    retval.AppendLine(wp);

                return retval.ToString();
            }
        }

    }
}
