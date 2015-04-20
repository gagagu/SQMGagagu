// ********************************************************************************
// *** created by A. Eckers (gagagu)
// *** 01/2014
// ********************************************************************************

// ATTENTION: I'm not responsible for data loose. Please backup your data !!!

// See file SQMExport.sqf for detailed description of class system!


// start importet
_res="Arma2Net.Unmanaged" callExtension "SQMImporter [Import, Stratis, SQMExport, gagagu]";
if(_res == 'True') then
{
 // ***** get Main SQM Data
 
	_DataVersion = "Arma2Net.Unmanaged" callExtension "SQMImporter [GetVersion]";
	// get all AddOns with comma separated
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	_AddOns = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetAddOns','%1']", 'mission'];
	// get all AddOnsAuto with comma separated
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	_AddOnsAuto = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetAddOnsAuto','%1']", 'mission'];
	// get the random seed value
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	_RandomSeed = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetRandomSeed','%1']", 'mission'];
 
	// ***** get Intel Data
	// get Intel data complete in one comma separated list
	// Parameter: name of parent class: mission, intro, outrowin, outroloose
	_IntelData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetIntelDataAsStringList','%1']", 'mission'];
	// result parameter list order:
	// briefingName,overviewText,year,month,day,hour,minute,startWeather,forecastWeather
	// ,startFog,forecastFog,startFogDecay,forecastFogDecay,rainForced,startRain,forecastRain,lightningsForced
	// ,startLightnings,forecastLightnings,wavesForced,startWaves,forecastWaves,startWind,forecastWind
	// ,windForced,startGust,forecastGust,startWindDir,forecastWindDir,timeOfChanges,IndFriendlyTo
   
	// get separate single Intel Value
	_briefingName = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetIntelValueByName','%1','%2']", 'mission','briefingName'];
	_startWeather = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetIntelValueByName','%1','%2']", 'mission','startWeather'];
	_year = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetIntelValueByName','%1','%2']", 'mission','year'];
			
			
	// *****Groups
	
	// how many groups?
	_groups = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetGroupsCount','%1']", 'mission'];
	
	if((parseNumber _groups)>=0) then
	{
		for "_i" from 0 to ((parseNumber _groups)-1) do
		{
			_GroupSide = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetGroupSideByID','%1','%2']", 'mission', _i];
			// ***** Vehicles
			_VehGrCount = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehiclesCountFromGroupItem','%1','%2']", 'mission', _i];
			if((parseNumber _VehGrCount)>=0) then
			{
				for "_v" from 0 to ((parseNumber _VehGrCount)-1) do
				{
					// get complete data from group vehicle
					// Parameter: name of parent class: mission, intro, outrowin, outroloose
					// Parameter: id of group
					// Parameter: id of vehicle
					_VehGrData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemFromGroupByIDAsStringList','%1','%2','%3']", 'mission', _i, _v];
					// result parameter list order:
					// id,vehicle,side,position,placement,azimut,offsetY,special
					// ,player,str_lock,rank,age,text,init,description,skill,health,fuel,ammo,presence,presenceCondition,leader
					
					
					
					// get separate single group vehicle Value
					_vehicle = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission',_i, _v,'vehicle'];
					_age = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission', _i, _v,'age'];
					_skill = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission', _i, _v,'skill'];
					// position x,z,y
					_position = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission', _i, _v,'position'];
				};
			};
			
			// ***** Waypoints
			_WayGrCount = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointsCountFromGroupItem','%1','%2']", 'mission', _i];
			if((parseNumber _WayGrCount)>=0) then
			{
				for "_t" from 0 to ((parseNumber _WayGrCount)-1) do
				{
					// get complete data from group waypoint
					// Parameter: name of parent class: mission, intro, outrowin, outroloose
					// Parameter: id of group
					// Parameter: id of waypoint
					_WayGrData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemFromGroupByIDAsStringList','%1','%2','%3']", 'mission', _i, _t];
					// result parameter list order:
					// name,position,type,placement,completitionRadius,timeoutMin,timeoutMid
					// ,timeoutMax,text,description,combatMode,formation,speed,combat,expCond,expActiv,script,showWP
					
					// get separate single group waypoint Value
					_name = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission',_i, _t,'name'];
					_type = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission', _i, _t,'type'];
					_formation = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission', _i, _t,'formation'];
					// position x,z,y
					_position = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemParameterFromGroupByID','%1','%2','%3','%4']", 'mission', _i, _t,'position'];
					
					
					// *** Waypoint Effect
					// Parameter: name of parent class: mission, intro, outrowin, outroloose
					// Parameter: id of group
					// Parameter: id of waypoint
					_WayEffGrData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemEffectFromGroupByIDAsStringList','%1','%2','%3']", 'mission', _i, _t];
					// result parameter list order:
					// condition,sound,voice,soundEnv,soundDet,track,titleEffect,titleType,title
	
					// get separate single group waypoint effect Value
					_voice = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemEffectParameterFromGroupByID','%1','%2','%3','%4']", 'mission',_i, _t,'voice'];
					//_voice = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetWaypointItemEffectParameterFromGroupByID','%1','%2','%3','%4']", 'mission',1, 0,'voice'];
				};
			};			
		};
	};

	
	// ****** Vehicles
	// how many vehicles?
	_vhcls = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehiclesCount','%1']", 'mission'];
	
	if((parseNumber _vhcls)>=0) then
	{
		for "_v" from 0 to ((parseNumber _vhcls)-1) do
		{
			// get complete data from vehicle
			// Parameter: name of parent class: mission, intro, outrowin, outroloose
			// Parameter: id of vehicle
			_VehData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemAsStringList','%1','%2']", 'mission', _v];
			// result parameter list order:
			// id,vehicle,side,position,placement,azimut,offsetY,special
			// ,player,str_lock,rank,age,text,init,description,skill,health,fuel,ammo,presence,presenceCondition,leader

			
			// get separate single vehicle Value
			_vehicle = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameter','%1','%2','%3']", 'mission', _v,'vehicle'];
			_age = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameter','%1','%2','%3']", 'mission',  _v,'age'];
			_skill = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameter','%1','%2','%3']", 'mission',  _v,'skill'];
			// position x,z,y
			_position = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetVehicleItemParameter','%1','%2','%3']", 'mission',  _v,'position'];
		};
	};
	
	// ****** Markers
	// how many markers?
	_markers= "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetMarkersCount','%1']", 'mission'];

	if((parseNumber _markers)>=0) then
	{
		for "_v" from 0 to ((parseNumber _markers)-1) do
		{
			// get complete data from vehicle
			// Parameter: name of parent class: mission, intro, outrowin, outroloose
			// Parameter: id of marker
			_MarkData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetMarkersItemAsStringList','%1','%2']", 'mission', _v];
			// result parameter list order:
			// position,name,text,markerType,type,colorName,a,b,angle
			
			// get separate single marker Value
			_name = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetMarkersItemParameter','%1','%2','%3']", 'mission', _v,'name'];
			_markerType = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetMarkersItemParameter','%1','%2','%3']", 'mission',  _v,'markerType'];
			_type = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetMarkersItemParameter','%1','%2','%3']", 'mission',  _v,'type'];
			// position x,z,y
			_position = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetMarkersItemParameter','%1','%2','%3']", 'mission',  _v,'position'];
			
		};
	};	
	
	// ****** Sensors
	// how many sensors?
	_sensors= "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorsCount','%1']", 'mission'];
	
	if((parseNumber _sensors)>=0) then
	{
		for "_v" from 0 to ((parseNumber _sensors)-1) do
		{
			// get complete data from vehicle
			// Parameter: name of parent class: mission, intro, outrowin, outroloose
			// Parameter: id of sensor
			_SensorData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemAsStringList','%1','%2']", 'mission', _v];
			// result parameter list order:
			// position,name,text,rectangular,a,b,angle,interruptable,timeoutMin,timeoutMid,timeoutMax,
			// type,activationBy,repeating,activationType,expCond,expActiv,expDesactiv

			// get separate single sensor Value
			_name = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemParameter','%1','%2','%3']", 'mission', _v,'name'];
			_text = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemParameter','%1','%2','%3']", 'mission',  _v,'text'];
			_type = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemParameter','%1','%2','%3']", 'mission',  _v,'type'];
			// position x,z,y
			_position = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemParameter','%1','%2','%3']", 'mission',  _v,'position'];
	
			// *** Sensor Effect
			// Parameter: name of parent class: mission, intro, outrowin, outroloose
			// Parameter: id of sensor
			_SensEffData = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemEffectsAsStringList','%1','%2']", 'mission', _v];
			// result parameter list order:
			// condition,sound,voice,soundEnv,soundDet,track,titleEffect,titleType,title

			// get separate single sensor effect Value
			_title = "Arma2Net.Unmanaged" callExtension format ["SQMImporter ['GetSensorItemEffectParameter','%1','%2','%3']", 'mission',_v,'title'];	
		};
	};	
};

// clear all memory
"Arma2Net.Unmanaged" callExtension "SQMImporter [FinishImport]";

Hint "Import SQM Data completed";