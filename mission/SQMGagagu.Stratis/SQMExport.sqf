// ********************************************************************************
// *** created by A. Eckers (gagagu)
// *** 01/2014
// ********************************************************************************

// ATTENTION: I'm not responsible for data loose. Please backup your data !!!

_objID=0;

// Initialize sqm export with method "InitExport"
// parameters:
// WorldName = Name of the map (e.g. Stratis or Altis)
// MissionName = Name of the mission (same as name of directory for saving file)
// ProfileName = Name of the player profile (needs to find the save directory)
"Arma2Net.Unmanaged" callExtension "SQMExporter [InitExport, Stratis, SQMExport, gagagu]";

// also usable is following method. It will be first import all stuff in to the class so the export
// is able to overwrite specific parts
// "Arma2Net.Unmanaged" callExtension "SQMExporter [InitExportFirstImport, Stratis, SQMExport, gagagu]";

// As first entry in every sqm file is a version number.
// I guess it's to determine from which ARMA version the mission file is (ARMA 1,2,3)
// The actual version number is '12'
"Arma2Net.Unmanaged" callExtension "SQMExporter [SetVersion, '12']";

// Every mission sqm consists of four classes, called:
// 1 = Mission
// 2 = Intro
// 3 = OutroWin
// 4 = OutroLoose

// All four classes have the same class structure but different content.
// The Mission class contains the main mission parameters and objects.
// I don't really know for what the other classes are but i guess the 
// names of these classes tells enought.

// In only show the Mission class in detail, all other classes are
// filled with minimum data.

// *************************************************************************
// ***************************** class mission *****************************
// *************************************************************************

// *** AddOns and AddOnsAuto
// The first entries in the mission class are 
// the arrays: AddOns and AddOnsAuto
// It seems that these arrays contains the used classes for the objects
// used in the mission

// Example List
_addons=["a3_map_stratis",
		"A3_Characters_F_BLUFOR",
		"A3_Characters_F_OPFOR",
		"A3_Characters_F_INDEP",
		"A3_Characters_F_Civil",
		"A3_Animals_F_Rabbit",
		"A3_Structures_F_Civ_Constructions",
		"A3_Air_F_Heli_Light_01",
		"A3_Soft_F_MRAP_01",
		"A3_Boat_F_Boat_Armed_01",
		"a3_soft_f_gamma_offroad",
		"a3_characters_f_beta",
		"A3_Modules_F_HC"];

// Export addons to mission AddOns class
for "_i" from 0 to (count _addons) - 1  do {
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOns','%1','%2']", 'mission', _addons select _i];
};

// Export addons to mission AddOnsAuto class
for "_i" from 0 to (count _addons) - 1  do {
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOnsAuto','%1','%2']", 'mission', _addons select _i];
};

//*** RamdomSeed
// The next entry is RandomSeed
// It's a random number value and different for every four classes (mission, intro, outrowin, outroloose)
// You can set it on two different ways. First you can set the number directly.
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetRandomSeed','%1','%2']", 'mission', '3538210'];

// or you can let the system create a randomly number.
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['CreateRandomSeed','%1']", 'mission'];

// *** Intel class
// This class contains all general parameters for the mission like time, weather, description, name, etc.
// following parameters are possible:

