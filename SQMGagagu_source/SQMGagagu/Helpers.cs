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
     class Helpers
    {
        internal static Markers GetMarkers(String ClassName, SqmFile sqmfile, Exception lastException)
        {
            try
            {
                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        return sqmfile.mission.markers;
                    case "INTRO":
                        return sqmfile.intro.markers;
                    case "OUTROWIN":
                        return sqmfile.outrowin.markers;
                    case "OUTROLOOSE":
                        return sqmfile.outroloose.markers;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                return null;
            }
        } //GetMarkers

        internal static Intel GetIntel(String ClassName, SqmFile sqmfile, Exception lastException)
        {
            try
            {
                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        return sqmfile.mission.intel;
                    case "INTRO":
                        return sqmfile.intro.intel;
                    case "OUTROWIN":
                        return sqmfile.outrowin.intel;
                    case "OUTROLOOSE":
                        return sqmfile.outroloose.intel;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                return null;
            }
        } //GetIntel

        internal static Groups GetGroups(String ClassName, SqmFile sqmfile, Exception lastException)
        {
            try
            {
                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        return sqmfile.mission.groups;
                    case "INTRO":
                        return sqmfile.intro.groups;
                    case "OUTROWIN":
                        return sqmfile.outrowin.groups;
                    case "OUTROLOOSE":
                        return sqmfile.outroloose.groups;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                return null;
            }
        } //GetGroups

        internal static Vehicles GetVehicles(String ClassName, SqmFile sqmfile, Exception lastException)
        {
            try
            {
                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        return sqmfile.mission.vehicles;
                    case "INTRO":
                        return sqmfile.intro.vehicles;
                    case "OUTROWIN":
                        return sqmfile.outrowin.vehicles;
                    case "OUTROLOOSE":
                        return sqmfile.outroloose.vehicles;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                return null;
            }
        } //GetVehicles

        internal static Sensors GetSensors(String ClassName, SqmFile sqmfile, Exception lastException)
        {
            try
            {
                switch (ClassName.ToUpper())
                {
                    case "MISSION":
                        return sqmfile.mission.sensors;
                    case "INTRO":
                        return sqmfile.intro.sensors;
                    case "OUTROWIN":
                        return sqmfile.outrowin.sensors;
                    case "OUTROLOOSE":
                        return sqmfile.outroloose.sensors;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
                return null;
            }
        } //GetSensors

        internal static int StrToInt(String value)
        {
            try
            {
                int val = 0;
                if (int.TryParse(value, out val))
                    return val;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        internal static double StrToDouble(String value)
        {
            try
            {
                double val = 0;
                if (double.TryParse(value, out val))
                    return val;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

    }
}
