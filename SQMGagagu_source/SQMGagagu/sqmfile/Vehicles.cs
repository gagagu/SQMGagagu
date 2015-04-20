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
    /// Vehicle class
    /// Exists inside Group class and as main class in mission file
    /// </summary>
    public class Vehicles
    {
        // contains all Vehicle class items
        private List<Vehicles_Item> ItemsList;

        /// <summary>
        /// constructor
        /// create new list
        /// </summary>
        public Vehicles()
        {
            ItemsList = new List<Vehicles_Item>();
        }

        public Vehicles_Item GetItemByID(int ItemID)
        {
            try
            {
                if (ItemsList.Count() == 0)
                    return null;

                return ItemsList[ItemID];
            }
            catch
            {
                return null;
            }
        }

        public int GetItemCount()
        {
            try
            {
                if (ItemsList != null)
                    return ItemsList.Count();
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Adds a new item to Vehicle class list
        /// </summary>
        /// <param name="id">id of item</param>
        /// <param name="vehicle">vehicle class name</param>
        /// <param name="side">sets the side of the item, values: WEST,EAST,GUER,CIV,LOGIC,AMBIENT LIFE,EMPTY</param>
        /// <param name="position">position array</param>
        /// <param name="placement">placement radius</param>
        /// <param name="azimut">rotational radius 0 - 365</param>
        /// <param name="offsetY">offset in Y direction</param>
        /// <param name="special">Special flag of item, values: NONE, CARGO, FLY, IN FORMATION</param>
        /// <param name="player">Control flag, values: PLAY CDG, PLAYER COMMANDER, NON PLAYABLE</param>
        /// <param name="str_lock">Itel locked flag, values: UNLOCKED, LOCKED, LOCKEDPLAYER,DEFAULT</param>
        /// <param name="rank">rank of item, values: CORPORAL,SERGEANT,LIEUTENANT,CAPTAIN,MAJOR,COLONEL,PRIVATE</param>
        /// <param name="age">info age of item, values: ACTUAL, 5 MIN,10 MIN,15 MIN,30 MIN,60 MIN,120 MIN, UNKNOWN</param>
        /// <param name="text">item name, could be empty</param>
        /// <param name="init">init code of item, could be empty</param>
        /// <param name="description">item description, could be empty</param>
        /// <param name="skill">0.0 - 1.0 skill of item</param>
        /// <param name="health">0.0 - 1.0 health of item</param>
        /// <param name="fuel">0.0 - 1.0 fuel of item</param>
        /// <param name="ammo">0.0 - 1.0 ammo of item</param>
        /// <param name="presence">0.0 - 1.0 presence of item</param>
        /// <param name="presenceCondition">presence condition code</param>
        /// <param name="leader">leader of group 0 or 1</param>
        public void AddItem(int id,
                            string vehicle,
                            string side,
                            SqmPosition position,
                            double placement,
                            double azimut,
                            double offsetY,
                            string special,
                            string player,
                            string str_lock,
                            string rank,
                            string age,
                            string text,
                            string init,
                            string description,
                            double skill,
                            double health,
                            double fuel,
                            double ammo,
                            double presence,
                            string presenceCondition,
                            int leader
                        )
        {
            Vehicles_Item item = new Vehicles_Item();
            item.id = id;
            item.vehicle = vehicle;
            item.side = side;
            item.position = position;
            item.placement = placement;
            item.azimut = azimut;
            item.offsetY = offsetY;
            item.special = special;
            item.player = player;
            item.str_lock = str_lock;
            item.rank = rank;
            item.age = age;
            item.text = text;
            item.init = init;
            item.description = description;
            item.skill = skill;
            item.health = health;
            item.fuel = fuel;
            item.ammo = ammo;
            item.presence = presence;
            item.presenceCondition = presenceCondition;
            item.leader = leader;
            ItemsList.Add(item);
        }

        public void DeleteAll()
        {
            ItemsList.Clear();
        }

        public void AddItem(Vehicles_Item item)
        {
            ItemsList.Add(item);
        }

        /// <summary>
        /// creates the class Vehicles string for export to file
        /// </summary>
        /// <returns>vehicle class string</returns>
        public string ToClassString(int tabulators)
        {
            if (ItemsList.Count == 0)
                return "";

            StringBuilder retval = new StringBuilder();

            string tabul = "";
            for (int x = 0; x < tabulators; x++)
            {
                tabul += "\t";
            }


            retval.AppendLine(tabul + "class Vehicles");
            retval.AppendLine(tabul + "{");

            retval.AppendLine(tabul + "\titems=" + ItemsList.Count().ToString() + ";");

            int y = 0;
            foreach (Vehicles_Item item in ItemsList)
            {
                retval.AppendLine(tabul + "\tclass Item" + y.ToString());
                retval.AppendLine(tabul + "\t{");

                retval.AppendLine(tabul + "\t\tid=" + item.id.ToString() + ";");
                retval.AppendLine(tabul + "\t\tvehicle=\"" + item.vehicle + "\";");
                retval.AppendLine(tabul + "\t\tside=\"" + item.side + "\";");
                if(item.position!=null)
                    retval.AppendLine(tabul + "\t\tposition[]={" + item.position.X.ToString().Replace(",", ".") + "," + item.position.Z.ToString().Replace(",", ".") + "," + item.position.Y.ToString().Replace(",", ".") + "};");

                if(item.placement>0)
                    retval.AppendLine(tabul + "\t\tplacement=" + item.placement.ToString().Replace(",", ".") + ";");

                if (item.azimut > 0)
                    retval.AppendLine(tabul + "\t\tazimut=" + item.azimut.ToString().Replace(",", ".") + ";");

                if (item.offsetY > 0)
                    retval.AppendLine(tabul + "\t\toffsetY=" + item.offsetY.ToString().Replace(",", ".") + ";");

                if (!string.IsNullOrEmpty(item.special) && (!item.special.ToUpper().Contains("IN FORMATION")))
                    retval.AppendLine(tabul + "\t\tspecial=\"" + item.special.ToUpper() + "\";");

                if (!string.IsNullOrEmpty(item.player) && (!item.player.ToUpper().Contains("NON PLAYABLE")))
                    retval.AppendLine(tabul + "\t\tplayer=\"" + item.player.ToUpper() + "\";");

                if (!string.IsNullOrEmpty(item.str_lock) && (!item.str_lock.ToUpper().Contains("DEFAULT")))
                    retval.AppendLine(tabul + "\t\tlock=\"" + item.str_lock.ToUpper() + "\";");

                if (!string.IsNullOrEmpty(item.rank) && (!item.rank.ToUpper().Contains("PRIVATE")))
                    retval.AppendLine(tabul + "\t\trank=\"" + item.rank.ToUpper() + "\";");

                if (!string.IsNullOrEmpty(item.age) && (!item.age.ToUpper().Contains("UNKNOWN")))
                    retval.AppendLine(tabul + "\t\tage=\"" + item.age.ToUpper() + "\";");

                if (!string.IsNullOrEmpty(item.text))
                    retval.AppendLine(tabul + "\t\ttext=\"" + item.text + "\";");

                if (!string.IsNullOrEmpty(item.init))
                    retval.AppendLine(tabul + "\t\tinit=\"" + item.init + "\";");

                if (!string.IsNullOrEmpty(item.description))
                    retval.AppendLine(tabul + "\t\tdescription=\"" + item.description + "\";");

                retval.AppendLine(tabul + "\t\tskill=" + item.skill.ToString().Replace(",", ".") + ";");

                if (item.health < 1)
                    retval.AppendLine(tabul + "\t\thealth=" + item.health.ToString().Replace(",", ".") + ";");

                if (item.fuel < 1)
                    retval.AppendLine(tabul + "\t\tfuel=" + item.fuel.ToString().Replace(",", ".") + ";");

                if (item.ammo < 1)
                    retval.AppendLine(tabul + "\t\tammo=" + item.ammo.ToString().Replace(",", ".") + ";");

                if (item.presence < 1)
                    retval.AppendLine(tabul + "\t\tpresence=" + item.presence.ToString().Replace(",", ".") + ";");

                if ((!string.IsNullOrEmpty(item.presenceCondition))&& (item.presenceCondition.ToUpper()!="TRUE"))
                    retval.AppendLine(tabul + "\t\tpresenceCondition=\"" + item.presenceCondition + "\";");

                if(item.leader>0)
                    retval.AppendLine(tabul + "\t\tleader=" + item.leader.ToString() + ";");

                retval.AppendLine(tabul + "\t};");
                y += 1;
            }

            retval.AppendLine(tabul + "};");

            return retval.ToString();
        }

    }
}