// 'briefingName' 		the name of the mission
// 'overviewText' 		mission description text
// 'year' 				self explained
// 'month' 				self explained
// 'day' 				self explained
// 'hour' 				self explained
// 'minute' 			self explained
// 'startWeather' 		overcast start 0.0 - 1.0
// 'forecastWeather'	overcast forecasted start 0.0 - 1.0
// 'startFog'			start fog value 0.00 - 1.0, non existent by 0
// 'forecastFog'		forecast fog value 0.00 - 1.0, non existent by 0
// 'startFogDecay'		start fog decay value 0.00 - 1.0
// 'forecastFogDecay' 	forecast fog decay value 0.00 - 1.0
// 'rainForced' 		is 1 when rain is manual, non existent by 0
// 'startRain'			start rain value 0.00 - 1.0, non existent by 0, is 0 by rainForced=0
// 'forecastRain'		forecast rain value 0.00 - 1.0, non existent by 0, is 0 by rainForced=0
// 'lightningsForced'	is 1 when lightning is manual, non existent by 0
// 'startLightnings'	start lightning value 0.00 - 1.0, non existent by 0, is 0 by lightningsForced=0
// 'forecastLightnings'	forecast lightning value 0.00 - 1.0, non existent by 0, is 0 by lightningsForced=0
// 'wavesForced'		is 1 when waves is manual, non existent by 0
// 'startWaves'			start waves value 0.00 - 1.0, non existent by 0, is 0 by wavesForced=0
// 'forecastWaves'		forecast waves value 0.00 - 1.0, existent by 0 !!!, is 0 by wavesForced=0
// 'startWind'			start wind value 0.00 - 1.0
// 'forecastWind'		forecast wind value 0.00 - 1.0
// 'windForced'			is 1 when wind is manual, non existent by 0
// 'startGust'			start gust value 0.00 - 1.0, non existent by 0, is 0 by windForced=0
// 'forecastGust'		forecast gust value 0.00 - 1.0, existent by 0 !!!, is 0 by windForced=0
// 'startWindDir'		start wind dir value 0.00 - 365, non existent by 0, is 0 by windForced=0
// 'forecastWindDir'	forecast wind dir value 0.00 - 365, existent by 0 !!!, is 0 by windForced=0
// 'timeOfChanges'		Time of weather change in seconds, non existent by 0
// 'IndFriendlyTo'		independents are friendly to: nobody, bluefor, oppfor, everybody
		 
// All parameters can set by the SetIntelDataValueByName command.
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: name of parameter
// Parameter: value for parameter

"Arma2Net.Unmanaged" callExtension "SQMExporter [SetIntelDataValueByName, mission, briefingName, SQMExport]";
"Arma2Net.Unmanaged" callExtension "SQMExporter [SetIntelDataValueByName, mission, overviewText, Sqm Export test mission]";
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','year', 2035];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','month', 7];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','day', 6];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','hour', 12];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','minute', 0];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','startWeather', 0.29];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','startWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','startWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','forecastWeather', 0.3];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','forecastWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','forecastWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','forecastLightnings', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','startFogDecay', 0.013];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'mission','forecastFogDecay', 0.013];


// *** Groups
// The groups class contains all group able objects except from side 'Ambient life' an 'empty'
// A group can consists of one object (like player or logical) or it can be a group (like rifle squad)

// To delete all group items you can use the DeteleAllGroups method. 
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// sample:
// "Arma2Net.Unmanaged" callExtension "SQMExporter [DeleteAllGroups, mission]";

// To fill a group you have first to create on with AddNewGroup
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Side of group: WEST,EAST,GUER,CIV or LOGIC
// as return you will get the id of group (needed to fill with objects), -1 on error
// sample:
_grp =  "Arma2Net.Unmanaged" callExtension "SQMExporter [AddNewGroup, mission, WEST]";

