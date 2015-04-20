using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{
    public class Waypoints
    {
        // contains all Vehicle class items
        private List<Waypoint_Item> ItemsList;

        /// <summary>
        /// constructor
        /// create new list
        /// </summary>
        public Waypoints()
        {
            ItemsList = new List<Waypoint_Item>();
        }

        public Waypoint_Item GetItemByID(int ItemID)
        {
            try
            {
                if (ItemID >= ItemsList.Count())
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

        public void AddItem(string name,
                            SqmPosition position,
                            string type,
                            double placement ,
                            double completitionRadius,
                            double timeoutMin,
                            double timeoutMid,
                            double timeoutMax,
                            string text,
                            string description,
                            string combatMode,
                            string formation,
                            string speed,
                            string combat,
                            string expCond,
                            string expActiv,
                            string script,
                            string showWP,
                            Effects effects)
        {
            Waypoint_Item item = new Waypoint_Item();
            item.name = name;
            item.position = position;
            item.type = type;
            item.placement = placement;
            item.completitionRadius = completitionRadius;
            item.timeoutMin = timeoutMin;
            item.timeoutMid = timeoutMid;
            item.timeoutMax = timeoutMax;
            item.text = text;
            item.description = description;
            item.combatMode = combatMode;
            item.formation = formation;
            item.speed = speed;
            item.combat = combat;
            item.expCond = expCond;
            item.expActiv = expActiv;
            item.script = script;
            item.showWP = showWP;
            item.effects = effects;
           
            ItemsList.Add(item);
        }

        public void DeleteAll()
        {
            ItemsList.Clear();
        }


        public void AddItem(Waypoint_Item item)
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


            retval.AppendLine(tabul + "class Waypoints");
            retval.AppendLine(tabul + "{");

            retval.AppendLine(tabul + "\titems=" + ItemsList.Count().ToString() + ";");

            int y = 0;
            foreach (Waypoint_Item item in ItemsList)
            {
                retval.AppendLine(tabul + "\tclass Item" + y.ToString());
                retval.AppendLine(tabul + "\t{");

                if(item.position!=null)
                    retval.AppendLine(tabul + "\t\tposition[]={" + item.position.X.ToString().Replace(",", ".") + "," + item.position.Z.ToString().Replace(",", ".") + "," + item.position.Y.ToString().Replace(",", ".") + "};");

                if ((!string.IsNullOrEmpty(item.type)) && (item.type.ToUpper() != "MOVE"))
                    retval.AppendLine(tabul + "\t\ttype=\"" + item.type + "\";");

                if (item.placement > 0)
                    retval.AppendLine(tabul + "\t\tplacement=" + item.placement.ToString().Replace(",", ".") + ";");

                if (item.completitionRadius > 0)
                    retval.AppendLine(tabul + "\t\tcompletitionRadius=" + item.completitionRadius.ToString().Replace(",", ".") + ";");

                if (item.timeoutMin > 0)
                    retval.AppendLine(tabul + "\t\ttimeoutMin=" + item.timeoutMin.ToString().Replace(",", ".") + ";");

                if (item.timeoutMid > 0)
                    retval.AppendLine(tabul + "\t\ttimeoutMid=" + item.timeoutMid.ToString().Replace(",", ".") + ";");

                if (item.timeoutMax > 0)
                    retval.AppendLine(tabul + "\t\ttimeoutMax=" + item.timeoutMax.ToString().Replace(",", ".") + ";");

                if (!string.IsNullOrEmpty(item.name))
                    retval.AppendLine(tabul + "\t\tname=\"" + item.name + "\";");

                if (!string.IsNullOrEmpty(item.text))
                    retval.AppendLine(tabul + "\t\ttext=\"" + item.text + "\";");

                if (!string.IsNullOrEmpty(item.description))
                    retval.AppendLine(tabul + "\t\tdescription=\"" + item.description + "\";");

                if ((!string.IsNullOrEmpty(item.combatMode)) && (item.combatMode.ToUpper() != "NO CHANGE"))
                    retval.AppendLine(tabul + "\t\tcombatMode=\"" + item.combatMode + "\";");

                if ((!string.IsNullOrEmpty(item.formation)) && (item.formation.ToUpper() != "NO CHANGE"))
                    retval.AppendLine(tabul + "\t\tformation=\"" + item.formation + "\";");

                if ((!string.IsNullOrEmpty(item.speed)) && (item.speed.ToUpper() != "NO CHANGE"))
                    retval.AppendLine(tabul + "\t\tspeed=\"" + item.speed + "\";");

                if ((!string.IsNullOrEmpty(item.combat)) && (item.combat.ToUpper() != "NO CHANGE"))
                    retval.AppendLine(tabul + "\t\tcombat=\"" + item.combat + "\";");

                if ((!string.IsNullOrEmpty(item.expCond)) && (item.expCond.ToUpper() != "TRUE"))
                    retval.AppendLine(tabul + "\t\texpCond=\"" + item.expCond + "\";");

                if (!string.IsNullOrEmpty(item.expActiv))
                    retval.AppendLine(tabul + "\t\texpActiv=\"" + item.expActiv + "\";");

                if (!string.IsNullOrEmpty(item.script))
                    retval.AppendLine(tabul + "\t\tscript=\"" + item.script + "\";");

                if ((!string.IsNullOrEmpty(item.showWP)) && (item.showWP.ToUpper() != "CADET"))
                    retval.AppendLine(tabul + "\t\tshowWP=\"" + item.showWP + "\";");

                // effects
                retval.AppendLine(tabul + "\t\tclass Effects");
                retval.AppendLine(tabul + "\t\t{");
                if (item.effects != null)
                {
                    if ((!string.IsNullOrEmpty(item.effects.condition)) && (item.effects.condition.ToUpper() != "TRUE"))
                        retval.AppendLine(tabul + "\t\t\tcondition=\"" + item.effects.condition + "\";");

                    if (!string.IsNullOrEmpty(item.effects.sound))
                        retval.AppendLine(tabul + "\t\t\tsound=\"" + item.effects.sound + "\";");

                    if (!string.IsNullOrEmpty(item.effects.voice))
                        retval.AppendLine(tabul + "\t\t\tvoice=\"" + item.effects.voice + "\";");

                    if (!string.IsNullOrEmpty(item.effects.soundEnv))
                        retval.AppendLine(tabul + "\t\t\tsoundEnv=\"" + item.effects.soundEnv + "\";");

                    if (!string.IsNullOrEmpty(item.effects.soundDet))
                        retval.AppendLine(tabul + "\t\t\tsoundDet=\"" + item.effects.soundDet + "\";");

                    if (!string.IsNullOrEmpty(item.effects.track))
                        retval.AppendLine(tabul + "\t\t\ttrack=\"" + item.effects.track + "\";");

                    if ((!string.IsNullOrEmpty(item.effects.titleEffect)) && (item.effects.titleEffect.ToUpper() != "PLAIN"))
                        retval.AppendLine(tabul + "\t\t\ttitleEffect=\"" + item.effects.titleEffect + "\";");

                    if ((!string.IsNullOrEmpty(item.effects.titleType)) && (item.effects.titleType.ToUpper() != "NONE"))
                        retval.AppendLine(tabul + "\t\t\ttitleType=\"" + item.effects.titleType + "\";");

                    if ((!string.IsNullOrEmpty(item.effects.title)) && (item.effects.title.ToUpper() != "NONE"))
                        retval.AppendLine(tabul + "\t\t\ttitle=\"" + item.effects.title + "\";");
                }
                retval.AppendLine(tabul + "\t\t};");
                // effects


                retval.AppendLine(tabul + "\t};");
                y += 1;
            }

            retval.AppendLine(tabul + "};");

            return retval.ToString();
        }
    }
}
