using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQMGagagu.sqmfile
{
    public class Intel
    {
        // mission name
        public string briefingName { get; set; }
        // mission description
        public string overviewText { get; set; }
        
        public int year {get; set;}
        public int month {get; set;}
        public int day {get; set;}
        public int hour {get; set;}
        public int minute {get; set;}

        // overcast start 0.0 - 1.0
        public double startWeather {get; set;}
        // overcast forecasted start 0.0 - 1.0
        public double forecastWeather {get; set;}

        // start fog value 0.00 - 1.0, non existent by 0
        public double startFog {get; set;}
        // forecast fog value 0.00 - 1.0, non existent by 0
        public double forecastFog {get; set;}

        // start fog decay value 0.00 - 1.0
        public double startFogDecay { get; set; }
        // forecast fog decay value 0.00 - 1.0
        public double forecastFogDecay { get; set; }

        // is 1 when rain is manual, non existent by 0
        public int rainForced {get; set;}
        // start rain value 0.00 - 1.0, non existent by 0, is 0 by rainForced=0
        public double startRain {get; set;}
        //forecast rain value 0.00 - 1.0, non existent by 0, is 0 by rainForced=0
        public double forecastRain {get; set;}

        // is 1 when lightning is manual, non existent by 0
        public int lightningsForced { get; set; }
        // start lightning value 0.00 - 1.0, non existent by 0, is 0 by lightningsForced=0
        public double startLightnings { get; set; }
        //forecast lightning value 0.00 - 1.0, non existent by 0, is 0 by lightningsForced=0
        public double forecastLightnings { get; set; }

        // is 1 when waves is manual, non existent by 0
        public int wavesForced { get; set; }
        // start waves value 0.00 - 1.0, non existent by 0, is 0 by wavesForced=0
        public double startWaves { get; set; }
        //forecast waves value 0.00 - 1.0, existent by 0 !!!, is 0 by wavesForced=0
        public double forecastWaves { get; set; }

        // start wind value 0.00 - 1.0
        public double startWind { get; set; }
        //forecast wind value 0.00 - 1.0
        public double forecastWind { get; set; }

        // is 1 when wind is manual, non existent by 0
        public int windForced { get; set; }
        // start gust value 0.00 - 1.0, non existent by 0, is 0 by windForced=0
        public double startGust { get; set; }
        //forecast gust value 0.00 - 1.0, existent by 0 !!!, is 0 by windForced=0
        public double forecastGust { get; set; }
        // start wind dir value 0.00 - 365, non existent by 0, is 0 by windForced=0
        public double startWindDir { get; set; }
        //forecast wind dir value 0.00 - 365, existent by 0 !!!, is 0 by windForced=0
        public double forecastWindDir { get; set; }

        // Time of weather change in seconds, non existent by 0
        public double timeOfChanges { get; set; }


         //independents are friensly to
         // nobody: resistanceWest=0;
         // bluefor: nothing
         // oppfor: resistanceWest=0; resistanceEast=1;
         // everybody: resistanceEast=1;
        public string IndFriendlyTo { get; set; }

		        /// <summary>
        /// creates the class Vehicles string for export to file
        /// </summary>
        /// <returns>vehicle class string</returns>
        public string ToClassString(int tabulators)
        {
            StringBuilder retval = new StringBuilder();

            string tabul = "";
            for (int x = 0; x < tabulators; x++)
            {
                tabul += "\t";
            }

            retval.AppendLine(tabul + "class Intel");
            retval.AppendLine(tabul + "{");

            if(!string.IsNullOrEmpty(briefingName))
                retval.AppendLine(tabul + "\tbriefingName=\"" + briefingName + "\";");

            if (!string.IsNullOrEmpty(overviewText))
                retval.AppendLine(tabul + "\toverviewText=\"" + overviewText + "\";");

            retval.AppendLine(tabul + "\tyear=" + year.ToString() + ";");
            retval.AppendLine(tabul + "\tmonth=" + month.ToString() + ";");
            retval.AppendLine(tabul + "\tday=" + day.ToString() + ";");
            retval.AppendLine(tabul + "\thour=" + hour.ToString() + ";");
            retval.AppendLine(tabul + "\tminute=" + minute.ToString() + ";");

            retval.AppendLine(tabul + "\tstartWeather=" + startWeather.ToString().Replace(",",".") + ";");
            retval.AppendLine(tabul + "\tforecastWeather=" + forecastWeather.ToString().Replace(",", ".") + ";");

            if(startFog>0)
                retval.AppendLine(tabul + "\tstartFog=" + startFog.ToString().Replace(",", ".") + ";");

            if (forecastFog > 0)
                retval.AppendLine(tabul + "\tforecastFog=" + forecastFog.ToString().Replace(",", ".") + ";");

            retval.AppendLine(tabul + "\tstartFogDecay=" + startFogDecay.ToString().Replace(",", ".") + ";");
            retval.AppendLine(tabul + "\tforecastFogDecay=" + forecastFogDecay.ToString().Replace(",", ".") + ";");

            if (rainForced > 0)
            {
                retval.AppendLine(tabul + "\trainForced=" + rainForced.ToString().Replace(",", ".") + ";");
                if (startRain > 0)
                    retval.AppendLine(tabul + "\tstartRain=" + startRain.ToString().Replace(",", ".") + ";");
                if (forecastRain > 0)
                    retval.AppendLine(tabul + "\tforecastRain=" + forecastRain.ToString().Replace(",", ".") + ";");
            }

            if (lightningsForced > 0)
            {
                retval.AppendLine(tabul + "\tlightningsForced=" + lightningsForced.ToString().Replace(",", ".") + ";");
                if (startLightnings > 0)
                    retval.AppendLine(tabul + "\tstartLightnings=" + startLightnings.ToString().Replace(",", ".") + ";");
                if (forecastLightnings > 0)
                    retval.AppendLine(tabul + "\tforecastLightnings=" + forecastLightnings.ToString().Replace(",", ".") + ";");
            }

            if (wavesForced > 0)
            {
                retval.AppendLine(tabul + "\twavesForced=" + wavesForced.ToString().Replace(",", ".") + ";");
                if (startWaves > 0)
                    retval.AppendLine(tabul + "\tstartWaves=" + startWaves.ToString().Replace(",", ".") + ";");
            }
            retval.AppendLine(tabul + "\tforecastWaves=" + forecastWaves.ToString().Replace(",", ".") + ";");

            retval.AppendLine(tabul + "\tstartWind=" + startWind.ToString().Replace(",", ".") + ";");
            retval.AppendLine(tabul + "\tforecastWind=" + forecastWind.ToString().Replace(",", ".") + ";");

            if (windForced > 0)
            {
                retval.AppendLine(tabul + "\twindForced=" + windForced.ToString().Replace(",", ".") + ";");
                if (startGust > 0)
                    retval.AppendLine(tabul + "\tstartGust=" + startGust.ToString().Replace(",", ".") + ";");
                if (forecastGust > 0)
                    retval.AppendLine(tabul + "\tforecastGust=" + forecastGust.ToString().Replace(",", ".") + ";");

                if (startWindDir > 0)
                    retval.AppendLine(tabul + "\tstartWindDir=" + startWindDir.ToString().Replace(",", ".") + ";");
                if (forecastWindDir > 0)
                    retval.AppendLine(tabul + "\tforecastWindDir=" + forecastWindDir.ToString().Replace(",", ".") + ";");
            }

            retval.AppendLine(tabul + "\ttimeOfChanges=" + timeOfChanges.ToString().Replace(",", ".") + ";");

            if (!string.IsNullOrEmpty(IndFriendlyTo))
            {
                switch (IndFriendlyTo.ToUpper())
                {
                    case "NOBOBY":
                        retval.AppendLine(tabul + "\tresistanceWest=0;");
                        break;
                    case "OPPFOR":
                        retval.AppendLine(tabul + "\tresistanceWest=0;");
                        retval.AppendLine(tabul + "\tresistanceEast=1;");
                        break;
                    case "EVERYBODY":
                        retval.AppendLine(tabul + "\tresistanceEast=1;");
                        break;
                }
            }


            retval.AppendLine(tabul + "};");

            return retval.ToString();
        }
    }
}