// Every group can contain (one or more) objects (called vehicles; "don't confuse with real vehicles, it's more than that)
// and the appropriate waypoints (when existent for these group)
if((parseNumber _grp)>=0) then
{
	// If needed you can delete all vehicles in these group with
	// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['DeteteAllVehiclesInGroupByID','%1','%2']", 'mission', _grp];
	
	// You can add a new vehicle to these group with AddNewVehicleToGroupByID
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to insert the vehicle
	// as return you will get the id of the vehicle (needed to fill with parameters)
	_vhc = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewVehicleToGroupByID','%1','%2']", 'mission', _grp];
	// You can set the position of these vehicle with SetGroupVehiclePosition
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to insert the vehicle
	// Parameter: Id of vehicle which you want to set the position
	// Parameter: X
	// Parameter: Y
	// Parameter: Z
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehiclePosition','%1','%2','%3','%4','%5','%6']", 'mission', _grp, _vhc, 1787.7959, 5.5, 5742.2236];
	// All other vehicle parameters can set with SetGroupVehicleValueByName
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to insert the vehicle
	// Parameter: Id of vehicle which you want to set the parameter
	// Parameter: name of parameter
	// Parameter: value for parameter
	
	// possible parameters are:
    // 'id' every vehicle object has a unique id, an continuous integer number 
	// 'vehicle' object class name of object
	// 'side' values: WEST,EAST,GUER,CIV,LOGIC,AMBIENT LIFE,EMPTY
	// 'placement' placement radius (double)
	// 'azimut' direction radius 0 - 365 (double)
	// 'offsetY' offset in y direction (double)
	// 'special' values: NONE, CARGO, FLY, IN FORMATION
	// 'player' values: PLAY CDG, PLAYER COMMANDER, NON PLAYABLE
	// 'str_lock' values: UNLOCKED, LOCKED, LOCKEDPLAYER, DEFAULT
	// 'rank' values: CORPORAL, SERGEANT, LIEUTENANT, CAPTAIN, MAJOR, COLONEL, PRIVATE
	// 'age' values: ACTUAL, 5 MIN,10 MIN,15 MIN,30 MIN,60 MIN,120 MIN, UNKNOWN
	// 'text' Name of object, could be empty
	// 'init' Initialisation code of object, could be empty
	// 'description' description text of object, could be empty
	// 'skill' 0.0 ... 1.0  the skill of the object (double)
	// 'health' 0.0 ... 1.0  the health of the object (double)
	// 'fuel' 0.0 ... 1.0  the fuel of the object (double)
	// 'ammo' 0.0 ... 1.0  the ammo of the object (double)
	// 'presence' 0.0 ... 1.0  the presence of the object (double)
    // 'presenceCondition' condition of presence, not existent by true
	// 'leader' leader of group (0 or 1)
	
	// samples:
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'age', 'ACTUAL'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'id', _objID];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'side', 'WEST'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'vehicle', 'B_Soldier_F'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'player', 'PLAYER COMMANDER'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'leader', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'skill', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'health', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'fuel', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'ammo', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'presence', 1];
};

_objID = _objID + 1;

