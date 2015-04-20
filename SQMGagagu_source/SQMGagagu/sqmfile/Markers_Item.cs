using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{
    public class Markers_Item
    {
        // position array
        public SqmPosition position { get; set; }
        // Name of marker
        public string name { get; set; }
        // text of marker, not exitent by null
        public string text { get; set; }

        // RECTANGLE, ELLIPSE, not existent by ICON
        public string markerType { get; set; }
        
        // only existent by markerType=ICON (name of icon) default: Empty
        public string type { get; set; }
        
        // name of Color ("ColorBrown"), not existent by default
        public string colorName { get; set; }
        
        // AXIS A
        public double a { get; set; }
        
        // AXIS B
        public double b { get; set; }
        
        // ANGLE not existent by 0
        public double angle { get; set; }
    }
}
