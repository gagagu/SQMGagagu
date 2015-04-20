using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SQMGagagu
{
    public class ArmaProfile
    {
        private static Exception lastException;

        /// <summary>
        /// Gets the last thrown exeption string
        /// </summary>
        /// <returns></returns>
        public static Exception GetLastException()
        {
            if (lastException != null)
                return lastException;
            else
                return null;
        }

        /// <summary>
        /// find out Arma profile for ProfileName
        /// </summary>
        /// <param name="ProfileName">Profile Name of user</param>
        /// <returns>true when found, else false</returns>
        public static string CheckProfileDirectory(string profilename)
        {
            try
            {
                string prpath = "";
                lastException = null;

                // check if my document exists
               string  mydocument = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (string.IsNullOrEmpty(mydocument))
                {
                    lastException = new DirectoryNotFoundException("Cannot find MyDocuments folder.");
                    return "";
                }

                // check if exists Arma 3 folder
                if (!Directory.Exists(Path.Combine(mydocument, "Arma 3")))
                {
                    lastException = new DirectoryNotFoundException("Cannot find 'Arma 3' folder in MyDocuments.");
                    return "";
                }

                prpath = Path.Combine(mydocument, "Arma 3");

                // check if Profile name exists in Arma 3 folder to identify the correct profile path
                if (File.Exists(Path.Combine(prpath, System.Web.HttpUtility.UrlEncode(profilename).Replace(".", "%2e") + ".Arma3Profile")))
                {
                    // find missions folder
                    if (!Directory.Exists(Path.Combine(prpath, "missions")))
                    {
                        lastException = new DirectoryNotFoundException("Cannot find 'missions' folder in Arma profile folder.");
                        return "";
                    }

                    // safe profile path
                    return Path.Combine(prpath, "missions");

                }

                // check other profile pathes
                prpath = Path.Combine(mydocument, "Arma 3 - Other Profiles");
                if (!Directory.Exists(prpath))
                {
                    lastException = new DirectoryNotFoundException("Cannot find 'Arma 3 - Other Profiles' folder in MyDocuments folder.");
                    return "";
                }

                DirectoryInfo dirinfos = new DirectoryInfo(prpath);
                foreach (DirectoryInfo dirinfo in dirinfos.GetDirectories())
                {
                    // check if Profile name exists in Arma 3 folder to identify the correct profile path
                    if (File.Exists(Path.Combine(dirinfo.FullName, System.Web.HttpUtility.UrlEncode(profilename).Replace(".", "%2e") + ".Arma3Profile")))
                    {
                        // find missions folder
                        if (!Directory.Exists(Path.Combine(dirinfo.FullName, "missions")))
                        {
                            lastException = new DirectoryNotFoundException("Cannot find 'missions' folder in Arma profile folder:" + dirinfo.FullName);
                            return "";
                        }

                        // safe profile path
                        return Path.Combine(dirinfo.FullName, "missions");

                    }
                }

                lastException = new DirectoryNotFoundException("Cannot find profle folder for '" + profilename + "' ");
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
            return "";
        }

        /// <summary>
        /// Creates the Mission file if not exists
        /// </summary>
        /// <param name="WorldName"></param>
        /// <param name="MissionName"></param>
        /// <returns></returns>
        public static bool CreateMissionFolder(string armaprofilepath, string missionname, string worldname)
        {
            try
            {
                lastException = null;

                // check variable
                if (string.IsNullOrEmpty(armaprofilepath))
                {
                    lastException = new DirectoryNotFoundException("Arma profile path not found.");
                    return false;
                }

                if (!Directory.Exists(Path.Combine(armaprofilepath, missionname + "." + worldname)))
                {
                    Directory.CreateDirectory(Path.Combine(armaprofilepath, missionname + "." + worldname));
                }

                return true;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return false;
            }
        } //CreateMissionFolder


        /// <summary>
        /// Extract mission filename from given parameters
        /// </summary>
        /// <param name="armaprofilepath"></param>
        /// <param name="missionname"></param>
        /// <param name="worldname"></param>
        /// <returns></returns>
        public static string GetMissionFilename(string armaprofilepath, string missionname, string worldname)
        {
            try
            {
                string missionpath = "";
                string missionfile = "";

                lastException = null;

                // check variable
                if (string.IsNullOrEmpty(armaprofilepath))
                {
                    lastException = new DirectoryNotFoundException("Arma profile path not found.");
                    return "";
                }

                missionpath = Path.Combine(armaprofilepath, missionname + "." + worldname);
                if (!Directory.Exists(missionpath))
                {
                    lastException = new DirectoryNotFoundException("Mission directory not found.");
                    return "";
                }

                missionfile = Path.Combine(missionpath, "mission.sqm");
                if (!File.Exists(missionfile))
                {
                    lastException = new FileNotFoundException("Missionfile not found.");
                    return "";
                }

                return missionfile;
            }
            catch (Exception ex)
            {
                lastException = ex;
                return "";
            }
        }

    } // class
}