// Samples (second group with two vehicles):
_grp =  "Arma2Net.Unmanaged" callExtension "SQMExporter [AddNewGroup, mission, EAST]";
if((parseNumber _grp)>=0) then
{
	_vhc = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewVehicleToGroupByID','%1','%2']", 'mission', _grp];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehiclePosition','%1','%2','%3','%4','%5','%6']", 'mission', _grp, _vhc, 1785.7566,5.5,5656.7075];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'age', 'ACTUAL'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'id', _objID];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'side', 'EAST'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'vehicle', 'O_Soldier_F'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'player', 'NON PLAYABLE'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'leader', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'skill', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'health', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'fuel', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'ammo', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'presence', 1];
	
	_objID = _objID + 1;
	
	_vhc = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewVehicleToGroupByID','%1','%2']", 'mission', _grp];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehiclePosition','%1','%2','%3','%4','%5','%6']", 'mission', _grp, _vhc, 1796.7566,5.5,5656.7075];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'age', 'ACTUAL'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'id', _objID];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'side', 'EAST'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'vehicle', 'O_Soldier_F'];
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'player', 'NON PLAYABLE'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'leader', 0];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'skill', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'health', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'fuel', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'ammo', 1];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupVehicleValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _vhc, 'presence', 1];	
	
	_objID = _objID + 1;
	
	// *** Waypoints
	// You can manage waypoints to the created group:
	
	// If needed you can delete all waypoints in these group with
	// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['DeteteAllWaypointsInGroupByID','%1','%2']", 'mission', _grp];
	// You can add a new waypoint to these group with AddNewWaypointToGroupByID
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to insert the waypoint
	// as return you will get the id of the waypoint (needed to fill with parameters)
	_wp = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewWaypointToGroupByID','%1','%2']", 'mission', _grp];

	// You can set the position of these waypoint with SetGroupWaypointPosition
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to insert the waypoint
	// Parameter: Id of waypoint which you want to set the position
	// Parameter: X
	// Parameter: Y
	// Parameter: Z
    "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointPosition','%1','%2','%3','%4','%5','%6']", 'mission', _grp, _wp, 1838.651,5.5,5666.2773];
	// All other waypoint parameters can set with SetGroupWaypointValueByName
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to insert the waypoint
	// Parameter: Id of waypoint which you want to set the parameter
	// Parameter: name of parameter
	// Parameter: value for parameter
	
	// possible parameters are:
	// 'name' Name of waypoint
    // 'type' Type of waypoint: MOVE,DESTROY,GETIN,SAD,JOIN,LEADER,GETOUT,CYCLE,LOAD,UNLOAD,TR UNLOAD,HOLD,SENTRY,GUARD,TALK,SCRIPTED,SUPPORT,GETIN NEAREST,DISMISS,LOITER
	// 'placement' Placement Radius (double)
	// 'completitionRadius' Completition Radius (double)
	// 'timeoutMin' Minimum Timeout (double)
	// 'timeoutMid' Mid Timeout (double)
	// 'timeoutMax' Maximum Timeout (double)
	// 'text' Waypoint Text
	// 'description' Waypoint description
	// 'combatMode' "NO CHANGE", "BLUE" (never fire), "GREEN" (hold fire), "WHITE" (hold fire engage at will), "YELLOW" (open fire), "RED" (open fire, engage at will)
	// 'formation' NO CHANGE, COLUMN, STAG COLUMN,WEDGE,ECH LEFT,ECH RIGHT,VEE,LINE,DIAMOND,FILE
	// 'speed' NO CHANGE, LIMITED, NORMAL, FÙLL
	// 'combat' NO CHANGE, CARELESS,SAFE,AWARE,COMBAT,STEALTH
	// 'expCond' default: true; 
	// 'expActiv' On Activation Script
	// 'script' Script for waypoint
	// 'showWP' NEVER, CADET, ALWAYS
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'showWP', 'NEVER'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'name', 'wp1'];
    // *** Effects
	// You are able to add effects to a waypoint by using the SetGroupWaypointEffectValueByName
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of group which you want to set the waypoint
	// Parameter: Id of waypoint which you want to set the effect parameter
	// Parameter: name of parameter
	// Parameter: value for parameter
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointEffectValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'voice', 'Alarm'];

	// possible parameters are:
    // 'condition' sets the condition of the effect
    // 'sound' set the anonymous sound effect
    // 'voice' set the voice sound effect
    // 'soundEnv' set the environment sound effect
    // 'soundDet' set the trigger sound effect
    // 'track' set the Track sound effect
    // 'titleEffect' set the special effect, not existent by PLAIN
    // 'titleType' Sets the Type (OBJECT,RES,TEXT), non Existent by NONE
    // 'title' The Text of the Title Type, not existent by titleType=NONE
		
	//Samples:
	_wp = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewWaypointToGroupByID','%1','%2']", 'mission', _grp];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointPosition','%1','%2','%3','%4','%5','%6']", 'mission', _grp, _wp, 1842.6024,5.5,5608.3062];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'showWP', 'NEVER'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'name', 'wp2'];	

	_wp = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewWaypointToGroupByID','%1','%2']", 'mission', _grp];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointPosition','%1','%2','%3','%4','%5','%6']", 'mission', _grp, _wp, 1810.3328,5.5,5572.0747];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'showWP', 'NEVER'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetGroupWaypointValueByName','%1','%2','%3','%4','%5']", 'mission', _grp, _wp, 'name', 'wp3'];		
};

// *** Vehicles            
// used only for "Ablient life" and "Empty" side objects, not groupable, no waypoints
// Same options like a vehicle object in the groups class
// If needed you can delete all vehicles with
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['DeleteAllVehicles','%1']", 'mission'];

// create a new vehicle with AddNewVehicle
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Side AMBIENT LIFE or EMPTY
_vhc1 = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewVehicle','%1', '%2']", 'mission', 'AMBIENT LIFE'];

