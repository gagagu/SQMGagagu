using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    public class OutroWin : Mission
    {
        public new string ToClassString()
        {

            StringBuilder retval = new StringBuilder();

            retval.AppendLine("class OutroWin");
            retval.AppendLine("{");
            // addons
            retval.AppendLine("\taddOns[]=");
            retval.AppendLine("\t{");
            foreach (string item in addOns)
            {
                if (item == addOns.Last())
                    retval.AppendLine("\t\t\"" + item + "\"");
                else
                    retval.AppendLine("\t\t\"" + item + "\",");

            }
            retval.AppendLine("\t};");

            // addOnsAuto
            retval.AppendLine("\taddOnsAuto[]=");
            retval.AppendLine("\t{");
            foreach (string item in addOnsAuto)
            {
                if (item == addOnsAuto.Last())
                    retval.AppendLine("\t\t\"" + item + "\"");
                else
                    retval.AppendLine("\t\t\"" + item + "\",");

            }
            retval.AppendLine("\t};");

            //randomSeed
            retval.AppendLine("\trandomSeed=" + randomSeed + ";");

            //Intel
            retval.AppendLine(intel.ToClassString(1));

            //Groups
            retval.AppendLine(groups.ToClassString());

            //vehicles
            retval.AppendLine(vehicles.ToClassString(1));

            //Markers
            retval.AppendLine(markers.ToClassString());

            //Sensors
            retval.AppendLine(sensors.ToClassString());

            // end
            retval.AppendLine("};");

            return retval.ToString();
        }
    
    }
}
