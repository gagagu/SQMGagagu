using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

using SQMGagagu;
using SQMGagagu.sqmfile;
using SQMGagagu.sqmfile.datatypes;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private SqmFile sqmfile = new SqmFile();

        public Form1()
        {
            InitializeComponent();
        }


        #region "import mission string"
        private void btSQMImport_Click(object sender, EventArgs e)
        {
            SQMImporter inp = new SQMImporter();

            //string file = "";
            //file = @"C:\Users\eckers.alexander\Documents\Arma 3 - Other Profiles\gagagu\missions\EmptyMission.Stratis\mission.sqm";
            // file = @"C:\Users\eckers.alexander\Documents\Arma 3 - Other Profiles\gagagu\missions\demo_attachToWithMovement_0_6.Stratis\mission.sqm";
            //if(inp.Import(file)
            //{
            //    textBox1.Text = inp.ToClassString();
            //}

            if (inp.Import("Stratis", "SQMExport", "gagagu"))
            {
                textBox1.Text = inp.ToClassString();

                // get Briefing Name of misison class
                String briefingName = inp.GetIntelValueByName("mission", "briefingName");
                // get complete intel data of mission class as string
                IList<String> intData = inp.GetIntelDataAsStringList("mission");

                //// get year value of intel in mission class
                //int year = inp.GetIntelIntValueByName("mission", "year");
                //// get start weather value of intel in miccion class
                //Double startWeather = inp.GetIntelDoubleValueByName("mission", "startWeather");

                // get number of groups in mission class
                int groups = inp.GetGroupsCount("mission");
                if (groups > 0)
                {
                    // get side of specified group class in parent mission class
                    String side=inp.GetGroupSideByID("mission", "0");
                    
                    // get count of vehicle items in specified group class in parent mission class
                    int vcount = inp.GetVehiclesCountFromGroupItem("mission", "0");
                    if (vcount > 0)
                    {
                        IList<String> vItems= inp.GetVehicleItemFromGroupByIDAsStringList("mission", "0", "0");
                        String descr = inp.GetVehicleItemParameterFromGroupByID("mission","1", "1", "vehicle");
                    }


                    // get count of waypoint items in specified group class in parent mission class
                    int wcount = inp.GetWaypointsCountFromGroupItem("mission", "1");
                    if (wcount > 0)
                    {
                        IList<String> wItems = inp.GetWaypointItemFromGroupByIDAsStringList("mission", "1","0");
                        String descr = inp.GetWaypointItemParameterFromGroupByID("mission", "1", "0", "name");
                        String azi = inp.GetWaypointItemParameterFromGroupByID("mission", "1", "0", "leader");
                        String health = inp.GetWaypointItemParameterFromGroupByID("mission", "1", "0", "health");
                        //IList<String> effectItems = inp.GetWaypointItemEffectFromGroupByIDAsStringList("mission", "1", "1");
                    }

                }


                //vehicles
                int vehicles = inp.GetVehiclesCount("mission");
                if (vehicles > 0)
                {
                    IList<String> vItems = inp.GetVehicleItemAsStringList("mission", "0");

                    //String descr = inp.GetVehicleStringValueByName("mission", 0, "vehicle");
                    //int azi = inp.GetVehicleIntValueByName("mission", 0, "azimut");
                    //Double health = inp.GetVehicleIntValueByName("mission", 0, "health");
                }


                //markers
                int markers = inp.GetMarkersCount("mission");
                if (markers > 0)
                {
                    IList<String> mItems = inp.GetMarkersItemAsStringList("mission", "0");

                    String name = inp.GetMarkersItemParameter("mission","0", "name");
                    String angle = inp.GetMarkersItemParameter("mission", "0", "angle");
                }

                //Sensors
                int sensors = inp.GetSensorsCount("mission");
                if (sensors > 0)
                {
                    IList<String> sItems = inp.GetSensorItemAsStringList("mission", "0");

                    String name = inp.GetSensorItemParameter("mission", "0", "name");
                    String angle = inp.GetSensorItemParameter("mission", "0", "angle");

                    IList<String> effectItems = inp.GetSensorItemEffectsAsStringList("mission", "0");
                }

                bool ret=inp.FinishImport();
            }
            

        }
        #endregion

        #region "export mission string"
        private void btSQMExport_Click(object sender, EventArgs e)
        {
            SQMExporter exp = new SQMExporter();
            bool x = false;
            textBox1.Text = "";

            // init
            x = exp.InitExport("Stratis", "blablubb", "gagagu");
            textBox1.Text += "InitExport=" + x.ToString() + "\r\n";
            exp.SetIntelDataValueByName("mission", "briefingName", "test");
            exp.SetIntelDataValueByName("mission", "year", "2010");

            int grp=exp.AddNewGroup("mission", "WEST");
            exp.AddNewVehicleToGroupByID("mission", grp.ToString());

            exp.AddNewWaypointToGroupByID("mission", grp.ToString());
            exp.SetGroupWaypointPosition("mission", grp.ToString(), "0", "1", "1", "1");

            exp.SetGroupWaypointEffectValueByName("mission", grp.ToString(), "0", "voice", "Alarm");

            int sen=exp.AddNewSensor("mission");

            //x = exp.AddVehicleItem(2, "B_Heli_Light_01_F", "EMPTY", 1705.3921, 5.5, 5413.7266, 0.0, 0.0, 0.0, "IN FORMATION", "NON PLAYABLE", "DEFAULT", "PRIVATE", "ACTUAL", "", "", "", 1.0, 1.0, 1.0, 1.0, 1.0, "", 1);
            //textBox1.Text += "AddVehicleItem=" + x.ToString() + "\r\n";

            //x = exp.AddVehicleItem(3, "Rabbit_F", "AMBIENT LIFE", 1647.1359, 5.5, 5498.0913, 0.0, 0.0, 0.0, "IN FORMATION", "NON PLAYABLE", "DEFAULT", "PRIVATE", "ACTUAL", "", "", "", 1.0, 1.0, 1.0, 1.0, 1.0, "", 1);
            //textBox1.Text += "AddVehicleItem=" + x.ToString() + "\r\n";

            // write file
            x = exp.FinishExport(true);
            textBox1.Text += "FinishExport=" + x.ToString() + "\r\n";
        }
        #endregion

        #region "create mission string"
        private void btCreateMissionString_Click(object sender, EventArgs e)
        {
            var mission = sqmfile.mission;
            var intel = mission.intel;
            var vehicles = mission.vehicles;
            var markers = mission.markers;
            var sensors = mission.sensors;

            var intro = sqmfile.intro;
            var outrowin = sqmfile.outrowin;
            var outroloose = sqmfile.outroloose;

            sqmfile.version = "12"; // the version of the file "12" seems to be the actual value

            //********************************************************************
            //********************************************************************
            //********************************************************************
            // Class Mission (needed)
            //********************************************************************
            //********************************************************************
            //********************************************************************

            //********************************************************************
            // effect, used for groups and sensors
            //********************************************************************
            Effects effect = new Effects();                                 // defines the sensor effects
            effect.condition = "true";                                      // (string) condition, default="true"
            effect.sound = "";                                              // (string) ANONYMOUS Effect sound name
            effect.voice = "";                                              // (string) VOICE Effect sound name
            effect.soundEnv = "";                                           // (string) ENVIRONMENT Effect sound name
            effect.soundDet = "";                                           // (string) TRIGGER Effect sound name
            effect.track = "";                                              // (string) TRACK Effect sound
            effect.titleEffect = "PLAIN";                                   // (string) Effects: "PLAIN", "PLAIN DOWN","BLACK","BLACK FADED","BLACK OUT","BLACK IN","WHITE OUT","WHITE IN","PLAIN NOFADE"
            effect.titleType = "NONE";                                      // (string) Effect Type: "NONE","OBJECT","RES","TEXT"
            effect.title = "NONE";                                          // (string) Title, depended on titleType settings. Name of Ressource, Object or Text title, "NONE" by titleType="NONE"

            //********************************************************************
            // random Seed (needed)
            //********************************************************************
            mission.randomSeed = "3538210"; // (string) seems to be a random value

            //********************************************************************
            // addOns (needed)
            //********************************************************************
            // is not really clear for what this values are. "addOns" and "addOnsAuto" seems to contains the same values.
            // It looks like the class names of the used classes in mission (may be for preload?)
            // The first value is the class name of the map (stratis or altis)
            mission.addOns.Add("a3_map_stratis");
            mission.addOns.Add("A3_Characters_F_BLUFOR");
            mission.addOns.Add("A3_Characters_F_OPFOR");
            mission.addOns.Add("A3_Characters_F_INDEP");
            mission.addOns.Add("A3_Characters_F_Civil");
            mission.addOns.Add("A3_Animals_F_Rabbit");
            mission.addOns.Add("A3_Structures_F_Civ_Constructions");
            mission.addOns.Add("A3_Air_F_Heli_Light_01");
            mission.addOns.Add("A3_Soft_F_MRAP_01");
            mission.addOns.Add("A3_Boat_F_Boat_Armed_01");
            mission.addOns.Add("a3_soft_f_gamma_offroad");
            mission.addOns.Add("a3_characters_f_beta");
            mission.addOns.Add("A3_Modules_F_HC");

            //********************************************************************
            // addOnsAuto (needed)
            //********************************************************************
            mission.addOnsAuto.Add("a3_map_stratis");
            mission.addOnsAuto.Add("A3_Characters_F_BLUFOR");
            mission.addOnsAuto.Add("A3_Characters_F_OPFOR");
            mission.addOnsAuto.Add("A3_Characters_F_INDEP");
            mission.addOnsAuto.Add("A3_Characters_F_Civil");
            mission.addOnsAuto.Add("A3_Animals_F_Rabbit");
            mission.addOnsAuto.Add("A3_Structures_F_Civ_Constructions");
            mission.addOnsAuto.Add("A3_Air_F_Heli_Light_01");
            mission.addOnsAuto.Add("A3_Soft_F_MRAP_01");
            mission.addOnsAuto.Add("A3_Boat_F_Boat_Armed_01");
            mission.addOnsAuto.Add("a3_soft_f_gamma_offroad");
            mission.addOnsAuto.Add("a3_characters_f_beta");
            mission.addOnsAuto.Add("A3_Modules_F_HC");

            //********************************************************************
            // Intel (partitial needed)
            //********************************************************************            
            // needed, you have to fill this values for every mission!
            intel.briefingName = "Testmission";                         // (string) the name of the mission
            intel.overviewText = "Misssion description";                // (string) the mission description
            intel.year = 2035;                                          // (int) the year value (self explained)
            intel.month = 7;                                            // (int: 1-12) the month value
            intel.day = 6;                                              // (int: 1-31) the day value
            intel.hour = 12;                                            // (int: 1-24) the hour value
            intel.minute = 0;                                           // (int: 0-59) the minute value
            intel.startWeather = 0.84999996;                            // (double: 0.0 - 1.0) overcast start value
            intel.forecastWeather = 0.84999996;                         // (double: 0.0 - 1.0) overcast forecast value
            intel.startFogDecay = 0.013;                                // (double: 0.0 - 1.0) fog decay start value
            intel.forecastFogDecay = 0.013;                             // (double: 0.0 - 1.0) fog decay forecast value
            intel.forecastWaves = 0.34999999;                           // (double: 0.0 - 1.0) forecast waves (always needed)
            intel.startWind = 0.44999999;                               // (double: 0.0 - 1.0) wind start value
            intel.forecastWind = 0.50999999;                            // (double: 0.0 - 1.0) wind forecast value

            // they are not needed for a empty mission
            intel.startFog = 0.17;                                      // (double: 0.0 - 1.0) start fog value
            intel.forecastFog = 0.63;                                   // (double: 0.0 - 1.0) forecast fog value

            intel.rainForced = 1;                                       // (int: 0 or 1) is 1 when rain is manual otherwise 0
            intel.startRain = 0.28;                                     // (double: 0.0 - 1.0) start rain value
            intel.forecastRain = 0.32999998;                            // (double: 0.0 - 1.0) forecast rain value

            intel.lightningsForced = 1;                                 // (int: 0 or 1) is 1 when lightning is manual otherwise 0
            intel.startLightnings = 0.42999998;                         // (double: 0.0 - 1.0) start lightning value
            intel.forecastLightnings = 0.41999999;                      // (double: 0.0 - 1.0) forecast lightning value

            intel.wavesForced = 1;                                      // (int: 0 or 1) is 1 when waves is manual otherwise 0
            intel.startWaves = 0.35999998;                              // (double: 0.0 - 1.0) start waves value

            intel.windForced = 1;                                       // (int: 0 or 1) is 1 when wind is manual otherwise 0
            intel.startGust = 0.45999998;                               // (double: 0.0 - 1.0) start gust value
            intel.forecastGust = 0.63;                                  // (double: 0.0 - 1.0) forecast gust value
            intel.startWindDir = 110;                                   // (double: 0.0 - 360.0) start wind dir value
            intel.forecastWindDir = 85;                                 // (double: 0.0 - 360.0) forecast wind dir value

            intel.timeOfChanges = 8100;                                 // (int) time of weather change in seconds



            intel.IndFriendlyTo = "OPPFOR";                             // independents are friensly to
            // nobody
            // bluefor
            // oppfor
            // everybody

            //********************************************************************
            // groups
            //********************************************************************
            // all object except of side "Ambient life" and "Empty" are grouped together
            // a group can consists of one object (like player or logical) or it can be a group (like rifle squad)

            Groups_Item Gruppe1 = new Groups_Item();                                    // define a new group
            // this is the player object:
            Gruppe1.side = "WEST";                                                        // (string) side of group: "WEST","EAST","GUER","CIV","LOGIC"
            // **********************************************
            // Vehicles
            // You can add one or more vehicles (objects) to a group
            // **********************************************
            Gruppe1.vehicles.AddItem(0,                                              // (int) id of object (every object has its own id, a counter counting up)
                                        "B_Soldier_F",                                  // (string) class name of object ( in this sample the player "B_Soldier_F")
                                        "WEST",                                         // (string) the Side of the Object: WEST,EAST,GUER,CIV,LOGIC ( do not use AMBIENT LIFE and EMPTY here, use vehicle class instead)
                                        new SqmPosition(1787.7959, 5.5, 5742.2236),     // (SqmPosition) position of object (x,z,y)
                                        0.0,                                            // (double) placement radius of object
                                        0.0,                                            // (double: 0.0 - 360.0) AZIMUTH
                                        0.0,                                            // (double) Elevation
                                        "IN FORMATION",                                 // (string) SPECIAL values: "NONE", "CARGO", "FLY", "IN FORMATION"
                                        "PLAYER COMMANDER",                             // (string) CONTROL value: "PLAY CDG", "PLAYER COMMANDER", "NON PLAYABLE"
                                        "DEFAULT",                                      // (string) Vehicle Lock values: "UNLOCKED", "LOCKED", "LOCKEDPLAYER", "DEFAULT"
                                        "PRIVATE",                                      // (string) Rank values: "CORPORAL","SERGEANT","LIEUTENANT","CAPTAIN","MAJOR","COLONEL", "PRIVATE"
                                        "ACTUAL",                                       // (string) Info Age values: "ACTUAL", "5 MIN","10 MIN","15 MIN","30 MIN","60 MIN","120 MIN", "UNKNOWN"
                                        "",                                             // (string) Name of object, could be empty
                                        "",                                             // (string) Initialisation code of object, could be empty
                                        "",                                             // (string) description text of object, could be empty
                                        1.0,                                            // (double 0.0-1.0) the Skill of the object
                                        1.0,                                            // (double 0.0-1.0) the Health of the object
                                        1.0,                                            // (double 0.0-1.0) the Fuel of the object
                                        1.0,                                            // (double 0.0-1.0) the Ammo of the object
                                        1.0,                                            // (double 0.0-1.0) the Probably of presence
                                        "true",                                         // (string) condition of presence
                                        1);                                             // (int: 0 or 1) indicates the leader of the group; sould be hightes rank

            Gruppe1.vehicles.AddItem(1, "B_Boat_Armed_01_minigun_F", "WEST", new SqmPosition(1743.9027, 5.5, 5692.9346), 0.0, 234.2, 12.4, "IN FORMATION", "NON PLAYABLE", "DEFAULT", "PRIVATE", "ACTUAL", "", "", "", 1.0, 1.0, 1.0, 1.0, 1.0, "true", 0);
            // **********************************************
            // Waypoints
            // You can add one or more waypoints to a group
            // **********************************************
            Gruppe1.waypoints.AddItem("Waypointname", new SqmPosition(1811.3975, 5.5, 5801.8784),
                                        "MOVE",                                         // (string) Select Type: MOVE,DESTROY,GETIN,SAD,JOIN,LEADER,GETOUT,CYCLE,LOAD,UNLOAD,TR UNLOAD,HOLD,SENTRY,GUARD,TALK,SCRIPTED,SUPPORT,GETIN NEAREST,DISMISS,LOITER
                                        0,                                              // (double) Placement Radius
                                        0,                                              // (double) Completition Radius
                                        0,                                              // (double) Timeout Min
                                        0,                                              // (double) Timeout Mid
                                        0,                                              // (double) Timeout Max
                                        "",                                             // (string) name of waypoint
                                        "",                                             // (string) description of waypoint
                                        "NO CHANGE",                                    // (string) combat mode: "NO CHANGE","BLUE" (never fire), "GREEN" (hold fire), "WHITE" (hold fire engage at will), "YELLOW" (open fire), "RED" (open fire, engage at will)
                                        "NO CHANGE",                                    // (string) formation: NO CHANGE, COLUMN, STAG COLUMN,WEDGE,ECH LEFT,ECH RIGHT,VEE,LINE,DIAMOND,FILE
                                        "NO CHANGE",                                    // (string) speed: NO CHANGE, LIMITED, NORMAL, FÙLL
                                        "NO CHANGE",                                    // (string) behavior: NO CHANGE, CARELESS,SAFE,AWARE,COMBAT,STEALTH
                                        "true",                                         // (string) Extra conditions, default: "true"
                                        "",                                             // (string) On Activation Script
                                        "",                                             // (string) script
                                        "NEVER",                                        // (string) Show waypoint: NEVER, CADET, ALWAYS
                                        effect);

            mission.groups.AddItem(Gruppe1);                                            // add to group

            // second group consists of one logic item
            Groups_Item Gruppe2 = new Groups_Item();
            Gruppe2.side = "LOGIC";
            Gruppe2.vehicles.AddItem(2, "Logic", "LOGIC", new SqmPosition(1699.3835, 5.5, 5472.8838), 0.0, 0.0, 0.0, "IN FORMATION", "NON PLAYABLE", "DEFAULT", "PRIVATE", "ACTUAL", "", "", "logic1", 1.0, 1.0, 1.0, 1.0, 1.0, "", 1);
            mission.groups.AddItem(Gruppe2);

            //********************************************************************
            // vehicles
            //********************************************************************
            // used only for "Ablient life" and "Empty" side objects
            // Same options like a vehicle object in the groups class

            vehicles.AddItem(3, "Rabbit_F", "AMBIENT LIFE", new SqmPosition(1647.1359, 5.5, 5498.0913), 0.0, 0.0, 0.0, "IN FORMATION", "NON PLAYABLE", "DEFAULT", "PRIVATE", "ACTUAL", "", "", "", 1.0, 1.0, 1.0, 1.0, 1.0, "", 1);

            vehicles.AddItem(4, "B_Heli_Light_01_F", "EMPTY", new SqmPosition(1705.3921, 5.5, 5413.7266), 0.0, 0.0, 0.0, "IN FORMATION", "NON PLAYABLE", "DEFAULT", "PRIVATE", "ACTUAL", "", "", "", 1.0, 1.0, 1.0, 1.0, 1.0, "", 1);

            //********************************************************************
            // Markers
            //********************************************************************
            markers.AddItem(new SqmPosition(1632.6907, 5.5, 5392.8438),     // (SqmPosition) position of object (x,z,y)
                            "marker1",                                      // (string) name of marker
                            "",                                             // (string) text of marker
                            "RECTANGLE",                                    // (string) Type of marker values: "RECTANGLE", "ELLIPSE", "ICON"
                            "EMPTY",                                        // (string) name of Icon default "EMPTY"
                            "ColorBrown",                                   // (string) name of color
                            10,                                             // (double) axis a
                            10,                                             // (double) axis b
                            0);                                             // (double: 0.0 - 360.0) angle

            markers.AddItem(new SqmPosition(1621.0508, 5.5, 5332.8828), "marker2", "text", "ICON", "EMPTY", "ColorBrown", 1, 1, 0);

            //********************************************************************
            // Sensors
            //********************************************************************


            sensors.AddItem(new SqmPosition(1602.5696, 5.5, 5119.2383),     // (SqmPosition) position of object (x,z,y)
                                "sensor1",                                  // (string) name of sensor
                                "",                                         // (string) text of sensor
                                0,                                          // (int 0 or 1) 0 by rectangular, 1 by ellipse shape
                                50,                                         // (double) axis a
                                50,                                         // (double) axis b
                                0,                                          // (double: 0.0 - 360.0) angle
                                1,                                          // (int 0 or 1) 0 by countdown, 1 by timer
                                0,                                          // (double) timeout min
                                0,                                          // (double) timeout mid
                                0,                                          // (double) timeout max
                                "NONE",                                     // (string) Trigger Type values: "NONE", "EAST G","WEST G","GUER G","SWITCH","END1","END2","END3","END4","END5","END6","END7","END8","LOOSE"
                                "NONE",                                     // (string) activation values: "NONE","EAST","WEST","GUER","CIV","LOGIC","ANY","ALPHA","BRAVO","CHARLIE","DELTA","ECHO","FOXTROT","GOLF","HOTEL","INDIA","JULIET","EAST SEIZED","WEST SEIZED","GUER SEIZED"
                                0,                                          // (int 0 or 1) 0=once, 1=releatedly
                                "PRESENT",                                  // (string) values: "PRESENT","NOT PRESENT","WEST D", "EAST D","GUER D", "CIV D"
                                "this",                                     // (string) condision default "this"
                                "",                                         // (string) on activation script
                                "",                                         // (string) on deactivation script
                                effect);

            //********************************************************************
            //********************************************************************
            //********************************************************************
            // Class Intro (needed)
            //********************************************************************
            //********************************************************************
            //********************************************************************
            intro.randomSeed = "1238507";
            intro.intel.timeOfChanges = 1800.0002;
            intro.intel.startWeather = 0.30000001;
            intro.intel.startWind = 0.1;
            intro.intel.startWaves = 0.1;
            intro.intel.forecastWeather = 0.30000001;
            intro.intel.forecastWind = 0.1;
            intro.intel.forecastWaves = 0.1;
            intro.intel.forecastLightnings = 0.1;
            intro.intel.year = 2035;
            intro.intel.month = 7;
            intro.intel.day = 6;
            intro.intel.hour = 12;
            intro.intel.minute = 0;
            intro.intel.startFogDecay = 0.013;
            intro.intel.forecastFogDecay = 0.013;

            //********************************************************************
            //********************************************************************
            //********************************************************************
            // Class OutroWin (needed)
            //********************************************************************
            //********************************************************************
            //********************************************************************
            outrowin.randomSeed = "7089989";
            outrowin.intel.timeOfChanges = 1800.0002;
            outrowin.intel.startWeather = 0.30000001;
            outrowin.intel.startWind = 0.1;
            outrowin.intel.startWaves = 0.1;
            outrowin.intel.forecastWeather = 0.30000001;
            outrowin.intel.forecastWind = 0.1;
            outrowin.intel.forecastWaves = 0.1;
            outrowin.intel.forecastLightnings = 0.1;
            outrowin.intel.year = 2035;
            outrowin.intel.month = 7;
            outrowin.intel.day = 6;
            outrowin.intel.hour = 12;
            outrowin.intel.minute = 0;
            outrowin.intel.startFogDecay = 0.013;
            outrowin.intel.forecastFogDecay = 0.013;

            //********************************************************************
            //********************************************************************
            //********************************************************************
            // Class OutroLoose (needed)
            //********************************************************************
            //********************************************************************
            //********************************************************************
            outroloose.randomSeed = "5991089";
            outroloose.intel.timeOfChanges = 1800.0002;
            outroloose.intel.startWeather = 0.30000001;
            outroloose.intel.startWind = 0.1;
            outroloose.intel.startWaves = 0.1;
            outroloose.intel.forecastWeather = 0.30000001;
            outroloose.intel.forecastWind = 0.1;
            outroloose.intel.forecastWaves = 0.1;
            outroloose.intel.forecastLightnings = 0.1;
            outroloose.intel.year = 2035;
            outroloose.intel.month = 7;
            outroloose.intel.day = 6;
            outroloose.intel.hour = 12;
            outroloose.intel.minute = 0;
            outroloose.intel.startFogDecay = 0.013;
            outroloose.intel.forecastFogDecay = 0.013;

            this.textBox1.Text = sqmfile.ToClassString();
        }
        #endregion

        #region "Export Mission but Import before"
        private void btExportFirstImport_Click(object sender, EventArgs e)
        {
            SQMExporter exp = new SQMExporter();
            textBox1.Text = "";

            if (exp.InitExportFirstImport("Stratis", "EmptyMission", "gagagu"))
            {
                textBox1.Text += "Import finished \r\n";
                if (exp.FinishExport(true))
                    textBox1.Text += "export finished \r\n";
                else
                    textBox1.Text += "export failed: \r\n" + exp.GetLastException();
            }
            else
            {
                textBox1.Text += "export failed: \r\n" + exp.GetLastException();
            }

            textBox1.Text += "\r\nend";
        }
        #endregion
    }
}
