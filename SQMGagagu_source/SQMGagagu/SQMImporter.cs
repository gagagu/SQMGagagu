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
    [AddIn("SQMImporter", Version = "0.1.0.0", Publisher = "Gagagu", Description = "Addin to import object data from sqm file.")]
    public class SQMImporter : MethodAddIn
    {
        private Exception lastException;
        private string strVersion = "";
        private string strMission = "";
        private string strIntro = "";
        private string strOutroWin = "";
        private string strOutroLoose = "";
        private SqmFile sqmfile;
        private enum ClassNames { mission, intro, outrowin, outroloose };
        CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        #region "external"
            #region "import"

            /// <summary>
            /// clear all memory
            /// </summary>
            /// <returns>true/false</returns>
            public bool FinishImport()
            {
                try {       
                    strVersion = "";
                    strMission = "";
                    strIntro = "";
                    strOutroWin = "";
                    strOutroLoose = "";
                    sqmfile=null;
                    lastException=null;

                    return true;
                }
                catch (Exception ex)
                {
                    this.lastException = ex;
                    return false;
                }
            }

            /// <summary>
            /// Imports sqm file to class structure
            /// </summary>
            /// <param name="WorldName">name of world (e.g. stratis or altis</param>
            /// <param name="MissionName">name of mission</param>
            /// <param name="ProfileName">name of user profile (from arma not windows)</param>
            /// <returns>true/false</returns>
            public bool Import(string WorldName,
                           string MissionName,
                           string ProfileName)
            {
                try
                {
                    lastException = null;
                    string filename = "";


                    // check profile directory
                    string armaprofilepath = ArmaProfile.CheckProfileDirectory(ProfileName);
                    if (string.IsNullOrEmpty(armaprofilepath))
                    {
                        this.lastException = ArmaProfile.GetLastException();
                        return false;
                    }

                    filename = ArmaProfile.GetMissionFilename(armaprofilepath, MissionName, WorldName);


                    if (string.IsNullOrEmpty(filename))
                    {
                        this.lastException = new Exception("Could not determine filename of mission.");
                        return false;
                    }


                    return Import(filename);
                }
                catch (Exception ex)
                {
                    this.lastException = ex;
                    return false;
                }
            }

            /// <summary>
            /// Imports sqm file to class structure
            /// </summary>
            /// <param name="filename">directory and filename</param>
            /// <returns>true/false</returns>
            public bool Import(string filename)
            {
                try
                {
                    lastException = null;
                    strVersion = "";
                    strMission = "";
                    strIntro = "";
                    strOutroWin = "";
                    strOutroLoose = "";

                    string fc = File.ReadAllText(filename, Encoding.UTF8);

                    if (fc.Length <= 0)
                        return false;
                
                    if (!CutMainParts(fc))
                       return false;

                    // init class
                    sqmfile = new SqmFile();

                    // process version number
                    if (!ProcessVersion())
                    {
                       return false;
                    }
    
                    // mission class
                    if (!ProcessClass(ClassNames.mission, strMission))
                    {
                        return false;
                    }

                    // intro class
                    if (!ProcessClass(ClassNames.intro, strIntro))
                    {
                        return false;
                    }

                    // OutroWin class
                    if (!ProcessClass(ClassNames.outrowin, strOutroWin))
                    {
                        return false;
                    }

                    // OutroLoose class
                    if (!ProcessClass(ClassNames.outroloose, strOutroLoose))
                    {
                        return false;
                    }


                    return true;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return false;
                }

            }//import

            #endregion

            #region "get main sqm Data"
            
            /// <summary>
            ///  Get the complete importet sqm class data
            /// </summary>
            /// <returns>sqm class data</returns>
            public SqmFile GetSqmFileData()
            {
                try{
                    return sqmfile;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            }

            /// <summary>
            /// Get version number of sqm file
            /// </summary>
            /// <returns>string with version number</returns>
            public String GetVersion()
            {
                try
                {
                    lastException = null;

                    if ((sqmfile != null) && (!String.IsNullOrEmpty(sqmfile.version)))
                        return sqmfile.version;
                    else
                        return "";
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return "";
                }
            } //GetVersion
            #endregion

            #region "get Main Class Data"
            /// <summary>
            /// Gets the list of used addons in file
            /// </summary>
            /// <param name="ClassName">Mission, Into, OutroWin,OutroLoose</param>
            /// <returns>IList of Strings</returns>
            public IList<String> GetAddOns(String ClassName)
            {
                try
                {
                    lastException = null;

                    if (sqmfile == null)
                        return null;

                    switch (ClassName.ToUpper())
                    {
                        case "MISSION":
                            return sqmfile.mission.addOns;
                        case "INTRO":
                            return sqmfile.intro.addOns;
                        case "OUTROWIN":
                            return sqmfile.outrowin.addOns;
                        case "OUTROLOOSE":
                            return sqmfile.outroloose.addOns;
                        default:
                            return null;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } // GetAddOns

            /// <summary>
            /// Gets the list of used addOnsAuto in file
            /// </summary>
            /// <param name="ClassName">Mission,Into,OutroWin,OutroLoose</param>
            /// <returns>IList of Strings</returns>
            public IList<String> GetAddOnsAuto(String ClassName)
            {
                try
                {
                    lastException = null;

                    if (sqmfile == null)
                        return null;

                    switch (ClassName.ToUpper())
                    {
                        case "MISSION":
                            return sqmfile.mission.addOnsAuto;
                        case "INTRO":
                            return sqmfile.intro.addOnsAuto;
                        case "OUTROWIN":
                            return sqmfile.outrowin.addOnsAuto;
                        case "OUTROLOOSE":
                            return sqmfile.outroloose.addOnsAuto;
                        default:
                            return null;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } // GetAddOns

            /// <summary>
            /// Gets the Random Seed out of specified class
            /// </summary>
            /// <param name="ClassName">Mission,Into,OutroWin,OutroLoose</param>
            /// <returns>String with random seed value</returns>
            public String GetRandomSeed(String ClassName)
            {
                try
                {
                    lastException = null;

                    if (sqmfile == null)
                        return "";

                    switch (ClassName.ToUpper())
                    {
                        case "MISSION":
                            return sqmfile.mission.randomSeed;
                        case "INTRO":
                            return sqmfile.intro.randomSeed;
                        case "OUTROWIN":
                            return sqmfile.outrowin.randomSeed;
                        case "OUTROLOOSE":
                            return sqmfile.outroloose.randomSeed;
                        default:
                            return "";
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return "";
                }
            } // GetAddOns
            #endregion

            #region "get Intel Data"
            /// <summary>
            /// Returns the complete intell class as IList contains strings.
            /// You could get in trouble if strings are too long!
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <returns></returns>
            public IList<String> GetIntelDataAsStringList(String Classname)
            {
                try
                {
                    lastException = null;

                    IList<String> data = new List<String>();

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Intel intel = Helpers.GetIntel(Classname,sqmfile,lastException);
                    if (intel == null)
                        return null;

                    data.Add(intel.briefingName);
                    data.Add(intel.overviewText);
                    data.Add(intel.year.ToString(culture));
                    data.Add(intel.month.ToString(culture));
                    data.Add(intel.day.ToString(culture));
                    data.Add(intel.hour.ToString(culture));
                    data.Add(intel.minute.ToString(culture));
                    data.Add(intel.startWeather.ToString(culture));
                    data.Add(intel.forecastWeather.ToString(culture));
                    data.Add(intel.startFog.ToString(culture));
                    data.Add(intel.forecastFog.ToString(culture));
                    data.Add(intel.startFogDecay.ToString(culture));
                    data.Add(intel.forecastFogDecay.ToString(culture));
                    data.Add(intel.rainForced.ToString(culture));
                    data.Add(intel.startRain.ToString(culture));
                    data.Add(intel.forecastRain.ToString(culture));
                    data.Add(intel.lightningsForced.ToString(culture));
                    data.Add(intel.startLightnings.ToString(culture));
                    data.Add(intel.forecastLightnings.ToString(culture));
                    data.Add(intel.wavesForced.ToString(culture));
                    data.Add(intel.startWaves.ToString(culture));
                    data.Add(intel.forecastWaves.ToString(culture));
                    data.Add(intel.startWind.ToString(culture));
                    data.Add(intel.forecastWind.ToString(culture));
                    data.Add(intel.windForced.ToString(culture));
                    data.Add(intel.startGust.ToString(culture));
                    data.Add(intel.forecastGust.ToString(culture));
                    data.Add(intel.startWindDir.ToString(culture));
                    data.Add(intel.forecastWindDir.ToString(culture));
                    data.Add(intel.timeOfChanges.ToString(culture));

                    data.Add(intel.IndFriendlyTo);

                    return data;
                }
                catch {
                    return null;
                }
            }

            public String GetIntelValueByName(String Classname, String ParameterName)
            {
                try
                {
                    lastException = null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)) || (String.IsNullOrEmpty(ParameterName)))
                        return "";

                    Intel intel = Helpers.GetIntel(Classname, sqmfile, lastException);
                    if (intel == null)
                        return "";

                    PropertyInfo inf = intel.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(intel);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else
                        return value.ToString().Replace(",",".");
                }
                catch
                {
                    return "";
                }
            }
            
 
            #endregion

            #region "groups"

            #region "main"
            /// <summary>
            /// Get the count of Group Items in Groups class of specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <returns>count of groups</returns>
            public int GetGroupsCount(String Classname)
            {
                try
                {
                    lastException = null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return 0;

                    Groups groups = Helpers.GetGroups(Classname,sqmfile,lastException);
                    if (groups != null)
                        return groups.GetItemCount();
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return 0;
                }
            } //GetGroupsCount

            /// <summary>
            /// Gets the Side of specified group
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <returns>side of group</returns>
            public String GetGroupSideByID(String Classname, String GroupID)
            {
                try
                {
                    lastException = null;

                    int id = -1;
                    if (!int.TryParse(GroupID, out id))
                        return "";

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Groups groups = Helpers.GetGroups(Classname,sqmfile,lastException);
                    if (groups == null)
                        return "";

                    if (id > groups.GetItemCount()-1)
                        return "";

                    Groups_Item item = groups.GetItemByID(id);
                    if (item == null)
                        return "";

                    return item.side;

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return "";
                }
            }
            #endregion

            #region "vehicles"
            /// <summary>
            /// Get the count of Vehicle Items in  specified Groups class of specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <returns>count of vehicles</returns>
            public int GetVehiclesCountFromGroupItem(String Classname, String GroupID)
            {
                try
                {
                    lastException = null;

                    int id = -1;
                    if (!int.TryParse(GroupID, out id))
                        return 0;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return 0;

                    Groups groups = Helpers.GetGroups(Classname,sqmfile,lastException);
                    if (groups == null)
                        return 0;

                    if (id > groups.GetItemCount()-1)
                        return 0;

                    Groups_Item item = groups.GetItemByID(id);
                    if (item == null)
                        return 0;

                    if (item.vehicles == null)
                        return 0;

                    return item.vehicles.GetItemCount();

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return 0;
                }
            }

            /// <summary>
            /// Gets a specified vehicle item as iList from a specified group in specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of vehicle (begins with 0)</param>
            /// <returns>vehicle item as IList of strings</returns>
            public IList<String> GetVehicleItemFromGroupByIDAsStringList(String Classname, String GroupID, String VehicleId)
            {

                try
                {
                    lastException = null;

                    int gid = -1;
                    if (!int.TryParse(GroupID, out gid))
                        return null;

                    int vid = -1;
                    if (!int.TryParse(VehicleId, out vid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Groups groups = Helpers.GetGroups(Classname,sqmfile,lastException);
                    if (groups == null)
                        return null;

                    if (gid > groups.GetItemCount()-1)
                        return null;

                    Groups_Item item = groups.GetItemByID(gid);
                    if (item == null)
                        return null;

                    Vehicles vehicles = item.vehicles;

                    if (vid > vehicles.GetItemCount()-1)
                        return null;

                    Vehicles_Item vItem = vehicles.GetItemByID(vid);
                    if (vItem == null)
                        return null;

                    return GetDataFromVehicleAsStringList(vItem);

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } //GetVehicleItemFromGroupByIDAsStringList

            /// <summary>
            /// Gets a specified vehicle item parameter from a specified group in specified parent class as string
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of vehicle (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value as string</returns>
            public String GetVehicleItemParameterFromGroupByID(String Classname, String GroupID, String VehicleId, String ParameterName)
            {
                try
                {
                    lastException = null;

                    int gid = -1;
                    if (!int.TryParse(GroupID, out gid))
                        return "";

                    int vid = -1;
                    if (!int.TryParse(VehicleId, out vid))
                        return "";

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                    if (groups == null)
                        return "";

                    if (gid > groups.GetItemCount() - 1)
                        return "";

                    Groups_Item item = groups.GetItemByID(gid);
                    if (item == null)
                        return "";

                    Vehicles vehicles = item.vehicles;

                    if (vid > vehicles.GetItemCount() - 1)
                        return "";

                    Vehicles_Item vItem = vehicles.GetItemByID(vid);
                    if (vItem == null)
                        return "";

                    PropertyInfo inf = vItem.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(vItem);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            }

            #endregion

            #region "waypoints"
            /// <summary>
            /// Get the count of Waypoint Items in  specified Groups class of specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <returns>count of Waypoints</returns>
            public int GetWaypointsCountFromGroupItem(String Classname, String GroupID)
            {
                try
                {
                    lastException = null;

                    int id = -1;
                    if (!int.TryParse(GroupID, out id))
                        return 0;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return 0;

                    Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                    if (groups == null)
                        return 0;

                    if (id > groups.GetItemCount() - 1)
                        return 0;

                    Groups_Item item = groups.GetItemByID(id);
                    if (item == null)
                        return 0;

                    if (item.waypoints == null)
                        return 0;

                    return item.waypoints.GetItemCount();

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return 0;
                }
            }

            /// <summary>
            /// Gets a specified waypoint item as iList from a specified group in specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of waypoint (begins with 0)</param>
            /// <returns>waypoint item as IList of strings</returns>
            public IList<String> GetWaypointItemFromGroupByIDAsStringList(String Classname, String GroupID, String WaypointId)
            {

                try
                {
                    lastException = null;

                    int gid = -1;
                    if (!int.TryParse(GroupID, out gid))
                        return null;

                    int wid = -1;
                    if (!int.TryParse(WaypointId, out wid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                    if (groups == null)
                        return null;

                    if (gid > groups.GetItemCount() - 1)
                        return null;

                    Groups_Item item = groups.GetItemByID(gid);
                    if (item == null)
                        return null;

                    Waypoints waypoints = item.waypoints;

                    if (wid > waypoints.GetItemCount() - 1)
                        return null;

                    Waypoint_Item wItem = waypoints.GetItemByID(wid);
                    if (wItem == null)
                        return null;


                    return GetDataFromWaypointAsStringList(wItem);
                    
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } //GetWaypointItemFromGroupByIDAsStringList

            /// <summary>
            /// Gets a specified waypoint item parameter from a specified group in specified parent class as string
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of waypoint (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value as string</returns>
            public String GetWaypointItemParameterFromGroupByID(String Classname, String GroupID, String WaypointId, String ParameterName)
            {
                try
                {
                    lastException = null;

                    int gid = -1;
                    if (!int.TryParse(GroupID, out gid))
                        return "";

                    int wid = -1;
                    if (!int.TryParse(WaypointId, out wid))
                        return "";

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                    if (groups == null)
                        return "";

                    if (gid > groups.GetItemCount() - 1)
                        return "";

                    Groups_Item item = groups.GetItemByID(gid);
                    if (item == null)
                        return "";

                    Waypoints waypoints = item.waypoints;

                    if (wid > waypoints.GetItemCount() - 1)
                        return "";

                    Waypoint_Item wItem = waypoints.GetItemByID(wid);
                    if (wItem == null)
                        return "";

                    PropertyInfo inf = wItem.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(wItem);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } //GetWaypointItemParameterFromGroupByID


            /// <summary>
            /// Gets a specified waypoint item effect as iList from a specified group in specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of waypoint (begins with 0)</param>
            /// <returns>waypoint item effect as IList of strings</returns>
            public IList<String> GetWaypointItemEffectFromGroupByIDAsStringList(String Classname, String GroupID, String WaypointId)
            {

                try
                {
                    lastException = null;

                    int gid = -1;
                    if (!int.TryParse(GroupID, out gid))
                        return null;

                    int wid = -1;
                    if (!int.TryParse(WaypointId, out wid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                    if (groups == null)
                        return null;

                    if (gid > groups.GetItemCount() - 1)
                        return null;

                    Groups_Item item = groups.GetItemByID(gid);
                    if (item == null)
                        return null;

                    Waypoints waypoints = item.waypoints;

                    if (wid > waypoints.GetItemCount() - 1)
                        return null;

                    Waypoint_Item wItem = waypoints.GetItemByID(wid);
                    if (wItem == null)
                        return null;


                    return GetDataFromWaypointEffectsAsStringList(wItem);
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } //GetWaypointItemFromGroupByIDAsStringList

            /// <summary>
            /// Gets a specified waypoint item effect parameter from a specified group in specified parent class as string
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of waypoint (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value as string</returns>
            public String GetWaypointItemEffectParameterFromGroupByID(String Classname, String GroupID, String WaypointId, String ParameterName)
            {
                try
                {
                    lastException = null;

                    int gid = -1;
                    if (!int.TryParse(GroupID, out gid))
                        return "";

                    int wid = -1;
                    if (!int.TryParse(WaypointId, out wid))
                        return "";

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Groups groups = Helpers.GetGroups(Classname, sqmfile, lastException);
                    if (groups == null)
                        return "";

                    if (gid > groups.GetItemCount() - 1)
                        return "";

                    Groups_Item item = groups.GetItemByID(gid);
                    if (item == null)
                        return "";

                    Waypoints waypoints = item.waypoints;

                    if (wid > waypoints.GetItemCount() - 1)
                        return "";

                    Waypoint_Item wItem = waypoints.GetItemByID(wid);
                    if (wItem == null)
                        return "";

                    if (wItem.effects == null)
                        return "";

                    PropertyInfo inf = wItem.effects.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(wItem.effects);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } //GetWaypointItemParameterFromGroupByID


            #endregion


            #endregion

            #region "vehicles"

            /// <summary>
            /// Get the count of vehicle items in specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <returns>count of vehicles</returns>
            public int GetVehiclesCount(String Classname)
            {
                try
                {
                    lastException = null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return 0;

                    Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                    if (vehicles != null)
                        return vehicles.GetItemCount();
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return 0;
                }
            } //GetVehiclesCount

            /// <summary>
            /// Get specified item from vehicle class as string list
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="VehicleID">id of vehicle (begins with 0)</param>
            /// <param name="VehicleItem">optional: VehicleItem (for internal use only)</param>
            /// <returns>vehicle item data as list of strings</returns>
            public IList<String> GetVehicleItemAsStringList(String Classname, String VehicleId)
            {
                try
                {
                    lastException = null;

                    int vid = -1;
                    if (!int.TryParse(VehicleId, out vid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                    if (vehicles == null)
                        return null;

                    if (vid > vehicles.GetItemCount()-1)
                        return null;

                    return GetDataFromVehicleAsStringList(vehicles.GetItemByID(vid));

                }
                catch
                {
                    return null;
                }
            } //GetVehicleItemDataAsStringList




            /// <summary>
            /// Gets a specified string value from a specific vehicle item
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="VehicleID">id of vehicle (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value of parameter as string</returns>
            public string GetVehicleItemParameter(String Classname, String VehicleId, String ParameterName)
            {
                try
                {
                    lastException = null;

                    if (String.IsNullOrEmpty(ParameterName))
                        return "";

                    int vid = -1;
                    if (!int.TryParse(VehicleId, out vid))
                        return null;


                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Vehicles vehicles = Helpers.GetVehicles(Classname, sqmfile, lastException);
                    if (vehicles == null)
                        return "";

                    if (vid > vehicles.GetItemCount()-1)
                        return "";


                    Vehicles_Item vItem = vehicles.GetItemByID(vid);
                    if (vItem == null)
                        return "";

                    PropertyInfo inf = vItem.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(vItem);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");
                    
                }
                catch
                {
                    return "";
                }
            } //GetVehicleStringValueByName

         
            #endregion

            #region "Markers"

            /// <summary>
            /// Get the count of marker items in specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <returns>count of markers</returns>
            public int GetMarkersCount(String Classname)
            {
                try
                {
                    lastException = null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return 0;

                    Markers markers = Helpers.GetMarkers(Classname,sqmfile,lastException);
                    if (markers != null)
                        return markers.GetItemCount();
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return 0;
                }
            } //GetMarkersCount

            /// <summary>
            /// Get specified item from marker class as string list
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="MarkerID">id of marker (begins with 0)</param>
            /// <param name="VehicleItem">optional: MarkerItem (for internal use only)</param>
            /// <returns>marker item data as list of strings</returns>
            public IList<String> GetMarkersItemAsStringList(String Classname, String MarkerId)
            {
                try
                {
                    lastException = null;

                    int mid = -1;
                    if (!int.TryParse(MarkerId, out mid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Markers markers = Helpers.GetMarkers(Classname,sqmfile,lastException);
                    if (markers == null)
                        return null;

                    if (mid >markers.GetItemCount()-1)
                        return null;

                    return GetDataFromMarkerAsStringList(markers.GetItemByID(mid));
  
                }
                catch
                {
                    return null;
                }
            } //GetMarkersItemAsStringList

            /// <summary>
            /// Gets a specified string value from a specific marker item
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="MarkerID">id of marker (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value of parameter as string</returns>
            public string GetMarkersItemParameter(String Classname, String MarkerId, String ParameterName)
            {
                try
                {
                    lastException = null;

                    if (String.IsNullOrEmpty(ParameterName))
                        return "";

                    int mid = -1;
                    if (!int.TryParse(MarkerId, out mid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Markers markers = Helpers.GetMarkers(Classname,sqmfile,lastException);
                    if (markers == null)
                        return "";

                    if (mid > markers.GetItemCount()-1)
                        return "";

                    Markers_Item mItem = markers.GetItemByID(mid);
                    if (mItem == null)
                        return "";

                    PropertyInfo inf = mItem.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(mItem);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");
                    
                }
                catch
                {
                    return "";
                }
            } //GetMarkersItemParameter

        
            #endregion

            #region "Sensors"
            /// <summary>
            /// Get the count of sensors items in specified parent class
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <returns>count of sensors</returns>
            public int GetSensorsCount(String Classname)
            {
                try
                {
                    lastException = null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return 0;

                    Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                    if (sensors != null)
                        return sensors.GetItemCount();
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return 0;
                }
            } //GetSensorsCount

            /// <summary>
            /// Get specified item from sensor class as string list
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="SensorID">id of sensor (begins with 0)</param>
            /// <returns>sensor item data as list of strings</returns>
            public IList<String> GetSensorItemAsStringList(String Classname, String SensorId)
            {
                try
                {
                    lastException = null;

                    int sid = -1;
                    if (!int.TryParse(SensorId, out sid))
                        return null;


                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                    if (sensors == null)
                        return null;

                    if (sid > sensors.GetItemCount()-1)
                        return null;

                    return GetDataFromSensorAsStringList(sensors.GetItemByID(sid));
                    
                }
                catch
                {
                    return null;
                }
            } //GetSensorItemDataAsStringList

            /// <summary>
            /// Gets a specified string value from a specific Sensor item
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="MarkerID">id of sensor (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value of parameter as string</returns>
            public string GetSensorItemParameter(String Classname, String SensorId, String ParameterName)
            {
                try
                {
                    lastException = null;

                    if (String.IsNullOrEmpty(ParameterName))
                        return "";

                    int sid = -1;
                    if (!int.TryParse(SensorId, out sid))
                        return null;

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                    if (sensors == null)
                        return "";

                    if (sid > sensors.GetItemCount() - 1)
                        return "";

                    Sensors_Item sItem = sensors.GetItemByID(sid);
                    if (sItem == null)
                        return "";

                    PropertyInfo inf = sItem.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(sItem);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");

                }
                catch
                {
                    return "";
                }
            } //GetMarkersItemParameter


            /// <summary>
            /// Get specified item from sensor class as string list
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="SensorID">id of sensor (begins with 0)</param>
            /// <returns>sensor item data as list of strings</returns>
            public IList<String> GetSensorItemEffectsAsStringList(String Classname, String SensorId)
            {
                try
                {
                    lastException = null;

                    int sid = -1;
                    if (!int.TryParse(SensorId, out sid))
                        return null;


                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return null;

                    Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                    if (sensors == null)
                        return null;

                    if (sid > sensors.GetItemCount()-1)
                        return null;

                    return GetDataFromSensorEffectsAsStringList(sensors.GetItemByID(sid));
                    
                }
                catch
                {
                    return null;
                }
            } //GetSensorItemEffectDataAsStringList


            /// <summary>
            /// Gets a specified waypoint item effect parameter from a specified group in specified parent class as string
            /// </summary>
            /// <param name="Classname">mission,into,outrowin,outroloose</param>
            /// <param name="GroupID">id of group (begins with 0)</param>
            /// <param name="VehicleId">id of waypoint (begins with 0)</param>
            /// <param name="ParameterName">name of parameter</param>
            /// <returns>value as string</returns>
            public String GetSensorItemEffectParameter(String Classname, String SensorId, String ParameterName)
            {
                try
                {
                    lastException = null;


                    int sid = -1;
                    if (!int.TryParse(SensorId, out sid))
                        return "";

                    if ((sqmfile == null) || (String.IsNullOrEmpty(Classname)))
                        return "";

                    Sensors sensors = Helpers.GetSensors(Classname, sqmfile, lastException);
                    if (sensors == null)
                        return null;

                    if (sid > sensors.GetItemCount() - 1)
                        return "";

                    Sensors_Item sItem = sensors.GetItemByID(sid);
                    if (sItem == null)
                        return "";

                    if (sItem.effects == null)
                        return "";

                    PropertyInfo inf = sItem.effects.GetType().GetProperty(ParameterName);
                    if (inf == null)
                        return "";

                    var value = inf.GetValue(sItem.effects);
                    if (value == null)
                        return "";

                    if (value is String)
                        return (String)value;
                    else if (value is SqmPosition)
                        return value.ToString();
                    else
                        return value.ToString().Replace(",", ".");

                }
                catch (Exception ex)
                {
                    lastException = ex;
                    return null;
                }
            } //GetSensorItemEffectParameter
            #endregion

        #endregion

        #region "internal"


            /// <summary>
        /// Gets the complete class string of sqm file
        /// </summary>
        /// <returns></returns>
        public string ToClassString()
        {
            return sqmfile.ToClassString();
        }

        /// <summary>
        /// Gets the last thrown exeption string
        /// </summary>
        /// <returns></returns>
        public string GetLastException()
        {
            if (lastException != null)
                return lastException.Message;
            else
                return "";
        }

        #region "process class"

        /// <summary>
        /// Process the imported sqm file as string
        /// </summary>
        /// <param name="klasse">mission, intro, outrowin, outroloose</param>
        /// <param name="txt">imported sqm file as string</param>
        /// <returns></returns>
        private bool ProcessClass(ClassNames klasse, string txt)
        {
            Regex myRegex;
            string tmp = "";

            try{
                // addOns
                myRegex = new Regex(@"addOns\[\]=");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if(!string.IsNullOrEmpty(tmp))
                {
                    tmp = tmp.Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    string[] tmp2=tmp.Split(',');
                    foreach(string tmp3 in tmp2)
                    {
                        switch (klasse)
                        {
                            case ClassNames.intro:
                                sqmfile.intro.addOns.Add(tmp3);
                                break;
                            case ClassNames.mission:
                                sqmfile.mission.addOns.Add(tmp3);
                                break;
                            case ClassNames.outrowin:
                                sqmfile.outrowin.addOns.Add(tmp3);
                                break;
                            case ClassNames.outroloose:
                                sqmfile.outroloose.addOns.Add(tmp3);
                                break;
                        }//switch
                        
                    }
                }

                // addOnsAuto
                myRegex = new Regex(@"addOnsAuto\[\]=");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    tmp = tmp.Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    string[] tmp2 = tmp.Split(',');
                    foreach (string tmp3 in tmp2)
                    {
                        switch (klasse)
                        {
                            case ClassNames.intro:
                                sqmfile.intro.addOnsAuto.Add(tmp3);
                                break;
                            case ClassNames.mission:
                                sqmfile.mission.addOnsAuto.Add(tmp3);
                                break;
                            case ClassNames.outrowin:
                                sqmfile.outrowin.addOnsAuto.Add(tmp3);
                                break;
                            case ClassNames.outroloose:
                                sqmfile.outroloose.addOnsAuto.Add(tmp3);
                                break;
                        }//switch
                    }
                }

                // randomSeed
                switch (klasse)
                {
                    case ClassNames.intro:
                        sqmfile.intro.randomSeed = GetIntVariable("randomSeed", txt).ToString(culture);
                        break;
                    case ClassNames.mission:
                        sqmfile.mission.randomSeed = GetIntVariable("randomSeed", txt).ToString(culture);
                        break;
                    case ClassNames.outrowin:
                        sqmfile.outrowin.randomSeed = GetIntVariable("randomSeed", txt).ToString(culture);
                        break;
                    case ClassNames.outroloose:
                        sqmfile.outroloose.randomSeed = GetIntVariable("randomSeed", txt).ToString(culture);
                        break;
                }//switch
      
                // class Intel
                myRegex = new Regex("class Intel");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (string.IsNullOrEmpty(tmp))
                {
                    lastException = new Exception("Could not read class Intel.");
                    return false;
                }
                else
                {
                    // decode Intel class
                    switch (klasse)
                    {
                        case ClassNames.intro:
                            if (!ProcessIntel(tmp, sqmfile.intro.intel))
                                return false;
                            break;
                        case ClassNames.mission:
                            if (!ProcessIntel(tmp, sqmfile.mission.intel))
                                return false;
                            break;
                        case ClassNames.outrowin:
                            if (!ProcessIntel(tmp, sqmfile.outrowin.intel))
                                return false;
                            break;
                        case ClassNames.outroloose:
                            if (!ProcessIntel(tmp, sqmfile.outroloose.intel))
                                return false;
                            break;
                    }//switch

                    // delete class to prevent errors on next steps
                    txt = RemoveToNextBracket(txt, myRegex.Match(txt).Index);
                }

                // class Groups
                myRegex = new Regex("class Groups");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    // decode Groups class
                    switch (klasse)
                    {
                        case ClassNames.intro:
                            if (!ProcessGroups(tmp, sqmfile.intro.groups))
                                return false;
                            break;
                        case ClassNames.mission:
                            if (!ProcessGroups(tmp, sqmfile.mission.groups))
                                return false;
                            break;
                        case ClassNames.outrowin:
                            if (!ProcessGroups(tmp, sqmfile.outrowin.groups))
                                return false;
                            break;
                        case ClassNames.outroloose:
                            if (!ProcessGroups(tmp, sqmfile.outroloose.groups))
                                return false;
                            break;
                    }//switch

                    // delete class to prevent errors on next steps
                    txt = RemoveToNextBracket(txt, myRegex.Match(txt).Index);
                }

                // class Vehicles
                myRegex = new Regex("class Vehicles");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    // decode class
                    switch (klasse)
                    {
                        case ClassNames.intro:
                            if (!ProcessVehicles(tmp, sqmfile.intro.vehicles))
                                return false;
                            break;
                        case ClassNames.mission:
                            if (!ProcessVehicles(tmp, sqmfile.mission.vehicles))
                                return false;
                            break;
                        case ClassNames.outrowin:
                            if (!ProcessVehicles(tmp, sqmfile.outrowin.vehicles))
                                return false;
                            break;
                        case ClassNames.outroloose:
                            if (!ProcessVehicles(tmp, sqmfile.outroloose.vehicles))
                                return false;
                            break;
                    }//switch


                    // delete class to prevent errors on next steps
                    txt = RemoveToNextBracket(txt, myRegex.Match(txt).Index);
                }

                // class Markers
                myRegex = new Regex("class Markers");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    // decode class
                    switch (klasse)
                    {
                        case ClassNames.intro:
                            if (!ProcessMarkers(tmp, sqmfile.intro.markers))
                                return false;
                            break;
                        case ClassNames.mission:
                            if (!ProcessMarkers(tmp, sqmfile.mission.markers))
                                return false;
                            break;
                        case ClassNames.outrowin:
                            if (!ProcessMarkers(tmp, sqmfile.outrowin.markers))
                                return false;
                            break;
                        case ClassNames.outroloose:
                            if (!ProcessMarkers(tmp, sqmfile.outroloose.markers))
                                return false;
                            break;
                    }//switch


                    // delete class to prevent errors on next steps
                    txt = RemoveToNextBracket(txt, myRegex.Match(txt).Index);
                }

                // class Sensors
                myRegex = new Regex("class Sensors");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    // decode class
                    switch (klasse)
                    {
                        case ClassNames.intro:
                            if (!ProcessSensors(tmp, sqmfile.intro.sensors))
                                return false;
                            break;
                        case ClassNames.mission:
                            if (!ProcessSensors(tmp, sqmfile.mission.sensors))
                                return false;
                            break;
                        case ClassNames.outrowin:
                            if (!ProcessSensors(tmp, sqmfile.outrowin.sensors))
                                return false;
                            break;
                        case ClassNames.outroloose:
                            if (!ProcessSensors(tmp, sqmfile.outroloose.sensors))
                                return false;
                            break;
                    }//switch


                    // delete class to prevent errors on next steps
                    txt = RemoveToNextBracket(txt, myRegex.Match(txt).Index);
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

        #region "Markers"
        private IList<String> GetDataFromMarkerAsStringList(Markers_Item MarkerItem)
        {
            try
            {
                if (MarkerItem == null)
                    return null;

                IList<String> data = new List<String>();

                data.Add(MarkerItem.position.X.ToString(culture));
                data.Add(MarkerItem.position.Z.ToString(culture));
                data.Add(MarkerItem.position.Y.ToString(culture));
                data.Add(MarkerItem.name);
                data.Add(MarkerItem.text);
                data.Add(MarkerItem.markerType);
                data.Add(MarkerItem.type);
                data.Add(MarkerItem.colorName);

                data.Add(MarkerItem.a.ToString(culture));
                data.Add(MarkerItem.b.ToString(culture));
                data.Add(MarkerItem.angle.ToString(culture));
               

                return data;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Parse all Markers out of the string
        /// </summary>
        /// <param name="txt">given sqm file string</param>
        /// <param name="Markers">ref of marker list</param>
        /// <returns>true/false</returns>
        private bool ProcessMarkers(string txt, Markers Markers)
        {
            Regex myRegex;
            string tmp = "";
            try
            {
                int Items = GetIntVariable("items", txt);
                if (Items > 0)
                {
                    for (int x = 0; x < Items; x++)
                    {
                        myRegex = new Regex("class Item" + x.ToString(culture));
                        tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            Markers_Item item = ProcessMarkersItem(tmp);
                            if (item != null)
                            {
                                Markers.AddItem(item);
                            }
                        }
                    } // for
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //ProcessMarkers

        /// <summary>
        /// process every marker item in markers class
        /// </summary>
        /// <param name="txt">item class string</param>
        /// <returns>marker item</returns>
        private Markers_Item ProcessMarkersItem(string txt)
        {

            try
            {
                Markers_Item item = new Markers_Item();
                item.position = GetSqmPositionVariable("position", txt);
                item.name = GetStringVariable("name", txt);
                item.text = GetStringVariable("text", txt);
                item.markerType = GetStringVariable("markerType", txt);
                item.type = GetStringVariable("type", txt);
                item.colorName = GetStringVariable("colorName", txt);
                item.a = GetDoubleVariable("a", txt);
                item.b = GetDoubleVariable("b", txt);
                item.angle = GetDoubleVariable("angle", txt);

                return item;
            }
            catch
            {
                return null;
            }
        } //ProcessMarkersItem
        #endregion

        #region "Sensors"

        private IList<String> GetDataFromSensorEffectsAsStringList(Sensors_Item SensorItem)
        {
            try
            {
                if (SensorItem == null)
                    return null;

                IList<String> data = new List<String>();

                data.Add(SensorItem.effects.condition);
                data.Add(SensorItem.effects.sound);
                data.Add(SensorItem.effects.voice);
                data.Add(SensorItem.effects.soundEnv);
                data.Add(SensorItem.effects.soundDet);
                data.Add(SensorItem.effects.track);
                data.Add(SensorItem.effects.titleEffect);
                data.Add(SensorItem.effects.titleType);
                data.Add(SensorItem.effects.title);

                return data;
            }
            catch
            {
                return null;
            }
        }

        private IList<String> GetDataFromSensorAsStringList(Sensors_Item SensorItem)
        {
            try
            {
                if (SensorItem == null)
                    return null;

                IList<String> data = new List<String>();

                data.Add(SensorItem.position.X.ToString(culture));
                data.Add(SensorItem.position.Z.ToString(culture));
                data.Add(SensorItem.position.Y.ToString(culture));
                data.Add(SensorItem.name);
                data.Add(SensorItem.text);
                data.Add(SensorItem.rectangular.ToString(culture));
                data.Add(SensorItem.a.ToString(culture));
                data.Add(SensorItem.b.ToString(culture));
                data.Add(SensorItem.angle.ToString(culture));
                data.Add(SensorItem.interruptable.ToString(culture));
                data.Add(SensorItem.timeoutMin.ToString(culture));
                data.Add(SensorItem.timeoutMid.ToString(culture));
                data.Add(SensorItem.timeoutMax.ToString(culture));
                data.Add(SensorItem.type);
                data.Add(SensorItem.activationBy);
                data.Add(SensorItem.repeating.ToString(culture));
                data.Add(SensorItem.activationType);
                data.Add(SensorItem.expCond);
                data.Add(SensorItem.expActiv);
                data.Add(SensorItem.expDesactiv);

                return data;
            }
            catch
            {
                return null;
            }
        }



        private bool ProcessSensors(string txt, Sensors Sensors)
        {
            Regex myRegex;
            string tmp = "";
            try
            {
                int Items = GetIntVariable("items", txt);
                if (Items > 0)
                {
                    for (int x = 0; x < Items; x++)
                    {
                        myRegex = new Regex("class Item" + x.ToString(culture));
                        tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            Sensors_Item item = ProcessSensorsItem(tmp);
                            if (item != null)
                            {
                                Sensors.AddItem(item);
                            }
                        }
                    } // for
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //ProcessMarkers

        private Sensors_Item ProcessSensorsItem(string txt)
        {
            Regex myRegex;
            string tmp = "";

            try
            {
                Sensors_Item item = new Sensors_Item();
                item.position = GetSqmPositionVariable("position", txt);
                item.name = GetStringVariable("name", txt);
                item.text = GetStringVariable("text", txt);
                item.rectangular = GetIntVariable("rectangular", txt);
                item.a = GetDoubleVariable("a", txt);
                item.b = GetDoubleVariable("b", txt);
                item.angle = GetDoubleVariable("angle", txt);
                item.interruptable = GetIntVariable("interruptable", txt);

                item.timeoutMin = GetDoubleVariable("timeoutMin", txt);
                item.timeoutMid = GetDoubleVariable("timeoutMid", txt);
                item.timeoutMax = GetDoubleVariable("timeoutMax", txt);

                item.type = GetStringVariable("type", txt);
                item.activationBy = GetStringVariable("activationBy", txt);
                item.repeating = GetIntVariable("repeating", txt);

                item.activationType = GetStringVariable("activationType", txt);
                item.expCond = GetStringVariable("expCond", txt);
                item.expActiv = GetStringVariable("expActiv", txt);
                item.expDesactiv = GetStringVariable("expDesactiv", txt);


                //effects
                myRegex = new Regex("class Effects");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    item.effects = new Effects();
                    item.effects.condition = GetStringVariable("condition", txt);
                    item.effects.sound = GetStringVariable("sound", txt);
                    item.effects.voice = GetStringVariable("voice", txt);
                    item.effects.soundEnv = GetStringVariable("soundEnv", txt);
                    item.effects.soundDet = GetStringVariable("soundDet", txt);
                    item.effects.track = GetStringVariable("track", txt);
                    item.effects.titleEffect = GetStringVariable("titleEffect", txt);
                    item.effects.titleType = GetStringVariable("titleType", txt);
                    item.effects.title = GetStringVariable("title", txt);
                }
                return item;
            }
            catch
            {
                return null;
            }
        } //ProcessMarkersItem
        #endregion

        #region "Waypoints"

        private IList<String> GetDataFromWaypointEffectsAsStringList(Waypoint_Item WaypointItem)
        {
            try
            {
                if (WaypointItem == null)
                    return null;

                IList<String> data = new List<String>();

                data.Add(WaypointItem.effects.condition);
                data.Add(WaypointItem.effects.sound);
                data.Add(WaypointItem.effects.voice);
                data.Add(WaypointItem.effects.soundEnv);
                data.Add(WaypointItem.effects.soundDet);
                data.Add(WaypointItem.effects.track);
                data.Add(WaypointItem.effects.titleEffect);
                data.Add(WaypointItem.effects.titleType);
                data.Add(WaypointItem.effects.title);

                return data;
            }
            catch
            {
                return null;
            }
        }

        private IList<String> GetDataFromWaypointAsStringList(Waypoint_Item WaypointItem)
        {
            try
            {
                if (WaypointItem == null)
                    return null;

                IList<String> data = new List<String>();

                data.Add(WaypointItem.name);
                data.Add(WaypointItem.position.X.ToString(culture));
                data.Add(WaypointItem.position.Z.ToString(culture));
                data.Add(WaypointItem.position.Y.ToString(culture));
                data.Add(WaypointItem.type);
                data.Add(WaypointItem.placement.ToString(culture));
                data.Add(WaypointItem.completitionRadius.ToString(culture));
                data.Add(WaypointItem.timeoutMin.ToString(culture));
                data.Add(WaypointItem.timeoutMid.ToString(culture));
                data.Add(WaypointItem.timeoutMax.ToString(culture));
                data.Add(WaypointItem.text);
                data.Add(WaypointItem.description);
                data.Add(WaypointItem.combatMode);
                data.Add(WaypointItem.formation);
                data.Add(WaypointItem.speed);
                data.Add(WaypointItem.combat);
                data.Add(WaypointItem.expCond);
                data.Add(WaypointItem.expActiv);
                data.Add(WaypointItem.script);
                data.Add(WaypointItem.showWP);

                return data;
            }
            catch
            {
                return null;
            }
        }

        private bool ProcessWaypoints(string txt, Waypoints Waypoints)
        {
            Regex myRegex;
            string tmp = "";
            try
            {
                int Items = GetIntVariable("items", txt);
                if (Items > 0)
                {
                    for (int x = 0; x < Items; x++)
                    {
                        myRegex = new Regex("class Item" + x.ToString(culture));
                        tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            Waypoint_Item item = ProcessWaypointItem(tmp);
                            if (item != null)
                            {
                                Waypoints.AddItem(item);
                            }
                        }
                    } // for
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        private Waypoint_Item ProcessWaypointItem(string txt)
        {

            try
            {
                Waypoint_Item item = new Waypoint_Item();
                Regex myRegex;
                string tmp = "";

                item.name = GetStringVariable("name", txt);

                item.position = GetSqmPositionVariable("position", txt);
                item.type = GetStringVariable("type", txt);
                item.placement = GetDoubleVariable("placement", txt);
                item.completitionRadius = GetDoubleVariable("completitionRadius", txt);

                item.timeoutMin = GetDoubleVariable("timeoutMin", txt);
                item.timeoutMid = GetDoubleVariable("timeoutMid", txt);
                item.timeoutMax = GetDoubleVariable("timeoutMax", txt);

                item.text = GetStringVariable("text", txt);
                item.description = GetStringVariable("description", txt);
                item.combatMode = GetStringVariable("combatMode", txt);
                item.formation = GetStringVariable("formation", txt);
                item.speed = GetStringVariable("speed", txt);
                item.combat = GetStringVariable("combat", txt);
                item.expCond = GetStringVariable("expCond", txt);
                item.expActiv = GetStringVariable("expActiv", txt);
                item.script = GetStringVariable("script", txt);
                item.showWP = GetStringVariable("showWP", txt);

                //effects
                myRegex = new Regex("class Effects");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    item.effects = new Effects();
                    item.effects.condition = GetStringVariable("condition", txt);
                    item.effects.sound = GetStringVariable("sound", txt);
                    item.effects.voice = GetStringVariable("voice", txt);
                    item.effects.soundEnv = GetStringVariable("soundEnv", txt);
                    item.effects.soundDet = GetStringVariable("soundDet", txt);
                    item.effects.track = GetStringVariable("track", txt);
                    item.effects.titleEffect = GetStringVariable("titleEffect", txt);
                    item.effects.titleType = GetStringVariable("titleType", txt);
                    item.effects.title = GetStringVariable("title", txt);
                }


                return item;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region "Vehicles"

        private IList<String> GetDataFromVehicleAsStringList(Vehicles_Item VehicleItem)
        {
            try
            {
                if (VehicleItem == null)
                    return null;

                IList<String> data = new List<String>();

                data.Add(VehicleItem.id.ToString(culture));
                data.Add(VehicleItem.vehicle);
                data.Add(VehicleItem.side);
                data.Add(VehicleItem.position.X.ToString(culture));
                data.Add(VehicleItem.position.Z.ToString(culture));
                data.Add(VehicleItem.position.Y.ToString(culture));
                data.Add(VehicleItem.placement.ToString(culture));
                data.Add(VehicleItem.azimut.ToString(culture));
                data.Add(VehicleItem.offsetY.ToString(culture));
                data.Add(VehicleItem.special);
                data.Add(VehicleItem.player);
                data.Add(VehicleItem.str_lock);
                data.Add(VehicleItem.rank);
                data.Add(VehicleItem.age);
                data.Add(VehicleItem.text);
                data.Add(VehicleItem.init);
                data.Add(VehicleItem.description);
                data.Add(VehicleItem.skill.ToString(culture));
                data.Add(VehicleItem.health.ToString(culture));
                data.Add(VehicleItem.fuel.ToString(culture));
                data.Add(VehicleItem.ammo.ToString(culture));
                data.Add(VehicleItem.presence.ToString(culture));
                data.Add(VehicleItem.presenceCondition);
                data.Add(VehicleItem.leader.ToString(culture));

                return data;
            }
            catch
            {
                return null;
            }
        }

        private bool ProcessVehicles(string txt, Vehicles Vehicles)
        {
            Regex myRegex;
            string tmp = "";
            try
            {
                int Items = GetIntVariable("items", txt);
                if (Items > 0)
                {
                    for (int x = 0; x < Items; x++)
                    {
                        myRegex = new Regex("class Item" + x.ToString(culture));
                        tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            Vehicles_Item item = ProcessVehicleItem(tmp);
                            if (item != null)
                            {
                                Vehicles.AddItem(item);
                            }
                        }
                    } // for
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        private Vehicles_Item ProcessVehicleItem(string txt)
        {

            try
            {
                Vehicles_Item item = new Vehicles_Item();

                item.id = GetIntVariable("id", txt);
                item.position = GetSqmPositionVariable("position", txt);
                item.vehicle = GetStringVariable("vehicle", txt);
                item.side = GetStringVariable("side", txt);
                item.special = GetStringVariable("special", txt);
                item.player = GetStringVariable("player", txt);
                item.str_lock = GetStringVariable("lock", txt);
                item.rank = GetStringVariable("rank", txt);
                item.age = GetStringVariable("age", txt);
                item.text = GetStringVariable("text", txt);
                item.init = GetStringVariable("init", txt);
                item.description = GetStringVariable("description", txt);
                item.presenceCondition = GetStringVariable("presenceCondition", txt);
                item.leader = GetIntVariable("leader", txt);
                item.placement = GetDoubleVariable("placement", txt);
                item.azimut = GetDoubleVariable("azimut", txt);
                item.offsetY = GetDoubleVariable("offsetY", txt);
                item.skill = GetDoubleVariable("skill", txt);
                item.health = GetDoubleVariable("health", txt);
                item.fuel = GetDoubleVariable("fuel", txt);
                item.ammo = GetDoubleVariable("ammo", txt);
                item.presence = GetDoubleVariable("presence", txt);

                return item;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region "Groups"


        private bool ProcessGroups(string txt, Groups Groups)
        {
            Regex myRegex;
            string tmp = "";

            try {
                int Items = GetIntVariable("items", txt);
                if (Items > 0)
                {
                    for (int x = 0; x < Items; x++)
                    { 
                        myRegex = new Regex("class Item" + x.ToString(culture));
                        tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            txt=RemoveToNextBracket(txt, myRegex.Match(txt).Index);
                            Groups_Item item = ProcessGroupsItem(tmp);
                            if (item != null)
                            {
                                Groups.AddItem(item);
                            }
                        }
                    } // for
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        private Groups_Item ProcessGroupsItem(string txt)
        {
            Regex myRegex;
            string tmp = "";

            try
            {
                Groups_Item item = new Groups_Item();

                //side
                item.side = GetStringVariable("side", txt);
                if(string.IsNullOrEmpty(item.side))
                    return null;

                //Vehicles
                myRegex = new Regex("class Vehicles");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    // decode vehicles
                    Vehicles vItem = new Vehicles();
                    if(ProcessVehicles(tmp,vItem))
                        item.vehicles=vItem;
                }

                //Waypoints
                myRegex = new Regex("class Waypoints");
                tmp = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (!string.IsNullOrEmpty(tmp))
                {
                    // decode waypoints
                    Waypoints wItem = new Waypoints();
                    if (ProcessWaypoints(tmp, wItem))
                        item.waypoints = wItem;
                }

                return item;
            }
            catch 
            {
                return null;
            }
        }
        #endregion

        #region "Intel"


        private bool ProcessIntel(string txt, sqmfile.Intel Intel)
        {
            try
            {
                Intel.briefingName = GetStringVariable("briefingName", txt);
                Intel.overviewText = GetStringVariable("overviewText", txt);
                Intel.year = GetIntVariable("year", txt);
                Intel.month = GetIntVariable("month", txt);
                Intel.day = GetIntVariable("day", txt);
                Intel.hour = GetIntVariable("hour", txt);
                Intel.minute = GetIntVariable("minute", txt);
                Intel.startWeather = GetDoubleVariable("startWeather", txt);
                Intel.forecastWeather = GetDoubleVariable("forecastWeather", txt);
                Intel.startFog = GetDoubleVariable("startFog", txt);
                Intel.forecastFog = GetDoubleVariable("forecastFog", txt);
                Intel.startFogDecay = GetDoubleVariable("startFogDecay", txt);
                Intel.forecastFogDecay = GetDoubleVariable("forecastFogDecay", txt);
                Intel.rainForced = GetIntVariable("rainForced", txt);
                Intel.startRain = GetDoubleVariable("startRain", txt);
                Intel.forecastRain = GetDoubleVariable("forecastRain", txt);
                Intel.lightningsForced = GetIntVariable("lightningsForced", txt);
                Intel.startLightnings = GetDoubleVariable("startLightnings", txt);
                Intel.forecastLightnings = GetDoubleVariable("forecastLightnings", txt);
                Intel.wavesForced = GetIntVariable("wavesForced", txt);
                Intel.startWaves = GetDoubleVariable("startWaves", txt);
                Intel.forecastWaves = GetDoubleVariable("forecastWaves", txt);
                Intel.startWind = GetDoubleVariable("startWind", txt);
                Intel.forecastWind = GetDoubleVariable("forecastWind", txt);
                Intel.windForced = GetIntVariable("windForced", txt);
                Intel.startGust = GetDoubleVariable("startGust", txt);
                Intel.forecastGust = GetDoubleVariable("forecastGust", txt);
                Intel.startWindDir = GetDoubleVariable("startWindDir", txt);
                Intel.forecastWindDir = GetDoubleVariable("forecastWindDir", txt);
                Intel.timeOfChanges = GetDoubleVariable("timeOfChanges", txt);
                Intel.IndFriendlyTo = GetStringVariable("IndFriendlyTo", txt);
                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        #endregion

        #region "tools"

        private String GetDataFromItemAsString<T>(T data, String ParameterName)
        {

            try
            {
                if ((data == null) || (String.IsNullOrEmpty(ParameterName)))
                    return "";

                PropertyInfo inf = data.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return "";

                var value = inf.GetValue(data);
                if (value == null)
                    return "";

                if (value is String)
                    return (String)value;
                else
                    return value.ToString();

            }
            catch
            {
                return "";
            }
        }

        private int GetDataFromItemAsInt<T>(T data, String ParameterName)
        {
            try
            {
                if ((data == null) || (String.IsNullOrEmpty(ParameterName)))
                    return 0;

                PropertyInfo inf = data.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return 0;

                var value = inf.GetValue(data);
                if (value == null)
                    return 0;

                if (value is int)
                    return (int)value;
                else
                {
                    int res = 0;
                    if (int.TryParse(value.ToString(), out res))
                    {
                        return res;
                    }
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private double GetDataFromItemAsDouble<T>(T data, String ParameterName)
        {
            try
            {
                if ((data == null) || (String.IsNullOrEmpty(ParameterName)))
                    return 0;

                PropertyInfo inf = data.GetType().GetProperty(ParameterName);
                if (inf == null)
                    return 0;

                var value = inf.GetValue(data);
                if (value == null)
                    return 0;

                if (value is Double)
                    return (Double)value;
                else
                {
                    Double res = 0;
                    if (Double.TryParse(value.ToString(), out res))
                    {
                        return res;
                    }
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private string RemoveToNextBracket(string txt, int index)
        {
            try
            {
                int startBracket = 0;
                int endBracket = 0;

                if (string.IsNullOrEmpty(txt))
                    return "";

                if (index <= 0)
                    return "";

                // find start bracket
                for (int i = index; i < txt.Length; i++)
                {
                    if ((txt[i]) == '{')
                    {
                        startBracket = i;
                        break;
                    }
                }

                if (startBracket <= 0)
                    return "";

                int openbrackets = 0;
                for (int i = startBracket; i < txt.Length; i++)
                {
                    if ((txt[i]) == '{')
                    {
                        openbrackets += 1;
                    }

                    if ((txt[i]) == '}')
                    {
                        if (openbrackets > 0)
                            openbrackets -= 1;

                        if (openbrackets == 0)
                        {
                            endBracket = i;
                            break;
                        }
                    }
                }

                if (endBracket <= 0)
                    return "";

                return txt.Remove(index, endBracket - index +2);
            }
            catch (Exception ex)
            {
                lastException = ex;
                return "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private bool CutMainParts(string txt)
        {
            Match match;
            Regex myRegex;


            try
            {
                // get Version string
                match = Regex.Match(txt, @"version=([\d]*)\b;");
                if (match.Success)
                {
                    strVersion = match.Groups[1].Value;
                }

                if (string.IsNullOrEmpty(strVersion))
                {
                    lastException = new Exception("Could not read version number.");
                    return false;
                }

                // get mission class content
                myRegex = new Regex("class Mission");
                strMission = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (string.IsNullOrEmpty(strMission))
                {
                    lastException = new Exception("Could not find mission class in file.");
                    return false;
                }

                // get intro class content
                myRegex = new Regex("class Intro");
                strIntro = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (string.IsNullOrEmpty(strIntro))
                {
                    lastException = new Exception("Could not find intro class in file.");
                    return false;
                }


                // get OutroWin class content
                myRegex = new Regex("class OutroWin");
                strOutroWin = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (string.IsNullOrEmpty(strOutroWin))
                {
                    lastException = new Exception("Could not find OutroWin class in file.");
                    return false;
                }

                // get OutroLoose class content
                myRegex = new Regex("class OutroLoose");
                strOutroLoose = GetInsideBrackets(txt, myRegex.Match(txt).Index);
                if (string.IsNullOrEmpty(strOutroLoose))
                {
                    lastException = new Exception("Could not find OutroLoose class in file.");
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } // SplitMainParts

        /// <summary>
        /// process version number
        /// </summary>
        /// <returns></returns>
        private bool ProcessVersion()
        {
            try
            {
                if (string.IsNullOrEmpty(strVersion))
                    return false;
                else
                    sqmfile.version = strVersion;

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        }

        /// <summary>
        /// Get the content between two brackets
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetInsideBrackets(string txt, int index)
        {
            try{
                int startBracket = 0;
                int endBracket = 0;

                if (string.IsNullOrEmpty(txt))
                    return "";

                if (index <= 0)
                    return "";

                // find start bracket
                for (int i = index; i < txt.Length; i++)
                {
                    if ((txt[i]) == '{')
                    {
                        startBracket = i;
                        break;
                    }
                }

                if (startBracket <= 0)
                    return "";

                int openbrackets = 0;
                for (int i = startBracket; i < txt.Length; i++)
                {
                    if ((txt[i]) == '{')
                    {
                        openbrackets += 1;
                    }

                    if ((txt[i]) == '}')
                    {
                        if (openbrackets > 0)
                            openbrackets -= 1;

                        if (openbrackets == 0)
                        {
                            endBracket = i;
                            break;
                        }
                    }
                }

                if (endBracket <= 0)
                    return "";

                return txt.Substring(startBracket +1, endBracket - (startBracket+1));
            }
            catch (Exception ex)
            {
                lastException = ex;
                return "";
            }
        }
        #endregion

        #region "extract values"


        private SqmPosition GetSqmPositionVariable(string name, string txt)
        {
            Match match;
            string tmp = "";

            try {
                if ((string.IsNullOrEmpty(txt)) || (string.IsNullOrEmpty(name)))
                    return null;

                match = Regex.Match(txt, name + @"\[\]=\{([\d|.|,]*)\};");
                if (match.Success)
                {
                    tmp = match.Groups[1].Value;
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return null;
                    }
                    else
                    {
                        if (!tmp.Contains(","))
                            return null;

                        string[] num = tmp.Split(',');

                        if (num.Length != 3)
                            return null;

                        double x = 0;
                        double y = 0;
                        double z = 0;

                        if (!double.TryParse(num[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x))
                            return null;
                        if (!double.TryParse(num[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y))
                            return null;
                        if (!double.TryParse(num[2], NumberStyles.Any, CultureInfo.InvariantCulture, out z))
                            return null;

                        return new SqmPosition(x,y,z);

                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            { 
                return null;
            }
        }

        private double GetDoubleVariable(string name, string txt)
        {
            Match match;
            string tmp = "";

            try
            {          
                if ((string.IsNullOrEmpty(txt)) || (string.IsNullOrEmpty(name)))
                    return 0;

                match = Regex.Match(txt, name + @"=([\d|.]*)\b;");
                if (match.Success)
                {
                    tmp = match.Groups[1].Value;
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return 0;
                    }
                    else
                    {
                        double x=0;
                        if (double.TryParse(tmp,NumberStyles.Any,CultureInfo.InvariantCulture, out x))
                            return x;
                        else
                            return 0;
                            
                    }
                }
                else
                {
                    return 0;
                }


            }
            catch (Exception ex)
            {
                lastException = ex;
                return 0;
            }
        }


        private int GetIntVariable(string name, string txt)
        {
            Match match;
            string tmp = "";

            try
            {
                if ((string.IsNullOrEmpty(txt)) || (string.IsNullOrEmpty(name)))
                    return 0;

                match = Regex.Match(txt, name + @"=([\d]*)\b;");
                if (match.Success)
                {
                    tmp = match.Groups[1].Value;
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return 0;
                    }
                    else
                    {
                        int x=0;
                        if (int.TryParse(tmp,out x))
                            return x;
                        else
                            return 0;
                            
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                lastException = ex;
                return 0;
            }
        }


        /// <summary>
        /// get an String variable out of txt
        /// </summary>
        /// <param name="name"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        private string GetStringVariable(string name, string txt)
        {
            Match match;
            string tmp = "";


            try
            {
                if ((string.IsNullOrEmpty(txt)) || (string.IsNullOrEmpty(name)))
                    return "";


                match = Regex.Match(txt, name + @"=""(.*\n?)"";");
                if (match.Success)
                {
                    tmp = match.Groups[1].Value;
                    if (string.IsNullOrEmpty(tmp))
                    {
                        return "";
                    }
                    else
                    {
                        return tmp;
                    }
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                lastException = ex;
                return "";
            }
        }
        #endregion
        #endregion

        #region "tests"
        //public String GetStringValue(String Classname, String ObjectName, String ValueName)
        //{
        //    if (sqmfile == null)
        //        return "";

        //    // get mission
        //    var missionmember = sqmfile.GetType().GetMember(Classname).Single();
        //    var mission = ((FieldInfo)missionmember).GetValue(sqmfile);

        //    // get 
        //    var itemmember = mission.GetType().GetMember(ObjectName).Single();
        //    var item = ((FieldInfo)itemmember).GetValue(mission);

        //    PropertyInfo inf = item.GetType().GetProperty(ValueName);
        //    var value = inf.GetValue(item);

        //    if (value is String )
        //        return (String)inf.GetValue(item);
        //    else
        //        return value.ToString(culture);
        //var myPropertyInfo = myType.GetProperty(infoParts[1]);


        //object bla=null;

        //Type tp = bla.GetType();

        //PropertyInfo inf = cl.GetProperty("SqmFile.mission.intel.briefingName");
        //if(inf!=null)
        //    if(inf.PropertyType==typeof(string))
        //        return (String)inf.GetValue(sqmfile.mission.intel);
        //    else return "";
        //else
        //    return "";

        //return "";
        //}

        //public object getValue(String memberName)
        //{
        //    var member = this.GetType().GetMember(memberName).Single();
        //    if (member.MemberType == MemberTypes.Property)
        //    {
        //        return ((PropertyInfo)member).GetValue(this, null);
        //    }
        //    if (member.MemberType == MemberTypes.Field)
        //    {
        //        return ((FieldInfo)member).GetValue(this);
        //    }
        //    else
        //    {
        //        throw new Exception("Bad member type.");
        //    }
        //}

        //public IList<String> GetIntelMain(String ClassName)
        //{
        //    try
        //    {
        //        IList<String> data = new List<String>();

        //        lastException = null;

        //        if (sqmfile == null)
        //            return null;

        //        Intel intel=GetIntel(ClassName);                
        //        if (intel == null)
        //            return null;

        //        data.Add(intel.briefingName);
        //        data.Add(intel.overviewText);
        //        data.Add(intel.IndFriendlyTo);

        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        lastException = ex;
        //        return null;
        //    }
        //} // GetIntelMain

        //public IList<Double> GetIntelDateTime(String ClassName)
        //{
        //    IList<Double> data = new List<Double>();

        //    lastException = null;

        //    if (sqmfile == null)
        //        return null;

        //    Intel intel = GetIntel(ClassName);
        //    if (intel == null)
        //        return null;

        //    return data;
        //}
        #endregion
    }
}
