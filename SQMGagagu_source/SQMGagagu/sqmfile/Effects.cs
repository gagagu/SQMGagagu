using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    public class Effects
    {
        // sets the condition of the effect, not existent by true
        public string condition {get; set;}
        // set the anonymous sound effect
        public string sound { get; set; }
        // set the voice sound effect
        public string voice { get; set; }
        // set the environment sound effect
        public string soundEnv { get; set; }
        // set the trigger sound effect
        public string soundDet { get; set; }
        // set the Track sound effect
        public string track { get; set; }

        // set the special effect, not existent by PLAIN
        public string titleEffect { get; set; }

        // Sets the Type (OBJECT,RES,TEXT), non Existent by NONE
        public string titleType { get; set; }
        
        // The Text of the Title Type, not existent by titleType=NONE
        public string title { get; set; }
    }
}
