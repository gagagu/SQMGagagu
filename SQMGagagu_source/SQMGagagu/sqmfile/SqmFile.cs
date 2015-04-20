using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    /// <summary>
    /// Class which describes the sqm mission file
    /// </summary>
    public class SqmFile
    {
        public string version { get; set; }

        // class Mission
        public Mission mission;

        // class Intro
        public Intro intro;

        // class OutroWin
        public OutroWin outrowin;

        // class OutroLoose
        public OutroLoose outroloose;

        /// <summary>
        /// Construktor
        /// </summary>
        public SqmFile()
        {
            mission = new Mission();
            intro = new Intro();
            outrowin = new OutroWin();
            outroloose = new OutroLoose();
        }

        /// <summary>
        ///  creates the class string for export to file
        /// </summary>
        /// <returns>class string</returns>
        public string ToClassString()
        {
            StringBuilder retval = new StringBuilder();

            retval.AppendLine("version=" + version +";");
            retval.AppendLine(mission.ToClassString());
            retval.AppendLine(intro.ToClassString());
            retval.AppendLine(outrowin.ToClassString());
            retval.AppendLine(outroloose.ToClassString());

            return retval.ToString();
        }
    }
}
