using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{
    public class Sensors
    {
        // contains all Vehicle class items
        private List<Sensors_Item> ItemsList;
        
        /// <summary>
        /// constructor
        /// create new list
        /// </summary>
        public Sensors()
        {
            ItemsList = new List<Sensors_Item>();
        }

        public Sensors_Item GetItemByID(int ItemID)
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

        public void AddItem(SqmPosition position,
                            string name,
                            string text,
                            int rectangular,
                            double a,
                            double b,
                            double angle,
                            int interruptable,
                            double timeoutMin,
                            double timeoutMid,
                            double timeoutMax,
                            string type,
                            string activationBy, 
                            int repeating,
                            string activationType,
                            string expCond,
                            string expActiv,
                            string expDesactiv,
                            Effects effects)
        {
            Sensors_Item item = new Sensors_Item();
            item.position = position;
            item.name = name;
            item.text = text;
            item.rectangular = rectangular;
            item.a = a;
            item.b = b;
            item.angle = angle;
            item.interruptable = interruptable;
            item.timeoutMin = timeoutMin;
            item.timeoutMid = timeoutMid;
            item.timeoutMax = timeoutMax;
            item.type = type;
            item.activationBy = activationBy;
            item.repeating = repeating;
            item.activationType = activationType;
            item.expCond = expCond;
            item.expActiv = expActiv;
            item.expDesactiv = expDesactiv;
            item.effects = effects;
            ItemsList.Add(item);
        }

        public void DeleteAll()
        {
            ItemsList.Clear();
        }

        public void AddItem(Sensors_Item item)
        {
            ItemsList.Add(item);
        }
        /// <summary>
        /// creates the class Markers string for export to file
        /// </summary>
        /// <returns>markers class string</returns>
        public string ToClassString()
        {
            if (ItemsList.Count == 0)
                return "";

            StringBuilder retval = new StringBuilder();
            string tabul = "\t";

            retval.AppendLine(tabul + "class Sensors");
            retval.AppendLine(tabul + "{");

            retval.AppendLine(tabul + "\titems=" + ItemsList.Count().ToString() + ";");

            int x = 0;
            foreach (Sensors_Item item in ItemsList)
            {
                retval.AppendLine(tabul + "\tclass Item" + x.ToString());
                retval.AppendLine(tabul + "\t{");

                if(item.position!=null)
                    retval.AppendLine(tabul + "\t\tposition[]={" + item.position.X.ToString().Replace(",", ".") + "," + item.position.Z.ToString().Replace(",", ".") + "," + item.position.Y.ToString().Replace(",", ".") + "};");
                
                retval.AppendLine(tabul + "\t\tage=\"UNKNOWN\";");

                if (!string.IsNullOrEmpty(item.name))
                    retval.AppendLine(tabul + "\t\tname=\"" + item.name + "\";");

                if (!string.IsNullOrEmpty(item.text))
                    retval.AppendLine(tabul + "\t\ttext=\"" + item.text + "\";");
                
                if(item.rectangular==1)
                    retval.AppendLine(tabul + "\t\trectangular=\"1\";");

                if (item.a != 50)
                    retval.AppendLine(tabul + "\t\ta=" + item.a.ToString().Replace(",", ".") + ";");

                if (item.b != 50)
                    retval.AppendLine(tabul + "\t\tb=" + item.b.ToString().Replace(",", ".") + ";");

                if (item.angle > 0)
                    retval.AppendLine(tabul + "\t\tangle=" + item.angle.ToString().Replace(",", ".") + ";");

                if (item.interruptable == 1)
                    retval.AppendLine(tabul + "\t\tinterruptable=1;");

                if (item.timeoutMin > 0)
                    retval.AppendLine(tabul + "\t\ttimeoutMin=" + item.timeoutMin.ToString().Replace(",", ".") + ";");

                if (item.timeoutMid > 0)
                    retval.AppendLine(tabul + "\t\ttimeoutMid=" + item.timeoutMid.ToString().Replace(",", ".") + ";");

                if (item.timeoutMax > 0)
                    retval.AppendLine(tabul + "\t\ttimeoutMax=" + item.timeoutMax.ToString().Replace(",", ".") + ";");

                if ((!string.IsNullOrEmpty(item.type)) && (item.type.ToUpper() != "NONE"))
                    retval.AppendLine(tabul + "\t\ttype=\"" + item.type + "\";");

                if ((!string.IsNullOrEmpty(item.activationBy)) && (item.activationBy.ToUpper() != "NONE"))
                    retval.AppendLine(tabul + "\t\tactivationBy=\"" + item.activationBy + "\";");

                if (item.repeating == 1)
                    retval.AppendLine(tabul + "\t\trepeating=\"1\";");

                if ((!string.IsNullOrEmpty(item.activationType)) && (item.activationType.ToUpper() != "PRESENT"))
                    retval.AppendLine(tabul + "\t\tactivationType=\"" + item.activationType + "\";");

                if ((!string.IsNullOrEmpty(item.expCond)) && (item.expCond.ToUpper() != "THIS"))
                    retval.AppendLine(tabul + "\t\texpCond=\"" + item.expCond + "\";");

                if (!string.IsNullOrEmpty(item.expActiv))
                    retval.AppendLine(tabul + "\t\texpActiv=\"" + item.expActiv + "\";");

                if (!string.IsNullOrEmpty(item.expDesactiv))
                    retval.AppendLine(tabul + "\t\texpDesactiv=\"" + item.expDesactiv + "\";");

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
                retval.AppendLine(tabul + "\t\t}");
                // effects

                retval.AppendLine(tabul + "\t};");
                x += 1;
            }

            retval.AppendLine(tabul + "};");
            return retval.ToString();
        }
    }
}
