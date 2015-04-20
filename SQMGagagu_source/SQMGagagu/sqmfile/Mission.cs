using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    /// <summary>
    /// Mission class
    /// </summary>
    public class Mission
    {
        // addOns[]
        public List<string> addOns;

        // addOnsAuto[]
        public List<string> addOnsAuto;

        // randomSeed
        public string randomSeed;

        // class Intel
        public Intel intel;

        // class Groups
        public Groups groups;

        // class Vehicles
        public Vehicles vehicles;

        // class Markers
        public Markers markers;

        // class Sensors
        public Sensors sensors;

        /// <summary>
        /// Construktor
        /// </summary>
        public Mission()
        {
            vehicles = new Vehicles();
            groups = new Groups();
            intel = new Intel();
            addOns = new List<string>();
            addOnsAuto = new List<string>();
            markers = new Markers();
            sensors = new Sensors();

            randomSeed = "";
        }

        /// <summary>
        ///  creates the class string for export to file
        /// </summary>
        /// <returns>class string</returns>
        public string ToClassString()
        {
            StringBuilder retval = new StringBuilder();

            retval.AppendLine("class Mission");
            retval.AppendLine("{");
            // addons
            retval.AppendLine("\taddOns[]=");
            retval.AppendLine("\t{");
            foreach (string item in addOns)
            {          
                if(item==addOns.Last())
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
