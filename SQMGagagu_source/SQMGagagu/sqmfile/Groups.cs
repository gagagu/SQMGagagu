using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    public class Groups
    {
        // contains all Vehicle class items
        private List<Groups_Item> ItemsList;

        public Groups()
        {
            ItemsList = new List<Groups_Item>();
        }

        public void AddItem(Groups_Item Item)
        {
            ItemsList.Add(Item);
        }

        public void DeleteAll()
        {
            ItemsList.Clear();
        }

        public Groups_Item GetItemByID(int ItemID)
        {
            try
            {
                if (ItemsList.Count() == 0)
                    return null;

                return ItemsList[ItemID];
            }
            catch {
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

        public string ToClassString()
        {
            try
            {
                if (ItemsList.Count == 0)
                    return "";


                StringBuilder retval = new StringBuilder();
                string tabul = "\t";

                retval.AppendLine("\tclass Groups");
                retval.AppendLine("\t{");

                retval.AppendLine("\t\titems=" + ItemsList.Count().ToString() + ";");

                int x = 0;
                foreach (Groups_Item item in ItemsList)
                {
                    retval.AppendLine(tabul + "\tclass Item" + x.ToString());
                    retval.AppendLine(tabul + "\t{");
                    retval.AppendLine(item.ToClassString());
                    retval.AppendLine(tabul + "\t};");
                    x += 1;
                }

                retval.AppendLine("\t};");

                return retval.ToString();
            }
            catch
            {
                return "";
            }
        }

    }
}
