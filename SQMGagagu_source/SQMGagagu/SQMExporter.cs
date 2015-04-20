using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Arma2Net.AddInProxy;
using System.Globalization;

using SQMGagagu.sqmfile;
using SQMGagagu.sqmfile.datatypes;

using System.Reflection;

namespace SQMGagagu
{
    [AddIn("SQMExporter", Version = "0.1.0.0", Publisher = "Gagagu", Description = "Addin to export object data to sqm file.")]
    public class SQMExporter : MethodAddIn
    {
        private Exception lastException;
        private SqmFile sqmfile;
        private string filename = "mission";
        private string filenameext = ".sqm";

        private string armaprofilepath="";
        private string missionname="";
        private string worldname = "";
        private string profilename = "";
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        #region "external"

        #region "main sqm data"
        /// <summary>
        /// Constructor
        /// </summary>
        public SQMExporter()
        {
        }

        /// <summary>
        /// gets the filename of mission file. mostely mission.sqm
        /// </summary>
        /// <returns></returns>
        public string GetFilename()
        {
            try
            {
                return filename;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return "";
            }
        }

        /// <summary>
        /// sets the filename. Only needed when filename should not be mission.sqm
        /// </summary>
        /// <param name="filename"></param>
        public bool SetFilename(string filename)
        {
            try
            {
                this.filename = filename;
                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Gets the last thrown exeption string
        /// </summary>
        /// <returns></returns>
        public string GetLastException()
        {
            try
            {
                if (lastException != null)
                    return lastException.Message;
                else
                    return "";
            }
            catch {
                return "";
            }
        }

        /// <summary>
        /// Sets the version numer of sqm file
        /// </summary>
        /// <param name="Version">number of version as string</param>
        /// <returns>false on error</returns>
        public bool SetVersion(String Version)
        {
            try
            {
                lastException = null;

                if (sqmfile == null)
                    return false;

                if (String.IsNullOrEmpty(Version))
                    return false;

                sqmfile.version = Version;
                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        #endregion

        #region "Export"

        /// <summary>
        /// Init Export but first import all data from mission file.
        /// </summary>
        /// <param name="WorldName">Name of Map (e.g. startis or altis</param>
        /// <param name="MissionName">Name of mission</param>
        /// <param name="ProfileName">arma user profile name</param>
        /// <returns></returns>
        public bool InitExportFirstImport(string WorldName,
                               string MissionName,
                               string ProfileName)
        {
            try
            {
                lastException = null;
                SQMImporter inp = new SQMImporter();
                if (inp.Import(WorldName, MissionName, ProfileName))
                {
                    this.sqmfile = inp.GetSqmFileData();
                    if (this.sqmfile != null)
                    {
                        return InitExport(WorldName, null, MissionName, ProfileName);
                    }
                    else
                    {
                        lastException = new Exception("Imported sqm file is empty.");
                        return false;
                    }
                }
                else
                {
                    this.lastException = new Exception(inp.GetLastException());
                    return false;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //InitExportFirstImport

        /// <summary>
        /// Init to export mission
        /// </summary>
        /// <param name="WorldName">Name of Map (e.g. startis or altis</param>
        /// <param name="MissionName">Name of mission</param>
        /// <param name="ProfileName">arma user profile name</param>
        /// <returns></returns>
        public bool InitExport(string WorldName,
                       string MissionName,
                       string ProfileName)
        {
            try{
                lastException = null;
                return InitExport(WorldName,null,MissionName,ProfileName);
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Init to export mission with possibillity to set some defautl values (only fo testing)
        /// </summary>
        /// <param name="WorldName"></param>
        /// <param name="WorldAddOnName"></param>
        /// <param name="MissionName"></param>
        /// <param name="ProfileName"></param>
        /// <returns></returns>
        public bool InitExport(string WorldName,
                               string WorldAddOnName,
                               string MissionName, 
                               string ProfileName)
        {
            try
            {
                lastException = null;
                this.missionname = MissionName;
                this.profilename = ProfileName;
                this.worldname = WorldName;

                // check profile directory
                armaprofilepath = ArmaProfile.CheckProfileDirectory(profilename);
                if (string.IsNullOrEmpty(armaprofilepath))
                {
                    this.lastException = ArmaProfile.GetLastException();
                    return false;
                }

                // create mission folder
                if (!ArmaProfile.CreateMissionFolder(armaprofilepath,missionname,worldname))
                    return false;

                // init class with default values
                if (!String.IsNullOrEmpty(WorldAddOnName))
                {
                    if (!InitClassWithDefaults(WorldAddOnName))
                        return false;
                }
                else {
                    sqmfile = new SqmFile();
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Writes the mission file to disk
        /// </summary>
        /// <param name="CreateBakFile">if exists mission file should it backup first?</param>
        /// <returns></returns>
        public bool FinishExport(bool CreateBakFile)
        {
            try
            {
                string missionfolder = Path.Combine(armaprofilepath, this.missionname + "." + this.worldname);
                string missionfilename =Path.Combine(missionfolder,this.filename);

                // check class
                if (sqmfile == null)
                {
                    lastException = new Exception("SQMFile Class is empty. Export canceled.");
                    return false;
                }

                // check mission folder
                if (!Directory.Exists(missionfolder))
                {
                    lastException = new DirectoryNotFoundException("Could not find mission folder");
                    return false;
                }

                // if mission file exists delete?
                if(File.Exists(missionfilename + filenameext))
                {
                    if (CreateBakFile)
                    {
                        if (File.Exists(missionfilename + filenameext))
                        {
                            File.Delete(missionfilename + ".bak");
                        }

                        File.Move(missionfilename + filenameext, missionfilename + ".bak");
                    }
                    else
                    {
                        File.Delete(missionfilename + filenameext);
                    }
                }

                using (StreamWriter file= new StreamWriter(missionfilename + filenameext))
                {
                    try
                    {
                        file.Write(sqmfile.ToClassString());
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        return false;
                    }
                }

                return true;
            }            
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }
        #endregion

        #region "Main Class Data"

        #region "AddOns"
        /// <summary>
        /// Clears the List of used addons in file
        /// </summary>
        /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
        /// <returns>false on error</returns>
        public bool ClearAddOns(String ClassName)
        {
            try
            {
                lastException = null;

                if (sqmfile == null)
                    return false;

                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        sqmfile.mission.addOns.Clear();
                        break;
                    case "INTRO":
                        sqmfile.intro.addOns.Clear();
                        break;
                    case "OUTROWIN":
                        sqmfile.outrowin.addOns.Clear();
                        break;
                    case "OUTROLOOSE":
                        sqmfile.outroloose.addOns.Clear();
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //ClearAddOns

        /// <summary>
        /// Add an Item to AddOns List
        /// </summary>
        /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
        /// <param name="AddOnName">AddOn Name as string</param>
        /// <returns>false on error</returns>
        public bool SetAddOns(String ClassName, String AddOnName)
        {
            try{
                lastException = null;

                if (sqmfile == null)
                    return false;

                if (String.IsNullOrEmpty(AddOnName))
                    return false;

                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        sqmfile.mission.addOns.Add(AddOnName);
                        break;
                    case "INTRO":
                        sqmfile.intro.addOns.Add(AddOnName);
                        break;
                    case "OUTROWIN":
                        sqmfile.outrowin.addOns.Add(AddOnName);
                        break;
                    case "OUTROLOOSE":
                        sqmfile.outroloose.addOns.Add(AddOnName);
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetAddOns

   

        #endregion

        #region "AddOnsAuto"
        /// <summary>
        /// Clears the List of used addonsauto in file
        /// </summary>
        /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
        /// <returns>false on error</returns>
        public bool ClearAddOnsAuto(String ClassName)
        {
            try
            {
                lastException = null;

                if (sqmfile == null)
                    return false;

                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        sqmfile.mission.addOnsAuto.Clear();
                        break;
                    case "INTRO":
                        sqmfile.intro.addOnsAuto.Clear();
                        break;
                    case "OUTROWIN":
                        sqmfile.outrowin.addOnsAuto.Clear();
                        break;
                    case "OUTROLOOSE":
                        sqmfile.outroloose.addOnsAuto.Clear();
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //ClearAddOnsAuto

        /// <summary>
        /// Add an Item to AddOnsAuto List
        /// </summary>
        /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
        /// <param name="AddOnName">AddOnAuto Name as string</param>
        /// <returns>false on error</returns>
        public bool SetAddOnsAuto(String ClassName, String AddOnName)
        {
            try
            {
                lastException = null;

                if (sqmfile == null)
                    return false;

                if (String.IsNullOrEmpty(AddOnName))
                    return false;

                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        sqmfile.mission.addOnsAuto.Add(AddOnName);
                        break;
                    case "INTRO":
                        sqmfile.intro.addOnsAuto.Add(AddOnName);
                        break;
                    case "OUTROWIN":
                        sqmfile.outrowin.addOnsAuto.Add(AddOnName);
                        break;
                    case "OUTROLOOSE":
                        sqmfile.outroloose.addOnsAuto.Add(AddOnName);
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetAddOnsAuto

   

        #endregion

        #region "RandomSeed"
        /// <summary>
        /// Sets the random seed value
        /// </summary>
        /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
        /// <param name="RandomSeed">Random Seed value as string</param>
        /// <returns>false on error</returns>
        public bool SetRandomSeed(String ClassName, String RandomSeed)
        {
            try
            {
                lastException = null;

                if (sqmfile == null)
                    return false;

                if (String.IsNullOrEmpty(RandomSeed))
                    return false;

                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        sqmfile.mission.randomSeed=RandomSeed;
                        break;
                    case "INTRO":
                        sqmfile.intro.randomSeed = RandomSeed;
                        break;
                    case "OUTROWIN":
                        sqmfile.outrowin.randomSeed = RandomSeed;
                        break;
                    case "OUTROLOOSE":
                        sqmfile.outroloose.randomSeed = RandomSeed;
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } // SetRandomSeed

        /// <summary>
        /// Creates a automatically a random seed value
        /// </summary>
        /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
        /// <returns>false on error</returns>
        public bool CreateRandomSeed(String ClassName)
        {
            try
            {
                Random rnd = new Random();
                String RandomSeed = rnd.Next(0000000, 9999999).ToString("D7"); 

                lastException = null;

                if (sqmfile == null)
                    return false;

                if (String.IsNullOrEmpty(RandomSeed))
                    return false;

                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        sqmfile.mission.randomSeed = RandomSeed;
                        break;
                    case "INTRO":
                        sqmfile.intro.randomSeed = RandomSeed;
                        break;
                    case "OUTROWIN":
                        sqmfile.outrowin.randomSeed = RandomSeed;
                        break;
                    case "OUTROLOOSE":
                        sqmfile.outroloose.randomSeed = RandomSeed;
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } // CreateRandomSeed

        #endregion

        #endregion

        #region "Set Intel Data"
        /// <summary>
        /// Sets a value from given parameter
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="ParameterName">Name of parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetIntelDataValueByName(String Classname, String ParameterName, String Value)
        {
            try
            {
                lastException = null;

                if ((sqmfile == null)
                    || (String.IsNullOrEmpty(Classname))
                    || (String.IsNullOrEmpty(ParameterName))
                    || (String.IsNullOrEmpty(Value)))
                    return false;

                Intel intel = Helpers.GetIntel(Classname, sqmfile, lastException);
                if (intel == null)
                    return false;

                PropertyInfo inf = intel.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(intel, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(intel, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(intel, val);
                    else
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        } //SetIntelDataValueByName

        #endregion

        #region "groups"

        /// <summary>
        /// Clear all items in groups class
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <returns>false on error</returns>
        public bool DeleteAllGroups(String Classname)
        {
            try
            {
                // check class
                if (sqmfile == null)
                {
                    lastException = new Exception("SQMFile Class is empty. Export canceled.");
                    return false;
                }

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                groups.DeleteAll();

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //DeleteAllGroups


        /// <summary>
        /// Adds a new empty group item
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="Side">WEST,EAST,GUER,CIV,LOGIC</param>
        /// <returns>group id, -1 on error</returns>
        public int AddNewGroup(String Classname, String Side)
        {
            try
            {
                lastException = null;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return -1;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return -1;

                Groups_Item item= new Groups_Item();
                item.side = Side;
                groups.AddItem(item);
                
                return groups.GetItemCount() - 1;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return -1;
            }
        } //AddNewGroup

        #region "vehicles"
        /// <summary>
        /// Deletes all vehicle in group
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <returns>false on error</returns>
        public bool DeteteAllVehiclesInGroupByID(String Classname, String GroupID)
        {
            try {
                lastException = null;

                int id = -1;
                if (!int.TryParse(GroupID, out id))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                Groups_Item gItem = groups.GetItemByID(id);
                if (gItem == null)
                    return false;

                gItem.vehicles.DeleteAll();

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Add new empty vehicle to specific group
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <returns>vehicle id, -1 on error</returns>
        public int AddNewVehicleToGroupByID(String Classname, String GroupID)
        {
            try
            {
                lastException = null;

                int id = -1;
                if (!int.TryParse(GroupID,out id))
                    return -1;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return -1;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return -1;

                if (groups.GetItemCount() <=0)
                    return -1;

                Groups_Item gItem = groups.GetItemByID(id);
                if (gItem == null)
                    return -1;

                Vehicles_Item vItem = new Vehicles_Item();
                gItem.vehicles.AddItem(vItem);

                return gItem.vehicles.GetItemCount() - 1;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return -1;
            }
        } //AddNewVehicleToGroupByID

        /// <summary>
        /// Sets a value from given parameter
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <param name="VehicleID">id of vehicle</param>
        /// <param name="ParameterName">Name of parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetGroupVehicleValueByName(String Classname, String GroupID, String VehicleID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int gid = -1;
                int vid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(GroupID))
                     || (String.IsNullOrEmpty(VehicleID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;

                if (!int.TryParse(GroupID, out gid))
                    return false;

                if (!int.TryParse(VehicleID, out vid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                if (groups.GetItemCount() <= 0)
                    return false;

                Groups_Item gItem = groups.GetItemByID(gid);
                if (gItem == null)
                    return false;

                Vehicles_Item vItem = gItem.vehicles.GetItemByID(vid);
                if (vItem == null)
                    return false;

                PropertyInfo inf = vItem.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(vItem, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(vItem, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(vItem, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetGroupVehicleValueByName

        /// <summary>
        /// Sets the position for vehicle
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <param name="VehicleID">id of vehicle</param>
        /// <param name="X">x position</param>
        /// <param name="Y">y position</param>
        /// <param name="Z">z position</param>
        /// <returns>false on error</returns>
        public bool SetGroupVehiclePosition(String Classname, String GroupID, String VehicleID, String X, String Y, String Z)
        {
           try
            {
                lastException = null;
                int gid = -1;
                int vid = -1;
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;


                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(GroupID))
                     || (String.IsNullOrEmpty(VehicleID))
                     || (String.IsNullOrEmpty(X))
                     || (String.IsNullOrEmpty(Y))
                     || (String.IsNullOrEmpty(Z)))
                    return false;

                if (!int.TryParse(GroupID, out gid))
                    return false;

                if (!int.TryParse(VehicleID, out vid))
                    return false;

                if (!double.TryParse(X, NumberStyles.Number,culture,out x))
                    return false;

                if (!double.TryParse(Y, NumberStyles.Number, culture, out y))
                    return false;

                if (!double.TryParse(Z, NumberStyles.Number, culture, out z))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                if (groups.GetItemCount() <= 0)
                    return false;

                Groups_Item gItem = groups.GetItemByID(gid);
                if (gItem == null)
                    return false;

                Vehicles_Item vItem = gItem.vehicles.GetItemByID(vid);
                if (vItem == null)
                    return false;

                SqmPosition pos = new SqmPosition(x,y,z);

                vItem.position=pos;

                return true;

            }
           catch (Exception ex)
           {
               lastException = ex;
               return false;
           }
        }

        #endregion

        #region "waypoints"
        /// <summary>
        /// Deletes all waypoints in group
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <returns>false on error</returns>
        public bool DeteteAllWaypointsInGroupByID(String Classname, String GroupID)
        {
            try
            {
                lastException = null;

                int id = -1;
                if (!int.TryParse(GroupID, out id))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                Groups_Item gItem = groups.GetItemByID(id);
                if (gItem == null)
                    return false;

                gItem.waypoints.DeleteAll();

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Add new empty waypoint to specific group
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <returns>vehicle id, -1 on error</returns>
        public int AddNewWaypointToGroupByID(String Classname, String GroupID)
        {
            try
            {
                lastException = null;

                int id = -1;
                if (!int.TryParse(GroupID, out id))
                    return -1;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return -1;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return -1;

                if (groups.GetItemCount() <= 0)
                    return -1;

                Groups_Item gItem = groups.GetItemByID(id);
                if (gItem == null)
                    return -1;

                Waypoint_Item wItem = new Waypoint_Item();
                gItem.waypoints.AddItem(wItem);

                return gItem.waypoints.GetItemCount() - 1;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return -1;
            }
        } //AddNewVehicleToGroupByID

        /// <summary>
        /// Sets a value from given parameter name
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <param name="WaypointID">id of vehicle</param>
        /// <param name="ParameterName">Name of parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetGroupWaypointValueByName(String Classname, String GroupID, String WaypointID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int gid = -1;
                int vid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(GroupID))
                     || (String.IsNullOrEmpty(WaypointID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;

                if (!int.TryParse(GroupID, out gid))
                    return false;

                if (!int.TryParse(WaypointID, out vid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                if (groups.GetItemCount() <= 0)
                    return false;

                Groups_Item gItem = groups.GetItemByID(gid);
                if (gItem == null)
                    return false;

                Waypoint_Item wItem = gItem.waypoints.GetItemByID(vid);
                if (wItem == null)
                    return false;

                PropertyInfo inf = wItem.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(wItem, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(wItem, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(wItem, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetGroupVehicleValueByName

        /// <summary>
        /// Sets the position for waypoint
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <param name="WaypointID">id of vehicle</param>
        /// <param name="X">x position</param>
        /// <param name="Y">y position</param>
        /// <param name="Z">z position</param>
        /// <returns>false on error</returns>
        public bool SetGroupWaypointPosition(String Classname, String GroupID, String WaypointID, String X, String Y, String Z)
        {
            try
            {
                lastException = null;
                int gid = -1;
                int vid = -1;
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;


                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(GroupID))
                     || (String.IsNullOrEmpty(WaypointID))
                     || (String.IsNullOrEmpty(X))
                     || (String.IsNullOrEmpty(Y))
                     || (String.IsNullOrEmpty(Z)))
                    return false;

                if (!int.TryParse(GroupID, out gid))
                    return false;

                if (!int.TryParse(WaypointID, out vid))
                    return false;

                if (!double.TryParse(X, NumberStyles.Number, culture, out x))
                    return false;

                if (!double.TryParse(Y, NumberStyles.Number, culture, out y))
                    return false;

                if (!double.TryParse(Z, NumberStyles.Number, culture, out z))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                if (groups.GetItemCount() <= 0)
                    return false;

                Groups_Item gItem = groups.GetItemByID(gid);
                if (gItem == null)
                    return false;

                Waypoint_Item wItem = gItem.waypoints.GetItemByID(vid);
                if (wItem == null)
                    return false;

                SqmPosition pos = new SqmPosition(x, y, z);

                wItem.position = pos;

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Sets a value from given effect parameter name
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="GroupID">id of group</param>
        /// <param name="WaypointID">id of waypoint</param>
        /// <param name="ParameterName">Name of effect parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetGroupWaypointEffectValueByName(String Classname, String GroupID, String WaypointID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int gid = -1;
                int vid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(GroupID))
                     || (String.IsNullOrEmpty(WaypointID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;

                if (!int.TryParse(GroupID, out gid))
                    return false;

                if (!int.TryParse(WaypointID, out vid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                if (groups == null)
                    return false;

                if (groups.GetItemCount() <= 0)
                    return false;

                Groups_Item gItem = groups.GetItemByID(gid);
                if (gItem == null)
                    return false;

                Waypoint_Item wItem = gItem.waypoints.GetItemByID(vid);
                if (wItem == null)
                    return false;


                if (wItem.effects == null)
                    wItem.effects = new Effects();

                PropertyInfo inf = wItem.effects.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(wItem.effects, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(wItem.effects, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(wItem.effects, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetGroupWaypointEffectValueByName

        #endregion

        #endregion

        #region "Vehicles"

        /// <summary>
        ///  Detele all Items im vehicle class
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <returns>false on error+</returns>
        public bool DeleteAllVehicles(String Classname)
        {
            try
            {
                // check class
                if (sqmfile == null)
                {
                    lastException = new Exception("SQMFile Class is empty. Export canceled.");
                    return false;
                }

                Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                if (vehicles == null)
                    return false;

                vehicles.DeleteAll();

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //DeleteAllVehicles

        /// <summary>
        /// Add new empty Vehicle
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="Side">AMBIENT LIFE or EMPTY</param>
        /// <returns>vehicle id, -1 on error</returns>
        public int AddNewVehicle(String Classname, String Side)
        {
            try
            {
                lastException = null;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return -1;

                Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                if (vehicles == null)
                    return -1;

                Vehicles_Item item = new Vehicles_Item();
                item.side = Side;
                vehicles.AddItem(item);

                return vehicles.GetItemCount() - 1;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return -1;
            }
        }

        /// <summary>
        /// Sets position of vehicle
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="VehicleID">vehicle id</param>
        /// <param name="X">x position</param>
        /// <param name="Y">y position</param>
        /// <param name="Z">z position</param>
        /// <returns>false on error</returns>
        public bool SetVehiclePosition(String Classname, String VehicleID, String X, String Y, String Z)
        {
            try
            {
                lastException = null;
                int vid = -1;
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;


                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(VehicleID))
                     || (String.IsNullOrEmpty(X))
                     || (String.IsNullOrEmpty(Y))
                     || (String.IsNullOrEmpty(Z)))
                    return false;


                if (!int.TryParse(VehicleID, out vid))
                    return false;

                if (!double.TryParse(X, NumberStyles.Number, culture, out x))
                    return false;

                if (!double.TryParse(Y, NumberStyles.Number, culture, out y))
                    return false;

                if (!double.TryParse(Z, NumberStyles.Number, culture, out z))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                if (vehicles == null)
                    return false;

                if (vehicles.GetItemCount() <= 0)
                    return false;

                Vehicles_Item vItem = vehicles.GetItemByID(vid);
                if (vItem == null)
                    return false;

                SqmPosition pos = new SqmPosition(x, y, z);

                vItem.position = pos;

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } // SetVehiclePosition

        /// <summary>
        /// Sets a value from given parameter name
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="VehicleID">vehicle id</param>
        /// <param name="ParameterName">Name of effect parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetVehicleValueByName(String Classname, String VehicleID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int vid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(VehicleID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;

                if (!int.TryParse(VehicleID, out vid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                if (vehicles == null)
                    return false;

                if (vehicles.GetItemCount() <= 0)
                    return false;

                Vehicles_Item vItem = vehicles.GetItemByID(vid);
                if (vItem == null)
                    return false;

                PropertyInfo inf = vItem.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(vItem, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(vItem, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(vItem, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetVehicleValueByName

        #endregion

        #region "Markers"

        /// <summary>
        ///  Detele all Items in markers class
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <returns>false on error</returns>
        public bool DeleteAllMarkers(String Classname)
        {
            try
            {
                // check class
                if (sqmfile == null)
                {
                    lastException = new Exception("SQMFile Class is empty. Export canceled.");
                    return false;
                }

                Markers markers = Helpers.GetMarkers(Classname, sqmfile, lastException);
                if (markers == null)
                    return false;

                markers.DeleteAll();

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //DeleteAllMarkers

        /// <summary>
        /// Add new empty Marker
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <returns>marker id, -1 on error</returns>
        public int AddNewMarker(String Classname)
        {
            try
            {
                lastException = null;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return -1;

                Markers markers = Helpers.GetMarkers(Classname, sqmfile, lastException);
                if (markers == null)
                    return -1;

                Markers_Item item = new Markers_Item();
                markers.AddItem(item);

                return markers.GetItemCount() - 1;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return -1;
            }
        }

        /// <summary>
        /// Sets position of marker
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="VehicleID">marker id</param>
        /// <param name="X">x position</param>
        /// <param name="Y">y position</param>
        /// <param name="Z">z position</param>
        /// <returns>false on error</returns>
        public bool SetMarkerPosition(String Classname, String MarkerID, String X, String Y, String Z)
        {
            try
            {
                lastException = null;
                int mid = -1;
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;


                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(MarkerID))
                     || (String.IsNullOrEmpty(X))
                     || (String.IsNullOrEmpty(Y))
                     || (String.IsNullOrEmpty(Z)))
                    return false;


                if (!int.TryParse(MarkerID, out mid))
                    return false;

                if (!double.TryParse(X, NumberStyles.Number, culture, out x))
                    return false;

                if (!double.TryParse(Y, NumberStyles.Number, culture, out y))
                    return false;

                if (!double.TryParse(Z, NumberStyles.Number, culture, out z))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Markers markers = Helpers.GetMarkers(Classname, sqmfile, lastException);
                if (markers == null)
                    return false;

                if (markers.GetItemCount() <= 0)
                    return false;

                Markers_Item mItem = markers.GetItemByID(mid);
                if (mItem == null)
                    return false;

                SqmPosition pos = new SqmPosition(x, y, z);

                mItem.position = pos;

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } // SetMarkerPosition

        /// <summary>
        /// Sets a value from given parameter name
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="VehicleID">marker id</param>
        /// <param name="ParameterName">Name of parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetMarkerValueByName(String Classname, String MarkerID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int mid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(MarkerID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;

                if (!int.TryParse(MarkerID, out mid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Markers markers = Helpers.GetMarkers(Classname, sqmfile, lastException);
                if (markers == null)
                    return false;

                if (markers.GetItemCount() <= 0)
                    return false;

                Markers_Item mItem = markers.GetItemByID(mid);
                if (mItem == null)
                    return false;

                PropertyInfo inf = mItem.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(mItem, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(mItem, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(mItem, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetMarkerValueByName

        #endregion

        #region "Sensors"

        /// <summary>
        ///  Detele all Items in seonsors class
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <returns>false on error</returns>
        public bool DeleteAllSensors(String Classname)
        {
            try
            {
                // check class
                if (sqmfile == null)
                {
                    lastException = new Exception("SQMFile Class is empty. Export canceled.");
                    return false;
                }

                Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                if (sensors == null)
                    return false;

                sensors.DeleteAll();

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //DeleteAllSensors

        /// <summary>
        /// Add new empty Sensor
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <returns>marker id, -1 on error</returns>
        public int AddNewSensor(String Classname)
        {
            try
            {
                lastException = null;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return -1;

                Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                if (sensors == null)
                    return -1;

                Sensors_Item item = new Sensors_Item();
                sensors.AddItem(item);

                return sensors.GetItemCount() - 1;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return -1;
            }
        } //AddNewSensor

        /// <summary>
        /// Sets position of Sensor
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="VehicleID">sensor id</param>
        /// <param name="X">x position</param>
        /// <param name="Y">y position</param>
        /// <param name="Z">z position</param>
        /// <returns>false on error</returns>
        public bool SetSensorPosition(String Classname, String SensorID, String X, String Y, String Z)
        {
            try
            {
                lastException = null;
                int sid = -1;
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;


                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(SensorID))
                     || (String.IsNullOrEmpty(X))
                     || (String.IsNullOrEmpty(Y))
                     || (String.IsNullOrEmpty(Z)))
                    return false;


                if (!int.TryParse(SensorID, out sid))
                    return false;

                if (!double.TryParse(X, NumberStyles.Number, culture, out x))
                    return false;

                if (!double.TryParse(Y, NumberStyles.Number, culture, out y))
                    return false;

                if (!double.TryParse(Z, NumberStyles.Number, culture, out z))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                if (sensors == null)
                    return false;

                if (sensors.GetItemCount() <= 0)
                    return false;

                Sensors_Item sItem = sensors.GetItemByID(sid);
                if (sItem == null)
                    return false;

                SqmPosition pos = new SqmPosition(x, y, z);

                sItem.position = pos;

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } // SetSensorPosition

        /// <summary>
        /// Sets a value from given parameter name
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="VehicleID">sensor id</param>
        /// <param name="ParameterName">Name of parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetSensorValueByName(String Classname, String SensorID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int sid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(SensorID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;

                if (!int.TryParse(SensorID, out sid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                if (sensors == null)
                    return false;

                if (sensors.GetItemCount() <= 0)
                    return false;

                Sensors_Item sItem = sensors.GetItemByID(sid);
                if (sItem == null)
                    return false;

                PropertyInfo inf = sItem.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(sItem, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(sItem, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(sItem, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetSensorValueByName


        /// <summary>
        /// Sets a value from given sensor effect parameter name
        /// </summary>
        /// <param name="Classname">mission,into,outrowin,outroloose</param>
        /// <param name="SensorID">id of sensor</param>
        /// <param name="ParameterName">Name of effect parameter</param>
        /// <param name="Value">value for this parameter</param>
        /// <returns>false  on error</returns>
        public bool SetSensorEffectValueByName(String Classname,  String SensorID, String ParameterName, String Value)
        {
            try
            {
                lastException = null;
                int sid = -1;

                if ((sqmfile == null)
                     || (String.IsNullOrEmpty(Classname))
                     || (String.IsNullOrEmpty(SensorID))
                     || (String.IsNullOrEmpty(ParameterName))
                     || (String.IsNullOrEmpty(Value)))
                    return false;


                if (!int.TryParse(SensorID, out sid))
                    return false;

                if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                    return false;

                Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                if (sensors == null)
                    return false;

                if (sensors.GetItemCount() <= 0)
                    return false;

                Sensors_Item sItem = sensors.GetItemByID(sid);
                if (sItem == null)
                    return false;


                if (sItem.effects == null)
                    sItem.effects = new Effects();

                PropertyInfo inf = sItem.effects.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return false;

                if ((inf.PropertyType == typeof(System.String)) || (inf.PropertyType == typeof(string)))
                    inf.SetValue(sItem.effects, Value);
                else if ((inf.PropertyType == typeof(System.Int32)) || (inf.PropertyType == typeof(int)))
                {
                    int val = 0;
                    if (int.TryParse(Value, out val))
                        inf.SetValue(sItem.effects, val);
                    else
                        return false;
                }
                else if ((inf.PropertyType == typeof(System.Double)) || (inf.PropertyType == typeof(double)))
                {
                    double val = 0;
                    if (double.TryParse(Value, NumberStyles.Number, culture, out val))
                        inf.SetValue(sItem.effects, val);
                    else
                        return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //SetSensorEffectValueByName


        #endregion

        #endregion

        #region "internal"

        #region "defaults"
        /// <summary>
        /// Inits the class with default values.
        /// </summary>
        /// <param name="MissionName"></param>
        /// <param name="WorldAddOnName"></param>
        /// <returns></returns>
        private bool InitClassWithDefaults(string WorldAddOnName)
        {

            try
            {
                Random rnd = new Random();
                sqmfile = new SqmFile();

                var mission = sqmfile.mission;
                var intro = sqmfile.intro;
                var outrowin = sqmfile.outrowin;
                var outroloose = sqmfile.outroloose;


                // ****************************************************
                // Fill with default values
                // ****************************************************
                
                // Version
                sqmfile.version = "12"; // the version of the file "12" seems to be the actual value

                // ****************************************************
                // **************** class Mission *********************
                // ****************************************************

                // Addons
                mission.addOns.Add(WorldAddOnName);
                mission.addOnsAuto.Add(WorldAddOnName);
                // Random seed
                mission.randomSeed = rnd.Next(0000000, 9999999).ToString("D7"); // (string) seems to be a random value
                // Intel
                mission.intel.briefingName = this.missionname;                      // (string) the name of the mission
                mission.intel.overviewText = "";                                    // (string) the mission description
                mission.intel.startWeather = 0.28999999;                            // (double: 0.0 - 1.0) overcast start value
                mission.intel.startWind = 0.099999994;                              // (double: 0.0 - 1.0) wind start value
                mission.intel.startWaves = 0.099999994;                             // (double: 0.0 - 1.0) start waves value
                mission.intel.forecastWeather = 0.29999998;                         // (double: 0.0 - 1.0) overcast forecast value
                mission.intel.forecastWind = 0.099999994;                           // (double: 0.0 - 1.0) wind forecast value
                mission.intel.forecastWaves = 0.099999994;                          // (double: 0.0 - 1.0) forecast waves (always needed)
                mission.intel.forecastLightnings = 0.099999994;                     // (double: 0.0 - 1.0) forecast lightning value
                mission.intel.year = 2035;                                          // (int) the year value (self explained)
                mission.intel.month = 7;                                            // (int: 1-12) the month value
                mission.intel.day = 6;                                              // (int: 1-31) the day value
                mission.intel.hour = 12;                                            // (int: 1-24) the hour value
                mission.intel.minute = 0;                                           // (int: 0-59) the minute value
                mission.intel.startFogDecay = 0.013;                                // (double: 0.0 - 1.0) fog decay start value
                mission.intel.forecastFogDecay = 0.013;                             // (double: 0.0 - 1.0) fog decay forecast value

                // ****************************************************
                // **************** class Intro *********************
                // ****************************************************

                // Addons
                intro.addOns.Add(WorldAddOnName);
                intro.addOnsAuto.Add(WorldAddOnName);
                // Random seed
                intro.randomSeed = rnd.Next(0000000, 9999999).ToString("D7"); // (string) seems to be a random value
                // Intel
                intro.intel.briefingName = this.missionname;                      // (string) the name of the mission
                intro.intel.overviewText = "";                                    // (string) the mission description
                intro.intel.startWeather = 0.30000001;                            // (double: 0.0 - 1.0) overcast start value
                intro.intel.startWind = 0.1;                                      // (double: 0.0 - 1.0) wind start value
                intro.intel.startWaves = 0.1;                                     // (double: 0.0 - 1.0) start waves value
                intro.intel.forecastWeather = 0.30000001;                         // (double: 0.0 - 1.0) overcast forecast value
                intro.intel.forecastWind = 0.1;                                   // (double: 0.0 - 1.0) wind forecast value
                intro.intel.forecastWaves = 0.1;                                  // (double: 0.0 - 1.0) forecast waves (always needed)
                intro.intel.forecastLightnings = 0.1;                             // (double: 0.0 - 1.0) forecast lightning value
                intro.intel.year = 2035;                                          // (int) the year value (self explained)
                intro.intel.month = 7;                                            // (int: 1-12) the month value
                intro.intel.day = 6;                                              // (int: 1-31) the day value
                intro.intel.hour = 12;                                            // (int: 1-24) the hour value
                intro.intel.minute = 0;                                           // (int: 0-59) the minute value
                intro.intel.startFogDecay = 0.013;                                // (double: 0.0 - 1.0) fog decay start value
                intro.intel.forecastFogDecay = 0.013;                             // (double: 0.0 - 1.0) fog decay forecast value

                // ****************************************************
                // **************** class outrowin *********************
                // ****************************************************

                // Addons
                outrowin.addOns.Add(WorldAddOnName);
                outrowin.addOnsAuto.Add(WorldAddOnName);
                // Random seed
                outrowin.randomSeed = rnd.Next(0000000, 9999999).ToString("D7"); // (string) seems to be a random value
                // Intel
                outrowin.intel.briefingName = this.missionname;                      // (string) the name of the mission
                outrowin.intel.overviewText = "";                                    // (string) the mission description
                outrowin.intel.startWeather = 0.30000001;                            // (double: 0.0 - 1.0) overcast start value
                outrowin.intel.startWind = 0.1;                                      // (double: 0.0 - 1.0) wind start value
                outrowin.intel.startWaves = 0.1;                                     // (double: 0.0 - 1.0) start waves value
                outrowin.intel.forecastWeather = 0.30000001;                         // (double: 0.0 - 1.0) overcast forecast value
                outrowin.intel.forecastWind = 0.1;                                   // (double: 0.0 - 1.0) wind forecast value
                outrowin.intel.forecastWaves = 0.1;                                  // (double: 0.0 - 1.0) forecast waves (always needed)
                outrowin.intel.forecastLightnings = 0.1;                             // (double: 0.0 - 1.0) forecast lightning value
                outrowin.intel.year = 2035;                                          // (int) the year value (self explained)
                outrowin.intel.month = 7;                                            // (int: 1-12) the month value
                outrowin.intel.day = 6;                                              // (int: 1-31) the day value
                outrowin.intel.hour = 12;                                            // (int: 1-24) the hour value
                outrowin.intel.minute = 0;                                           // (int: 0-59) the minute value
                outrowin.intel.startFogDecay = 0.013;                                // (double: 0.0 - 1.0) fog decay start value
                outrowin.intel.forecastFogDecay = 0.013;                             // (double: 0.0 - 1.0) fog decay forecast value

                // ****************************************************
                // **************** class outroloose *********************
                // ****************************************************

                // Addons
                outroloose.addOns.Add(WorldAddOnName);
                outroloose.addOnsAuto.Add(WorldAddOnName);
                // Random seed
                outroloose.randomSeed = rnd.Next(0000000, 9999999).ToString("D7"); // (string) seems to be a random value
                // Intel
                outroloose.intel.briefingName = this.missionname;                       // (string) the name of the mission
                outroloose.intel.overviewText = "";                                    // (string) the mission description
                outroloose.intel.startWeather = 0.30000001;                            // (double: 0.0 - 1.0) overcast start value
                outroloose.intel.startWind = 0.1;                                      // (double: 0.0 - 1.0) wind start value
                outroloose.intel.startWaves = 0.1;                                     // (double: 0.0 - 1.0) start waves value
                outroloose.intel.forecastWeather = 0.30000001;                         // (double: 0.0 - 1.0) overcast forecast value
                outroloose.intel.forecastWind = 0.1;                                   // (double: 0.0 - 1.0) wind forecast value
                outroloose.intel.forecastWaves = 0.1;                                  // (double: 0.0 - 1.0) forecast waves (always needed)
                outroloose.intel.forecastLightnings = 0.1;                             // (double: 0.0 - 1.0) forecast lightning value
                outroloose.intel.year = 2035;                                          // (int) the year value (self explained)
                outroloose.intel.month = 7;                                            // (int: 1-12) the month value
                outroloose.intel.day = 6;                                              // (int: 1-31) the day value
                outroloose.intel.hour = 12;                                            // (int: 1-24) the hour value
                outroloose.intel.minute = 0;                                           // (int: 0-59) the minute value
                outroloose.intel.startFogDecay = 0.013;                                // (double: 0.0 - 1.0) fog decay start value
                outroloose.intel.forecastFogDecay = 0.013;                             // (double: 0.0 - 1.0) fog decay forecast value



                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }
        
    #endregion

        #endregion


    } // class
}