// You can set the position of these vehicle with SetVehiclePosition
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Id of vehicle
// Parameter: X
// Parameter: Y
// Parameter: Z
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehiclePosition','%1','%2','%3','%4','%5']", 'mission', _vhc1, 1647.1359,5.5,5498.0913];

// All other vehicle parameters can set with SetVehicleValueByName
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Id of vehicle which you want to set the parameter
// Parameter: name of parameter
// Parameter: value for parameter
// !! for parameter names list see group vehicle parameter list (above)	!!
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'id', _objID];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'age', 'ACTUAL'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'vehicle', 'Rabbit_F'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'leader', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'skill', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'health', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'fuel', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'ammo', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'presence', 1];	
_objID = _objID + 1;

// another sample:
_vhc1 = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewVehicle','%1', '%2']", 'mission', 'EMPTY'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehiclePosition','%1','%2','%3','%4','%5']", 'mission', _vhc1, 1705.3921,5.5,5413.7266];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'id', _objID];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'age', 'ACTUAL'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'vehicle', 'B_Heli_Light_01_F'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'leader', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'skill', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'health', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'fuel', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'ammo', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetVehicleValueByName','%1','%2','%3','%4']", 'mission', _vhc1, 'presence', 1];	
_objID = _objID + 1;
	
// *** Markers
// If needed you can delete all markers with
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['DeleteAllMarkers','%1']", 'mission'];

// create a new Marker with AddNewMarker
// Parameter: name of parent class: mission, intro, outrowin, outroloose
_mar1 = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewMarker','%1']", 'mission'];

// You can set the position of these marker with SetMarkerPosition
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Id of Marker
// Parameter: X
// Parameter: Y
// Parameter: Z
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerPosition','%1','%2','%3','%4','%5']", 'mission', _mar1, 1774.282,5.5,5418.3354];

// All other marker parameters can set with SetMarkerValueByName
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Id of marker which you want to set the parameter
// Parameter: name of parameter
// Parameter: value for parameter
// Following parameters are possible:
// 'name' Name of marker
// 'text' markers text
// 'markerType' values: RECTANGLE, ELLIPSE or ICON
// 'colorName' name of color sample: "ColorBrown"  (See Editor for mode)
// 'type' only existent by markerType=ICON (name of icon) default: Empty
// 'a' axis a (double)
// 'b' axis b (double)
// 'angle' (double)

"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar1, 'name', 'testmarker'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar1, 'text', 'markertext'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar1, 'type', 'Empty'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar1, 'colorName', 'ColorBrown'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar1, 'angle', 12];	

// another sample:
_mar2 = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewMarker','%1']", 'mission'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerPosition','%1','%2','%3','%4','%5']", 'mission', _mar2, 1759.8104,5.5,5340.436];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar2, 'name', 'test'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar2, 'markerType', 'ELLIPSE'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar2, 'type', 'Empty'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar2, 'a', 20];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetMarkerValueByName','%1','%2','%3','%4']", 'mission', _mar2, 'b', 20];

// *** Sensors
// If needed you can delete all sensors with
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['DeleteAllSensors','%1']", 'mission'];

// create a new sensor with AddNewSensor
// Parameter: name of parent class: mission, intro, outrowin, outroloose
_sen1 = "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['AddNewSensor','%1']", 'mission'];

// You can set the position of these sensor with SetSensorPosition
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Id of sensor
// Parameter: X
// Parameter: Y
// Parameter: Z
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorPosition','%1','%2','%3','%4','%5']", 'mission', _sen1, 1638.9841,5.5,5212.7974];

