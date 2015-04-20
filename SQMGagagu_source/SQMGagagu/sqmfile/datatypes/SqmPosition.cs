/*
 *      SQLGagagu created by A.Eckers
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile.datatypes
{
    /// <summary>
    /// contains the position array of any sql mission file item
    /// </summary>
    public class SqmPosition
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public SqmPosition(double x, double z, double y)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override String ToString()
        {
            return this.X.ToString().Replace(",", ".") + "," + this.Z.ToString().Replace(",", ".") + "," + this.Y.ToString().Replace(",", ".");
        }

    }
}
