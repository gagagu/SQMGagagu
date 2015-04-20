/*
 *      SQLGagagu created by A.Eckers
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{

    /// <summary>
    /// Item description class form sql file
    /// </summary>
    public class Vehicles_Item
    {
        // object id
        public int id { get; set; }

        // object class name
        public string vehicle { get; set; }

        // WEST,EAST,GUER,CIV,LOGIC,AMBIENT LIFE,EMPTY
        public string side { get; set; }

        // position array
        public SqmPosition position { get; set; }

        // placement radius, not existent by 0
        public double placement { get; set; }

        // direction radius 0 - 365
        public double azimut { get; set; }

        // offset in y direction
        public double offsetY { get; set; }

        // NONE, CARGO, FLY, not existent by IN FORMATION
        public string special { get; set; }

        // PLAY CDG, PLAYER COMMANDER, not existent by NON PLAYABLE
        public string player { get; set; }

        // UNLOCKED, LOCKED, LOCKEDPLAYER, not existent by DEFAULT
        public string str_lock {get; set;}

        // CORPORAL,SERGEANT,LIEUTENANT,CAPTAIN,MAJOR,COLONEL, not existent by PRIVATE
        public string rank { get; set; }

        // ACTUAL, 5 MIN,10 MIN,15 MIN,30 MIN,60 MIN,120 MIN, not existent by UNKNOWN
        public string age { get; set; }

        // Name of object, could be empty
        public string text { get; set; }

        // Initialisation code of object, could be empty
        public string init { get; set; }

        // description text of object, could be empty
        public string description { get; set; }

        // 0.0 ... 1.0  the skill of the object
        public double skill { get; set; }

        // 0.0 ... 1.0  the health of the object, not existent by 1
        public double health { get; set; }

        // 0.0 ... 1.0  the fuel of the object, not existent by 1
        public double fuel { get; set; }

        // 0.0 ... 1.0  the ammo of the object, not existent by 1
        public double ammo { get; set; }

        // 0.0 ... 1.0  the presence of the object, not existent by 1
        public double presence { get; set; }

        // condition of presence, not existent by true
        public string presenceCondition { get; set; }

        // leader of item group
        public int leader { get; set; }
    }
}