// All other marker parameters can set with SetSensorValueByName
// Parameter: name of parent class: mission, intro, outrowin, outroloose
// Parameter: Id of sensor which you want to set the parameter
// Parameter: name of parameter
// Parameter: value for parameter
// Following parameters are possible:
// 'name' name of sensor
// 'text' sensor text
// 'rectangular' (int) is 1 by rectangle shape, not exitent by ellipse
// 'a'(double) AXIS A, not existent by 50
// 'b'(double) AXIS B, not existent by 50
// 'angle'(double) ANGLE not existent by 0
// 'interruptable' (int) values: 1=Timeout, 0=Countdown
// 'timeoutMin' (double) minimum timeout
// 'timeoutMid' (double) mid timeout
// 'timeoutMax' (double) maximum timeout
// 'type' sets the trigger type, values: (i have to find out)
// 'activationBy' sets the activation by, values: (i have to find out)
// 'repeating' Repeading Trigger, values: (i have to find out)
// 'activationType' Sets the Activation type, values: (i have to find out) default: PRESENT
// 'expCond' Extra conditions, values: (i have to find out) default: this
// 'expActiv' On Activation Script, not existent by null
// 'expDesactiv' On Deactivation Script, not existent by null
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'activationBy', 'GUER SEIZED'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'activationType', 'CIV D'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'interruptable', 1];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'type', 'LOOSE'];	
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'text', 'blabla'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'name', 'bla'];

    // *** Effects
	// You are able to add effects to a sensor by using the SetSensorEffectValueByName
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	// Parameter: Id of sensor which you want to set the effect parameter
	// Parameter: name of parameter
	// Parameter: value for parameter
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorEffectValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'titleType', 'TEXT'];
	"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetSensorEffectValueByName','%1','%2','%3','%4']", 'mission', _sen1, 'title', 'SplashBohemia'];
	// Same parameter list like waypoint effects
	
// *************************************************************************
// ***************************** class intro *****************************
// *************************************************************************
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOns','%1','%2']", 'intro', 'a3_map_stratis'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOnsAuto','%1','%2']", 'intro', 'a3_map_stratis'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['CreateRandomSeed','%1']", 'intro'];
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetRandomSeed','%1','%2']", 'intro', '3538211'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','year', 2035];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','month', 7];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','day', 6];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','hour', 12];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','minute', 0];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','startWeather', 0.29];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','startWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','startWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','forecastWeather', 0.3];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','forecastWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','forecastWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','forecastLightnings', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','startFogDecay', 0.013];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'intro','forecastFogDecay', 0.013];

// *************************************************************************
// ***************************** class outrowin *****************************
// *************************************************************************
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOns','%1','%2']", 'outrowin', 'a3_map_stratis'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOnsAuto','%1','%2']", 'outrowin', 'a3_map_stratis'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['CreateRandomSeed','%1']", 'outrowin'];
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetRandomSeed','%1','%2']", 'outrowin', '3538212'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','year', 2035];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','month', 7];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','day', 6];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','hour', 12];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','minute', 0];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','startWeather', 0.29];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','startWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','startWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','forecastWeather', 0.3];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','forecastWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','forecastWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','forecastLightnings', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','startFogDecay', 0.013];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outrowin','forecastFogDecay', 0.013];

// *************************************************************************
// ***************************** class outroloose *****************************
// *************************************************************************
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOns','%1','%2']", 'outroloose', 'a3_map_stratis'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetAddOnsAuto','%1','%2']", 'outroloose', 'a3_map_stratis'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['CreateRandomSeed','%1']", 'outroloose'];
// "Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetRandomSeed','%1','%2']", 'outroloose', '3538213'];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','year', 2035];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','month', 7];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','day', 6];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','hour', 12];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','minute', 0];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','startWeather', 0.29];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','startWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','startWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','forecastWeather', 0.3];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','forecastWind', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','forecastWaves', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','forecastLightnings', 0.1];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','startFogDecay', 0.013];
"Arma2Net.Unmanaged" callExtension format ["SQMExporter ['SetIntelDataValueByName','%1','%2','%3']", 'outroloose','forecastFogDecay', 0.013];

// *************************************************************************
// *************************************************************************
// *************************************************************************

// Finish the export with method "FinishExport"
// parameters:
// CeateBakFile = If set to true the existent file will renamed with .bak extension
"Arma2Net.Unmanaged" callExtension "SQMExporter [FinishExport, true]";

// hint for finish
Hint "Export SQM Data completed";