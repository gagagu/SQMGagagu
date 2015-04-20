using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQMGagagu.sqmfile.datatypes;

namespace SQMGagagu.sqmfile
{
    public class Markers
    {
        // contains all Vehicle class items
        private List<Markers_Item> ItemsList;


        /// <summary>
        /// constructor
        /// create new list
        /// </summary>
        public Markers()
        {
            ItemsList = new List<Markers_Item>();
        }

        public Markers_Item GetItemByID(int ItemID)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="markerType"></param>
        /// <param name="type"></param>
        /// <param name="colorName"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        public void AddItem(SqmPosition position,
                        string name ,
                        string text ,
                        string markerType,
                        string type ,
                        string colorName ,
                        double a ,
                        double b ,
                        double angle)
        {
            Markers_Item item = new Markers_Item();
            item.position = position;
            item.name = name;
            item.text = text;
            item.markerType = markerType;
            item.type = type;
            item.colorName = colorName;
            item.a = a;
            item.b = b;
            item.angle = angle;
            ItemsList.Add(item);
        }
        
        public void DeleteAll()
        {
            ItemsList.Clear();
        }

        public void AddItem(Markers_Item item)
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

            retval.AppendLine(tabul + "class Markers");
            retval.AppendLine(tabul + "{");

            retval.AppendLine(tabul + "\titems=" + ItemsList.Count().ToString() + ";");

            int x = 0;
            foreach (Markers_Item item in ItemsList)
            {
                retval.AppendLine(tabul + "\tclass Item" + x.ToString());
                retval.AppendLine(tabul + "\t{");
                retval.AppendLine(tabul + "\t\tposition[]={" + item.position.X.ToString().Replace(",", ".") + "," + item.position.Z.ToString().Replace(",", ".") + "," + item.position.Y.ToString().Replace(",", ".") + "};");
                retval.AppendLine(tabul + "\t\tname=\"" + item.name + "\";");

                if (!string.IsNullOrEmpty(item.text))
                    retval.AppendLine(tabul + "\t\ttext=\"" + item.text + "\";");

                if ((!string.IsNullOrEmpty(item.markerType))&&(item.markerType.ToUpper()!="ICON"))
                    retval.AppendLine(tabul + "\t\tmarkerType=\"" + item.markerType + "\";");

                if (!string.IsNullOrEmpty(item.type))
                    retval.AppendLine(tabul + "\t\ttype=\"" + item.type + "\";");

                if ((!string.IsNullOrEmpty(item.colorName)) && (item.colorName.ToUpper() != "DEFAULT"))
                    retval.AppendLine(tabul + "\t\tcolorName=\"" + item.colorName + "\";");

                retval.AppendLine(tabul + "\t\ta=" + item.a.ToString().Replace(",", ".") + ";");
                retval.AppendLine(tabul + "\t\tb=" + item.b.ToString().Replace(",", ".") + ";");
                
                if (item.angle > 0)
                    retval.AppendLine(tabul + "\t\tangle=" + item.angle.ToString().Replace(",", ".") + ";");

                retval.AppendLine(tabul + "\t};");
                x += 1;
            }

            retval.AppendLine(tabul + "};");
            return retval.ToString();
        }

    }
}
