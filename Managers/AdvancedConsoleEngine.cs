using Il2CppSystem.Runtime.Serialization.Formatters.Binary;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
using Sons.Items.Core;
using Sons.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sons.Characters.VailWorldEventData;
namespace ImprovedDebugConsole.Managers
{
    public static class AdvancedConsoleEngine
    {
        public const string ArgOnOff = "On/Off";
        public const string ArgTrueFalse = "True/False";
        public const string ArgItem = "ItemName/ID";
        public const string ArgCharacter = "CharacterType";
        public const string ArgPrefab = "PrefabName";
        public const string ArgWeather = "WeatherType";
        public const string ArgWindIntensity = "Intensity/Off";
        public const string ArgStat = "StatName";
        public const string ArgCount = "Count";
        public const string ArgAmount = "Amount";
        public const string ArgFloat = "Float";
        public const string ArgFloatOff = "Float/Off";
        public const string ArgDistance = "Distance";
        public const string ArgInt = "Integer";
        public const string ArgString = "String";
        public const string ArgGameObject = "GameObjectName";
        public const string ArgTag = "Tag";
        public const string ArgLayer = "Layer";
        public const string ArgScene = "SceneName/Index";
        public const string ArgSeason = "Season";
        public const string ArgDifficulty = "Difficulty";
        public const string ArgGameMode = "GameMode";
        public const string ArgVirginia = "VirginiaState";
        public const string ArgWorldEvent = "WorldEventName";
        public const string ArgWorkScheduler = "TryCatchMode";
        public const string ArgTreeRadiusState = "TreeState";
        public const string ArgSpeakerMode = "SpeakerMode";
        public const string ArgAnimShowType = "AnimShowType";
        public const string ArgRadarType = "RadarType";
        public const string ArgForce = "Force";
        public const string ArgLifetime = "Lifetime";
        public const string ArgDamage = "Damage";
        public const string ArgGoto = "GotoPoint/Coordinates/GameObject";
        public const string ArgCloudProfile = "CloudProfile";
        public const string ArgLodType = "LODType";
        public const string ArgPostProcessing = "PostProcessType";
        public const string ArgWorldObjectState = "WorldObjectState";

        public struct CommandArg
        {
            public string Name;
            public string Type;
            public bool IsOptional;
            public string Label => IsOptional ? $"[{Name}]" : $"<{Name}>";
        }

        public static Dictionary<string, CommandArg[]> CommandSignatures = new(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, List<string>> ArgumentPools = new(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, string> CommandDescriptions = new(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, string[]> CustomAutoFillLists = new Dictionary<string, string[]>();
        public static TMPro.TMP_Text ActiveDescriptionText;

        public static List<string> ItemSuggestions = new();
        public static List<string> PrefabSuggestions = new();
        public static List<string> CharacterSuggestions = new();
        public static List<string> WorldEventSuggestions = new();
        public static List<string> GotoSuggestions = new();
        public static List<string> WeatherSuggestions = new() { "light", "medium", "heavy", "cloud", "sunny", "clear", "stop", "none", "block" };
        public static List<string> StatSuggestions = new() { "Full", "Health", "Stamina", "Energy", "Hunger", "Thirst", "Fullness", "Sanity", "BatteryCharge", "Rest" };
        public static List<string> OnOffSuggestions = new() { "on", "off" };
        public static List<string> TrueFalseSuggestions = new() { "true", "false" };
        public static List<string> SeasonSuggestions = new() { "spring", "summer", "autumn", "fall", "winter", "snow" };
        public static List<string> DifficultySuggestions = new() { "peaceful", "normal", "hard", "hardsurvival" };
        public static List<string> GameModeSuggestions = new() { "standard", "mod", "creative" };
        public static List<string> VirginiaSuggestions = new() { "combat", "friend", "enemy" };
        public static List<string> WorkSchedulerSuggestions = new() { "try", "notry" };
        public static List<string> TreeRadiusStateSuggestions = new() { "clear", "damage", "destroy" };
        public static List<string> SpeakerModeSuggestions = new() { "def", "stereo", "2", "dolby", "5.1", "7.1", "atmos", "7.1.4" };
        public static List<string> AnimShowSuggestions = new() { "states", "clips", "on", "off" };
        public static List<string> RadarTypeSuggestions = new() { "on", "type", "verbose", "fish", "off" };
        public static List<string> WindIntensitySuggestions = new() { "off", "stop", "0,1", "0,5", "1,0" };
        public static List<string> MultiplierOff = new() { "off", "0,5", "2,0" };
        public static List<string> CloudProfiles = new() { "random", "idle", "growingclouds", "reducingclouds", "raining" };
        public static List<string> LodSettingsTypeSuggestions = new() { "Trees", "SmallTree", "Bush", "SmallBush", "Rocks", "SmallRocks", "Plant", "Moss", "PickUps", "BuiltStructure", "CaveEntrance", "MediumCave", "SmallCave", "Cliffs" };
        public static List<string> PostProcessingSuggestions = new() { "ContactShadows", "MicroShadowing", "ScreenSpaceReflection", "ChromaticAberration", "ScreenSpaceAmbientOcclusion" };
        public static List<string> WorldObjectStateSuggestions = new() { "Default", "Cleared", "Damaged", "Destroyed", "Burning" };

        public static List<string> FloatTypes = new() { ArgFloat, ArgDistance, ArgDamage, ArgLifetime, ArgForce, ArgWindIntensity, ArgFloatOff };


        public static void InitializeRegistry()
        {
            CommandSignatures.Clear();
            CommandDescriptions.Clear();
            ArgumentPools.Clear();

            // Register Pools
            RegisterArgumentPool(ArgItem, ItemSuggestions);
            RegisterArgumentPool(ArgWeather, WeatherSuggestions);
            RegisterArgumentPool(ArgStat, StatSuggestions);
            RegisterArgumentPool(ArgOnOff, OnOffSuggestions);
            RegisterArgumentPool(ArgTrueFalse, TrueFalseSuggestions);
            RegisterArgumentPool(ArgSeason, SeasonSuggestions);
            RegisterArgumentPool(ArgDifficulty, DifficultySuggestions);
            RegisterArgumentPool(ArgGameMode, GameModeSuggestions);
            RegisterArgumentPool(ArgVirginia, VirginiaSuggestions);
            RegisterArgumentPool(ArgCharacter, CharacterSuggestions);
            RegisterArgumentPool(ArgPrefab, PrefabSuggestions);
            RegisterArgumentPool(ArgWorldEvent, WorldEventSuggestions);
            RegisterArgumentPool(ArgWorkScheduler, WorkSchedulerSuggestions);
            RegisterArgumentPool(ArgTreeRadiusState, TreeRadiusStateSuggestions);
            RegisterArgumentPool(ArgSpeakerMode, SpeakerModeSuggestions);
            RegisterArgumentPool(ArgAnimShowType, AnimShowSuggestions);
            RegisterArgumentPool(ArgRadarType, RadarTypeSuggestions);
            RegisterArgumentPool(ArgWindIntensity, WindIntensitySuggestions);
            RegisterArgumentPool(ArgGoto, GotoSuggestions);
            RegisterArgumentPool(ArgCloudProfile, CloudProfiles);
            RegisterArgumentPool(ArgLodType, LodSettingsTypeSuggestions);
            RegisterArgumentPool(ArgFloatOff, MultiplierOff);
            RegisterArgumentPool(ArgPostProcessing, PostProcessingSuggestions);
            RegisterArgumentPool(ArgWorldObjectState, WorldObjectStateSuggestions);


            RegisterCommand("additem", new[] { ArgItem }, "Adds the specified item to the player's inventory up to its max stack amount.");
            RegisterCommand("spawnitem", new[] { ArgItem, Optional(ArgCount) }, "Spawns one or more item pickups in front of the player.");
            RegisterCommand("spawnpickup", new[] { ArgItem }, "Spawns a pickup in front of the player at the player's location.");
            RegisterCommand("removeitem", new[] { ArgItem }, "Removes all of the specified item from the player's inventory.");
            RegisterCommand("addvirginia", new[] { Optional(ArgVirginia) }, "Spawns Virginia with the specified gear preset and starting sentiment.");
            RegisterCommand("virginiasentiment", new[] { ArgFloat }, "Adds the given sentiment value to Virginia's relationship with the player.");
            RegisterCommand("forcerain", new[] { ArgWeather }, "Forces rain with a specific intensity, clears rain, or blocks rain.");
            RegisterCommand("AddCharacter", new[] { ArgCharacter, Optional(ArgCount), Optional(ArgDistance) }, "Spawns one or more NPC characters in front of the player.");
            RegisterCommand("aipause", new[] { ArgOnOff }, "Pauses or unpauses the entire Vail world simulation.");
            RegisterCommand("aishowdebugcamera", new[] { ArgOnOff }, "Shows or hides the Vail debug camera overlay for AI visualization.");
            RegisterCommand("aiverboselog", new[] { ArgOnOff }, "Enables or disables verbose debug logging from the AI system.");
            RegisterCommand("aiworldstats", new[] { ArgOnOff }, "Shows or hides the world simulation statistics overlay.");
            RegisterCommand("aipoolstats", new[] { ArgOnOff }, "Shows or hides the AI actor pool statistics overlay.");
            RegisterCommand("aiworldeventstats", new[] { ArgOnOff }, "Shows or hides world event statistics overlay.");
            RegisterCommand("aizonestats", new[] { ArgOnOff }, "Shows or hides zone statistics overlay for AI simulation zones.");
            RegisterCommand("aivillageclosest", new[] { ArgOnOff }, "Shows or hides debug information about the closest village to the player.");
            RegisterCommand("aivailstats", new[] { ArgOnOff }, "Shows or hides Vail actor manager statistics overlay.");
            RegisterCommand("removedead", new string[0], "Immediately removes all dead AI bodies from the world.");
            RegisterCommand("removeliving", new string[0], "Immediately removes all living AI actors from the world.");
            RegisterCommand("damagodebug", new[] { ArgOnOff }, "Enables or disables damage debug visualization overlay.");
            RegisterCommand("aijumpdebug", new[] { ArgOnOff }, "Enables or disables AI auto-jump debug logging and visualization.");
            RegisterCommand("aigodmode", new[] { ArgOnOff }, "When enabled, makes all AI enemies invulnerable.");
            RegisterCommand("aiknockdowndisable", new[] { ArgOnOff }, "When enabled, prevents AI enemies from being knocked down by player attacks.");
            RegisterCommand("characterlods", new[] { ArgOnOff }, "Enables or disables visual LOD system on AI characters.");
            RegisterCommand("aidummy", new[] { ArgOnOff }, "When enabled, sets all AI to critical only mode.");
            RegisterCommand("aighostplayer", new[] { ArgOnOff }, "When enabled, makes the player invisible to AI enemies.");
            RegisterCommand("aiforcestrafe", new[] { ArgOnOff }, "When enabled, forces all AI actors to strafe.");
            RegisterCommand("aishowpaths", new[] { ArgOnOff }, "Shows or hides AI pathfinding path visualization.");
            RegisterCommand("aistructurelog", new[] { ArgOnOff }, "Enables or disables debug logging for AI path structure data.");
            RegisterCommand("aishownavgraph", new[] { ArgOnOff, Optional(ArgFloat) }, "Shows or hides the navigation mesh visualization with an optional Y-axis offset.");
            RegisterCommand("aidisable", new[] { ArgOnOff }, "Completely enables or disables the entire Vail world simulation system.");
            RegisterCommand("animalsenabled", new[] { ArgOnOff }, "Enables or disables animal spawning in the world simulation.");
            RegisterCommand("animallimitmult", new[] { ArgFloat }, "Overrides the animal population limit multiplier.");
            RegisterCommand("aianimspeed", new[] { ArgFloat }, "Sets a global animation speed multiplier for all AI actors.");
            RegisterCommand("navgraphforceupdate", new string[0], "Forces the A* navigation graph to update.");
            RegisterCommand("aihelicopter", new[] { ArgOnOff }, "Toggles helicopter-holding behavior on BossMutant AI actors.");
            RegisterCommand("airunworldevent", new[] { ArgWorldEvent }, "Forces a specific world event to run immediately.");
            RegisterCommand("aishowdebug", new[] { ArgOnOff }, "Shows or hides the AI debug overlay.");
            RegisterCommand("aishowanims", new[] { ArgAnimShowType, Optional(ArgString) }, "Shows AI animation state/clip overlays. The optional string filters by actor name.");
            RegisterCommand("aishowstats", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI stat overlay. The optional string filters by actor name.");
            RegisterCommand("aishowsurvivalstats", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI survival stat overlay. The optional string filters by actor name.");
            RegisterCommand("aishowhealth", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI health bar overlay. The optional string filters by actor name.");
            RegisterCommand("aishowthoughts", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI thought process overlay. The optional string filters by actor name.");
            RegisterCommand("aishowplayerinfluences", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI player influence overlay. The optional string filters by actor name.");
            RegisterCommand("aishoweventmemory", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI event memory overlay. The optional string filters by actor name.");
            RegisterCommand("aishowanmtags", new[] { ArgOnOff, Optional(ArgString) }, "Shows AI animation tags overlay. The optional string filters by actor name.");
            RegisterCommand("airadar", new[] { ArgRadarType, Optional(ArgString) }, "Shows AI radar overlay. The optional string filters by actor name.");
            RegisterCommand("virtualplayers", new[] { ArgInt }, "Sets the number of virtual player actors for AI testing.");
            RegisterCommand("aiarmorlevel", new[] { ArgFloat }, "Forces all AI armor level to a specific value.");
            RegisterCommand("aiarmortier", new[] { ArgInt }, "Forces all AI armor tier to a specific value.");
            RegisterCommand("aiangerlevel", new[] { ArgFloat }, "Forces all AI anger level to a specific value.");
            RegisterCommand("aithought", new[] { ArgString, ArgInt }, "Forces a specific thought on all AI actors matching the group.");
            RegisterCommand("aithoughtnocooldown", new[] { ArgOnOff }, "AI debug thought groups ignore their cooldown timers.");
            RegisterCommand("aimemoryadjust", new[] { ArgString, ArgFloat }, "Adjusts AI memory stats for the local player.");
            RegisterCommand("aistatadjust", new[] { ArgStat, ArgFloat }, "Adjusts AI actor stats by the given amount.");
            RegisterCommand("aitestsleep", new[] { ArgFloat }, "Simulates the player sleeping for a given number of hours.");
            RegisterCommand("killradius", new[] { ArgDistance }, "Instantly kills all AI actors within the given radius.");
            RegisterCommand("igniteradius", new[] { ArgDistance, Optional(ArgLifetime) }, "Ignites all AI actors within radius. Optional lifetime defaults to 30 seconds.");
            RegisterCommand("burnbodyradius", new[] { ArgDistance, Optional(ArgLifetime) }, "Burns dead bodies within radius. Optional lifetime defaults to 30 seconds.");
            RegisterCommand("dismemberradius", new[] { ArgDistance, Optional(ArgForce) }, "Dismembers AI actors within radius. Optional force defaults to 30.");
            RegisterCommand("damageradius", new[] { ArgDistance, ArgDamage }, "Damages all AI actors within radius by the given amount.");
            RegisterCommand("virginiagiveitem", new[] { ArgItem }, "Gives a specific item by database ID directly to Virginia's inventory.");
            RegisterCommand("virginiagiveplayer", new[] { ArgItem }, "Makes Virginia instantly carry and then give the specified item to the player.");
            RegisterCommand("virginiaincutscenes", new[] { ArgOnOff }, "Forces Virginia to always appear in end-game cutscenes.");
            RegisterCommand("robbyincutscenes", new[] { ArgOnOff }, "Forces Robby to always appear in end-game cutscenes.");
            RegisterCommand("robbycarry", new[] { ArgItem }, "Makes Robby instantly carry the specified item type.");
            RegisterCommand("creepyvillage", new string[0], "Triggers a Creepy takeover of the closest cannibal village. This cannot be undone!");
            RegisterCommand("clearmidactionflag", new string[0], "Clears the player's mid-action flag.");
            RegisterCommand("resetheldanim", new string[0], "Re-applies animation variables for items in both hands.");
            RegisterCommand("forceplayerexpression", new[] { ArgString }, "Forces the player's facial expression to the specified one.");
            RegisterCommand("playeranimparams", new string[0], "Dumps all current player animation data to the console log.");
            RegisterCommand("virginiasit", new string[0], "Forces Virginia to visit the player.");
            RegisterCommand("audiodebug", new[] { ArgOnOff }, "Enables or disables FMOD audio debug logging.");
            RegisterCommand("clearaudioparameters", new string[0], "Clears all custom audio parameters.");
            RegisterCommand("audioparameter", new[] { ArgString, ArgFloat }, "Sets or adds a custom FMOD audio parameter.");
            RegisterCommand("audio2dtest", new[] { ArgOnOff }, "Creates or destroys a 2D audio test listener GameObject.");
            RegisterCommand("audioplayevent", new[] { ArgString }, "Plays the first FMOD event matching the given pattern.");
            RegisterCommand("audiodescription", new[] { ArgString }, "Logs the description for all FMOD events matching the pattern.");
            RegisterCommand("freecamera", new[] { ArgOnOff }, "Creates or destroys a free-flying debug camera.");
            RegisterCommand("dynamicrescameradebug", new[] { ArgOnOff }, "Enables or disables dynamic resolution debug readout overlay.");
            RegisterCommand("playerdebugcamera", new[] { ArgOnOff }, "Toggles the Vail debug camera focused on the player.");
            RegisterCommand("cameradlss", new[] { ArgOnOff }, "Enables or disables DLSS on the main camera.");
            RegisterCommand("camerafov", new[] { ArgString }, "Overrides the player's camera field of view to a specific value.");
            RegisterCommand("spawnedobjectstats", new[] { ArgOnOff }, "Shows or hides spawned object manager statistics overlay.");
            RegisterCommand("logging", new[] { ArgOnOff }, "Enables or disables Unity's debug logger.");
            RegisterCommand("logshowerrors", new[] { ArgOnOff }, "Controls whether error-level log messages are displayed.");
            RegisterCommand("logshowwarnings", new[] { ArgOnOff }, "Controls whether warning-level log messages are displayed.");
            RegisterCommand("logshowinfo", new[] { ArgOnOff }, "Controls whether info-level log messages are displayed.");
            RegisterCommand("logshownone", new string[0], "Disables all log message categories at once.");
            RegisterCommand("help", new string[0], "Lists all available console command names.");
            RegisterCommand("clear", new string[0], "Clears all accumulated log entries from the display.");
            RegisterCommand("toggleplayerstats", new string[0], "Toggles the player stats display overlay.");
            RegisterCommand("toggleoverlay", new string[0], "Cycles through overlay display modes.");
            RegisterCommand("playervisibility", new[] { ArgOnOff }, "Shows or hides the player visibility debug overlay.");
            RegisterCommand("showfps", new[] { ArgOnOff }, "Shows or hides the FPS counter display.");
            RegisterCommand("togglefpsdisplay", new string[0], "Toggles the FPS display on or off.");
            RegisterCommand("reporterrorsnow", new[] { ArgString }, "Collects and sends an error report to a configured API endpoint.");
            RegisterCommand("reportwarningsnow", new[] { ArgString }, "Collects and sends a warning report.");
            RegisterCommand("reportlogsnow", new[] { ArgString }, "Collects and sends an info log report.");
            RegisterCommand("count", new[] { ArgString }, "Counts all active objects of the given type in the scene.");
            RegisterCommand("counttag", new[] { ArgTag }, "Counts all GameObjects with the given tag.");
            RegisterCommand("listactiveentities", new string[0], "Lists all active Bolt network entities by name.");
            RegisterCommand("checkfrozentities", new string[0], "Counts and logs frozen vs. non-frozen entities.");
            RegisterCommand("checkattachedentities", new string[0], "Counts and logs attached vs. non-attached entities.");
            RegisterCommand("refreshentities", new string[0], "Refreshes the active/total Bolt entity counts and logs them.");
            RegisterCommand("listobjects", new[] { ArgString }, "Finds and logs all GameObjects matching the given name pattern.");
            RegisterCommand("listgowithlayer", new[] { ArgLayer }, "Lists all GameObjects on the specified layer.");
            RegisterCommand("countgowithlayer", new[] { ArgLayer }, "Counts all GameObjects on the specified layer.");
            RegisterCommand("findobjectswithshader", new[] { ArgString }, "Finds all renderers using a specific shader.");
            RegisterCommand("enablego", new[] { ArgGameObject }, "Re-enables a previously disabled GameObject by name.");
            RegisterCommand("disablego", new[] { ArgGameObject }, "Deactivates a GameObject found by exact name match.");
            RegisterCommand("disablegowildcard", new[] { ArgString }, "Deactivates all GameObjects matching the regex pattern.");
            RegisterCommand("destroywildcard", new[] { ArgString }, "Destroys all GameObjects matching the regex pattern.");
            RegisterCommand("togglego", new[] { ArgGameObject }, "Toggles a GameObject's active state.");
            RegisterCommand("destroy", new[] { ArgGameObject }, "Destroys a GameObject found by exact name match.");
            RegisterCommand("disablecomponent", new[] { ArgString }, "Disables a specific component on a GameObject.");
            RegisterCommand("enablecomponent", new[] { ArgString }, "Enables a specific component on a GameObject.");
            RegisterCommand("disablescene", new[] { ArgScene }, "Deactivates all root GameObjects in the named scene.");
            RegisterCommand("enablescene", new[] { ArgScene }, "Re-enables all root GameObjects in the named scene.");
            RegisterCommand("inspectgo", new[] { ArgGameObject }, "Recursively inspects and logs a GameObject's hierarchy and components.");
            RegisterCommand("enablecheats", new[] { ArgOnOff }, "Enables or disables cheat mode globally.");
            RegisterCommand("veganmode", new[] { ArgOnOff }, "Prevents enemies from spawning.");
            RegisterCommand("loghack", new[] { ArgOnOff }, "Enables infinite log chopping on the held item.");
            RegisterCommand("stonehack", new[] { ArgOnOff }, "Enables infinite stone mining on the held item.");
            RegisterCommand("energyhack", new[] { ArgOnOff }, "Player energy never depletes.");
            RegisterCommand("ammohack", new[] { ArgOnOff }, "Ranged weapon ammo never depletes.");
            RegisterCommand("godmode", new[] { ArgOnOff }, "Sets full stats, disables survival, and enables god mode.");
            RegisterCommand("buildhack", new[] { ArgOnOff }, "Enables unlimited building without resource costs.");
            RegisterCommand("survival", new[] { ArgOnOff }, "Enables or disables survival features.");
            RegisterCommand("buildermode", new[] { ArgOnOff }, "Enables build hack, god mode, disables survival, and adds items.");
            RegisterCommand("listitems", new string[0], "Lists all item names with their IDs.");
            RegisterCommand("listitemswithtags", new string[0], "Lists all items that have tags.");
            RegisterCommand("additemswithtag", new[] { ArgTag }, "Adds all items matching the given tag to inventory.");
            RegisterCommand("addallitems", new string[0], "Adds one full stack of every non-story item to inventory.");
            RegisterCommand("removeallitems", new string[0], "Removes all non-story items from inventory.");
            RegisterCommand("addallstoryitems", new string[0], "Adds one full stack of every story item to inventory.");
            RegisterCommand("removeallstoryitems", new string[0], "Removes all story items from inventory.");
            RegisterCommand("setinventorypercent", new[] { ArgInt }, "Adjusts all inventory item amounts to the given percentage.");
            RegisterCommand("refillcontainers", new string[0], "Refills all volume container items in the player's inventory.");
            RegisterCommand("addallbookpages", new string[0], "Sets endgame exited, maxes sanity, and adds story items.");
            RegisterCommand("heallocalplayer", new[] { ArgInt }, "Heals the local player by the given amount.");
            RegisterCommand("hitlocalplayer", new[] { ArgInt }, "Damages the local player by the given amount.");
            RegisterCommand("killlocalplayer", new string[0], "Instantly kills the local player.");
            RegisterCommand("knockdownlocalplayer", new[] { ArgInt }, "Knocks down the local player and deals damage.");
            RegisterCommand("revivelocalplayer", new string[0], "Revives the local player if dead.");
            RegisterCommand("regenhealth", new[] { ArgFloat }, "Heals the local player by the specified amount.");
            RegisterCommand("setstat", new[] { ArgStat, Optional(ArgFloat) }, "Sets specific player vitals/stats.");
            RegisterCommand("deathcount", new[] { ArgInt }, "Sets the player's tracked death count.");
            RegisterCommand("playgameover", new string[0], "Triggers the game over sequence immediately.");
            RegisterCommand("playdeathcutscene", new[] { ArgString }, "Triggers a player death cutscene of the specified type.");
            RegisterCommand("listdeathmarkers", new string[0], "Lists all death and drown markers in the scene.");
            RegisterCommand("playdeathmarkerindex", new[] { ArgInt }, "Plays the death/drown cutscene marker at the given index.");
            RegisterCommand("playdeathmarker", new[] { ArgString }, "Plays a death cutscene or drown wakeup sequence by marker name.");
            RegisterCommand("setexitedendgame", new[] { ArgOnOff }, "Sets whether the player has exited the endgame state.");
            RegisterCommand("blockplayerfinaldeath", new[] { ArgOnOff }, "Blocks the player from experiencing the final death state.");
            RegisterCommand("gameoverdelaytime", new[] { ArgFloat }, "Sets the delay time before the game over screen appears.");
            RegisterCommand("instantrespawnhere", new[] { ArgString }, "The player instantly respawns at their current location upon death.");
            RegisterCommand("timescale", new[] { ArgFloat }, "Sets the Unity time scale.");
            RegisterCommand("settimeofday", new[] { ArgString }, "Sets the current time of day and restores normal time progression.");
            RegisterCommand("locktimeofday", new[] { ArgString }, "Locks game time or sets and locks a specific time.");
            RegisterCommand("jumptimeofday", new[] { ArgFloat, Optional(ArgFloat) }, "Jumps the game time forward by a number of days.");
            RegisterCommand("setgametimespeed", new[] { ArgFloat }, "Sets the game time speed multiplier.");
            RegisterCommand("setcurrentday", new[] { ArgInt }, "Sets the current in-game day number.");
            RegisterCommand("season", new[] { ArgSeason }, "Locks the current season to the specified one.");
            RegisterCommand("unlockseason", new string[0], "Unlocks the season so it can change naturally again.");
            RegisterCommand("forcecloud", new[] { ArgCloudProfile, Optional(ArgFloat) }, "Forces a specific cloud profile and optionally sets cloud cover.");
            RegisterCommand("forcecloudprofile", new[] { ArgCloudProfile }, "Switches to a specific cloud profile.");
            RegisterCommand("cloudenable", new[] { ArgOnOff }, "Enables or disables cloud rendering entirely.");
            RegisterCommand("cloudshadowsenable", new[] { ArgOnOff }, "Enables or disables cloud shadow rendering.");
            RegisterCommand("cloudfactor", new[] { ArgFloat }, "Locks the storm/cloud cover factor to a specific amount.");
            RegisterCommand("setwindintensity", new[] { ArgWindIntensity }, "Locks wind intensity to a float value, or use 'off'/'stop' to return to normal dynamic wind.");
            RegisterCommand("speedyrun", new[] { ArgString }, "Adds a speed multiplier to the player's run and swim speeds.");
            RegisterCommand("superjump", new[] { ArgString }, "Sets a super jump multiplier on the player.");
            RegisterCommand("goto", new[] { ArgGoto }, "Teleports the player to a named goto point or specific coordinates.");
            RegisterCommand("gotocoords", new[] { ArgString }, "Teleports the player to exact world coordinates.");
            RegisterCommand("gotoforce", new[] { ArgGoto }, "Force-teleports the player, skipping landing checks.");
            RegisterCommand("gototag", new[] { ArgTag }, "Teleports the player to a GameObject with the given tag.");
            RegisterCommand("follow", new[] { ArgGameObject }, "Makes the player character follow the specified target.");
            RegisterCommand("followstop", new string[0], "Stops the player from following and makes them visible again.");
            RegisterCommand("setlookrotation", new[] { ArgString }, "Sets the player's look rotation directly.");
            RegisterCommand("mousexsensitivity", new[] { ArgFloat }, "Sets the mouse horizontal sensitivity.");
            RegisterCommand("mouseysensitivity", new[] { ArgFloat }, "Sets the mouse vertical sensitivity.");
            RegisterCommand("gamepadxsensitivity", new[] { ArgFloat }, "Sets the gamepad right-stick horizontal sensitivity.");
            RegisterCommand("gamepadysensitivity", new[] { ArgFloat }, "Sets the gamepad right-stick vertical sensitivity.");
            RegisterCommand("gamepaddeadzone", new[] { ArgFloat }, "Sets the gamepad stick deadzone.");
            RegisterCommand("invertlook", new[] { ArgOnOff }, "Enables or disables inverted Y-axis look.");
            RegisterCommand("sprinttoggle", new[] { ArgOnOff }, "Changes sprint to a toggle instead of hold-to-sprint.");
            RegisterCommand("crouchtoggle", new[] { ArgOnOff }, "Changes crouch to a toggle instead of hold-to-crouch.");
            RegisterCommand("terrainrender", new[] { ArgOnOff }, "Enables or disables terrain heightmap and tree/foliage rendering.");
            RegisterCommand("terrainrendersimple", new[] { ArgOnOff }, "Switches the terrain material to a simple tessellation shader.");
            RegisterCommand("terrainpixelerror", new[] { ArgFloat }, "Sets the closest active terrain's heightmap pixel error.");
            RegisterCommand("terraintess", new[] { ArgString }, "Sets the terrain tessellation factor shader value.");
            RegisterCommand("terraintessdist", new[] { ArgString }, "Sets the terrain tessellation min/max distance shader values.");
            RegisterCommand("terrainparallax", new[] { ArgFloat }, "Sets the parallax mapping enable value on the terrain material.");
            RegisterCommand("togglevsync", new string[0], "Toggles VSync on/off.");
            RegisterCommand("toggleocclusionculling", new string[0], "Toggles occlusion culling on/off for the main camera.");
            RegisterCommand("qualitytexture", new[] { ArgInt }, "Sets global texture mipmap limit (resolution).");
            RegisterCommand("mipmapstreaming", new[] { ArgOnOff }, "Toggles texture streaming mipmaps.");
            RegisterCommand("mipmapstreambudget", new[] { ArgFloat }, "Sets memory budget for texture streaming and flushes mipmaps.");
            RegisterCommand("mipmapstreamingdiscard", new[] { ArgOnOff }, "Toggles discarding unused streaming mip textures.");
            RegisterCommand("targetframerate", new[] { ArgInt }, "Sets the target frame rate.");
            RegisterCommand("billboardenabled", new[] { ArgLodType, ArgOnOff }, "Enables or disables a specific billboard type.");
            RegisterCommand("billboardignorechanges", new[] { ArgOnOff }, "Toggles whether billboard changes are ignored.");
            RegisterCommand("anisoenabled", new[] { ArgOnOff }, "Enables or disables anisotropic filtering.");
            RegisterCommand("anisominmax", new[] { ArgFloat, ArgFloat }, "Sets global anisotropic filtering min and max limits.");
            RegisterCommand("loddebugbillboards", new[] { ArgOnOff }, "Toggles shader keyword to visualize billboard LOD transitions.");
            RegisterCommand("loddebugmaterials", new[] { ArgOnOff }, "Toggles LOD debug material coloring on world object locators.");
            RegisterCommand("loddebugranges", new[] { ArgLodType }, "Toggles LOD debug range visualization for a given LOD type.");
            RegisterCommand("lodforce3ddistance", new[] { ArgOnOff }, "Forces the LOD system to use 3D distance calculations.");
            RegisterCommand("lodforce2ddistance", new[] { ArgOnOff }, "Forces the LOD system to use 2D distance calculations.");
            RegisterCommand("dynamicresolutioncycletest", new[] { ArgOnOff }, "Runs a dynamic resolution stress test.");
            RegisterCommand("dynamicresolutionoverride", new[] { ArgString }, "Overrides the dynamic resolution scaler to a fixed value.");
            RegisterCommand("dynamicresolutiontarget", new[] { ArgInt }, "Sets the target FPS for dynamic resolution scaling.");
            RegisterCommand("showcollisionobjectnames", new[] { ArgDistance, Optional(ArgString), Optional(ArgString) }, "Overlays collider names in the world within distance. Optionally filter by 'layer'/'tag' and a name pattern.");
            RegisterCommand("showmeshobjectnames", new[] { ArgDistance, Optional(ArgFloat), Optional(ArgString) }, "Displays renderer names within distance. Optionally specify a minimum mesh size and a filter.");
            RegisterCommand("showmeshtrianglecounts", new[] { ArgOnOff }, "Includes triangle count alongside mesh names.");
            RegisterCommand("showmeshmaterialnames", new[] { ArgOnOff }, "Displays material name and shader name alongside mesh names.");
            RegisterCommand("showactivelights", new[] { ArgOnOff }, "Overlays the names of all enabled Light components.");
            RegisterCommand("showtriggercollision", new[] { ArgOnOff }, "Includes trigger colliders in the collision overlay display.");
            RegisterCommand("showworldobjects", new[] { ArgInt, Optional(ArgString) }, "Toggles world object debug drawing within distance.");
            RegisterCommand("showobjectlocation", new[] { ArgGameObject }, "Tracks and displays an in-world label pointing to the specified GameObject.");
            RegisterCommand("showstimuli", new[] { ArgInt, Optional(ArgString) }, "Overlays nearby AI Stimuli positions within distance.");
            RegisterCommand("slapchop", new[] { ArgOnOff }, "Enables one-hit tree felling.");
            RegisterCommand("regrowalltrees", new string[0], "Forces all trees in the world to regrow to their initial state.");
            RegisterCommand("restoreallworldlocators", new string[0], "Clears all world object locator states to default.");
            RegisterCommand("treescutall", new[] { ArgString }, "Cuts down or instantly clears all tree swappers in the scene.");
            RegisterCommand("treeradius", new[] { ArgTreeRadiusState, ArgFloat }, "Sets the world object state for all trees within the given radius.");
            RegisterCommand("forceremovetrees", new[] { ArgInt }, "Forces removal of N trees.");
            RegisterCommand("treecutsimulatebolt", new[] { ArgOnOff }, "Toggles Bolt network simulation for tree cutting events.");
            RegisterCommand("treefallcontactinfo", new[] { ArgOnOff }, "Toggles logging of tree fall contact/collision information.");
            RegisterCommand("treeocclusionbonus", new[] { ArgFloat }, "Overrides the tree occlusion bonus ratio in the LOD manager.");
            RegisterCommand("spawnfallingtree", new[] { ArgString }, "Spawns a falling tree at the specified location.");
            RegisterCommand("clearbushradius", new[] { ArgFloat }, "Breaks all world objects in a radius around the player.");
            RegisterCommand("breakobjects", new[] { ArgFloat, Optional(ArgString) }, "Breaks all breakable objects and cuts trees within the given range.");
            RegisterCommand("replaceshader", new[] { ArgString, ArgString }, "Replaces all materials using the source shader with the target shader.");
            RegisterCommand("removeshader", new[] { ArgString }, "Replaces all materials with a default shader and random tints.");
            RegisterCommand("applydefaultmaterials", new[] { ArgOnOff }, "Replaces all renderer shaders with a default lit material and random tint.");
            RegisterCommand("postprocessingcomponent", new[] { ArgPostProcessing, ArgOnOff }, "Enables or disables a named post-processing volume component.");
            RegisterCommand("exposuresetspeed", new[] { ArgString }, "Overrides the exposure adaptation speed on all Volumes.");
            RegisterCommand("physicsupdatetime", new[] { ArgString }, "Adjusts or resets the physics fixed-timestep.");
            RegisterCommand("gravity", new[] { ArgString }, "Overrides global gravity Y-component.");
            RegisterCommand("enablecollisionbasedkillbox", new[] { ArgOnOff }, "Enables or disables collider-based kill box mode.");
            RegisterCommand("greebledrockscollision", new[] { ArgOnOff }, "Adds or removes MeshCollider components on greebled rock objects.");
            RegisterCommand("userigidbodyrotation", new[] { ArgOnOff }, "Toggles using RigidBody physics for camera rotation.");
            RegisterCommand("meshcollidermeshlog", new[] { ArgOnOff }, "Toggles logging of shared mesh changes on MeshColliders.");
            RegisterCommand("disconnectplayer", new[] { ArgString }, "Server-only command. Disconnects a specific player by name.");
            RegisterCommand("disconnectplayers", new string[0], "Server-only command. Disconnects all connected clients.");
            RegisterCommand("kickplayers", new string[0], "Server-only command. Kicks all connected clients with a debug message.");
            RegisterCommand("joinsteamlobby", new string[0], "Placeholder for joining a Steam lobby by ID.");
            RegisterCommand("dumplobbyinfo", new string[0], "Dumps Steam lobby connection state information.");
            RegisterCommand("netspawnplayer", new string[0], "Instantiates a network player prefab for testing.");
            RegisterCommand("playernetanimator", new[] { ArgOnOff }, "Enables or disables Animator components on all networked player representations.");
            RegisterCommand("netanimator", new[] { ArgOnOff }, "Enables or disables Animator component on PlayerNet objects.");
            RegisterCommand("netskinnedbones", new[] { ArgInt }, "Sets the skin quality level and shadow casting for network players.");
            RegisterCommand("capsulemode", new[] { ArgOnOff }, "Hides renderers on networked player objects and shows only capsule colliders.");
            RegisterCommand("golfcartnetworkdebug", new[] { ArgOnOff }, "Toggles debug renderers for golf cart network behavior.");
            RegisterCommand("profilersnapshot", new string[0], "Dumps a memory profiler snapshot to the log.");
            RegisterCommand("profilersample", new[] { ArgOnOff }, "Starts or stops Unity Profiler sampling to a file.");
            RegisterCommand("gccollect", new string[0], "Forces a garbage collection.");
            RegisterCommand("unloadunusedassets", new string[0], "Frees unused assets from memory.");
            RegisterCommand("logtextures", new[] { ArgFloat }, "Starts or stops interval-based texture memory logging.");
            RegisterCommand("diagrenderers", new[] { ArgString }, "Writes a diagnostic text file listing renderers with mesh info.");
            RegisterCommand("dumptransforminfo", new[] { Optional(ArgString) }, "Enumerates Transforms and dumps a CSV file with transform hierarchy data.");
            RegisterCommand("loadscene", new[] { ArgScene }, "Loads a scene additively.");
            RegisterCommand("loadscenesingle", new[] { ArgScene }, "Loads a scene in Single mode, replacing all current scenes.");
            RegisterCommand("unloadscene", new[] { ArgScene }, "Unloads a scene by name or index.");
            RegisterCommand("allowasync", new[] { ArgOnOff }, "Enables or disables async loading.");
            RegisterCommand("save", new[] { ArgInt }, "Saves the game to the specified slot index.");
            RegisterCommand("load", new[] { ArgInt }, "Loads the game from the specified slot index.");
            RegisterCommand("saveplayer", new string[0], "Saves player-specific data.");
            RegisterCommand("loadplayer", new string[0], "Loads player-specific data.");
            RegisterCommand("resetsettings", new string[0], "Resets all game settings to their default values.");
            RegisterCommand("clearallsettings", new string[0], "Clears all game settings entirely.");
            RegisterCommand("setsetting", new[] { ArgString, ArgString }, "Sets a game settings key-value pair.");
            RegisterCommand("setgamesetupsetting", new[] { ArgString, ArgString }, "Sets a game setup settings key-value pair.");
            RegisterCommand("setspeakermode", new[] { ArgSpeakerMode }, "Sets the FMOD audio speaker mode configuration.");
            RegisterCommand("setplayerrace", new[] { ArgInt }, "Applies the specified player race by index.");
            RegisterCommand("getgamemode", new string[0], "Logs the current game mode.");
            RegisterCommand("setgamemode", new[] { ArgGameMode }, "Sets the game mode.");
            RegisterCommand("setdifficultymode", new[] { ArgDifficulty }, "Sets the difficulty mode (Peaceful, Normal, Hard, HardSurvival).");
            RegisterCommand("showdebugzones", new[] { ArgOnOff }, "Creates and shows zone cell visualizations.");
            RegisterCommand("gotozone", new[] { ArgString }, "Teleports the player to the center of the specified zone cell.");
            RegisterCommand("worldgroupid", new[] { ArgString, Optional(ArgOnOff) }, "Toggles or sets a specific world group ID.");
            RegisterCommand("setworldobjectstaterange", new[] { ArgDistance, ArgWorldObjectState }, "Applies a specific world object state to objects within range.");
            RegisterCommand("resetinputaxes", new string[0], "Resets all input axes.");
            RegisterCommand("rumbetest", new[] { ArgString }, "Tests gamepad rumble with the specified parameters.");
            RegisterCommand("astar", new[] { ArgOnOff }, "Enables or disables the A* pathfinding component.");
            RegisterCommand("timeofdayconnectiondebug", new[] { ArgOnOff }, "Toggles debug connection visualization on TimeOfDayHolder.");
            RegisterCommand("radiodebug", new[] { ArgOnOff }, "Toggles debug text display on the radio UI.");
            RegisterCommand("sleepcooldown", new[] { ArgOnOff }, "Toggles the sleep cooldown.");
            RegisterCommand("uvailstats", new[] { ArgOnOff }, "Shows Vail-specific actor stats overlay.");
            RegisterCommand("demomode", new[] { ArgOnOff }, "Disables debug camera/UI, enables god mode, and adds all items.");
            RegisterCommand("combatteststart", new string[0], "Teleports the player for combat testing and grants an axe.");
            RegisterCommand("loadmacros", new string[0], "Loads all macro files and registers them as console commands.");
            RegisterCommand("openmacrosfolder", new string[0], "Opens the Macros directory in Windows Explorer.");
            RegisterCommand("loaddebugconsolemod", new[] { ArgString }, "Loads an external debug mod assembly and registers its commands.");
            RegisterCommand("firstlookforce", new[] { ArgOnOff }, "Forces first-look mode on held controllers.");
            RegisterCommand("addmemory", new[] { ArgString }, "Adds or clears junk memory allocations for testing.");
            RegisterCommand("filteraudio", new[] { ArgString }, "Sets the FMOD audio path filter for debug output.");
            RegisterCommand("footstepdebug", new string[0], "Toggles footstep debug visualization.");
            RegisterCommand("showbutterflyinfo", new[] { ArgOnOff }, "Toggles butterfly spawner debug info display.");
            RegisterCommand("cavelight", new[] { ArgOnOff }, "Creates or toggles a point light attached to the player.");
            RegisterCommand("createlight", new string[0], "Spawns a new HD point light at the active world location.");
            RegisterCommand("duplicateobject", new[] { ArgGameObject, Optional(ArgInt), Optional(ArgFloat), Optional(ArgFloat), Optional(ArgFloat) }, "Instantiates copies of a named GameObject. Optionally specify count and X/Y/Z scale factors.");
            RegisterCommand("setproperty", new[] { ArgGameObject, ArgString, ArgString, ArgString, Optional(ArgString), Optional(ArgString) }, "Sets a property on a component of a GameObject via reflection.");
            RegisterCommand("sendmessageto", new[] { ArgGameObject, ArgString }, "Calls SendMessage on the target GameObject.");
            RegisterCommand("testeventmask", new string[0], "Sets all cameras' event masks to 0.");
            RegisterCommand("disablegameobjecttester", new[] { Optional(ArgString) }, "Benchmarks GameObject activation strategies.");
            RegisterCommand("trailer3", new string[0], "Preset for trailer 3 footage: handles items and inventory levels.");
            RegisterCommand("workscheduler", new[] { ArgWorkScheduler }, "Controls try-catch wrapping on WorkSchedulerBatch items.");
            RegisterCommand("toggleworkscheduler", new[] { Optional(ArgOnOff) }, "Enables, disables, or toggles the WorkScheduler system.");
            RegisterCommand("wsscaling", new[] { ArgOnOff }, "Toggles WorkScheduler FPS scaling.");
            RegisterCommand("getlayerculldistance", new[] { ArgLayer }, "Logs the current cull distance for the specified layer.");
            RegisterCommand("setlayerculldistance", new[] { ArgLayer, ArgInt }, "Sets the cull distance for a specific layer.");
            RegisterCommand("showworldposfor", new[] { ArgString }, "Adds world position GUI labels to all GameObjects with the given component.");
            RegisterCommand("hideworldposfor", new[] { ArgString }, "Removes world position GUI labels from GameObjects with the given component.");
            RegisterCommand("debugplayermelee", new[] { ArgString }, "Toggles player melee damage debug visualization.");
            RegisterCommand("debugplayerhitlog", new[] { ArgOnOff }, "Enables or disables logging of player hit events.");
            RegisterCommand("invisible", new[] { ArgOnOff }, "Sets the player invisible to enemies and disables bush pusher.");
            RegisterCommand("setopeningcrash", new[] { ArgString }, "Overrides the opening cutscene crash site alias.");
            RegisterCommand("diggingclear", new string[0], "Finds nearest digging component and activates it.");
            RegisterCommand("destroyragdoll", new string[0], "Removes all ragdoll instances from the scene.");
            RegisterCommand("enablestructureghosts", new[] { ArgOnOff }, "Toggles showing all structure ghost previews.");
            RegisterCommand("playerinterruptkeys", new[] { ArgOnOff }, "Starts a coroutine for debug input interrupt keys.");
            RegisterCommand("audioDebugStates", new[] { ArgOnOff }, "Toggles drawing audio and state debug info on all animators.");
            RegisterCommand("animStatesGui", new[] { ArgOnOff }, "Toggles a GUI overlay showing animator state info.");
            RegisterCommand("ResetAllAchievements", new string[0], "Clears every achievement via Steam API.");
            RegisterCommand("ResetAchievement", new[] { ArgString }, "Finds an achievement by its name and clears it on Steam.");
            RegisterCommand("ListAchievements", new string[0], "Iterates and logs all Steam achievements.");
            RegisterCommand("ToggleGrabberDebug", new[] { ArgOnOff }, "Toggles a GrabberDebug component on/off.");
            RegisterCommand("InstantBookBuild", new[] { ArgOnOff }, "Enables instant building from the crafting book.");
            RegisterCommand("PlayCutscene", new[] { ArgString, Optional(ArgString) }, "Plays a named cutscene by its alias or name.");
            RegisterCommand("toggleGrabsFaceDebug", new[] { ArgOnOff }, "Toggles visual debug display of blueprint faces in GRABS.");
            RegisterCommand("AddPrefab", new[] { ArgPrefab, Optional(ArgCount) }, "Spawns one or more prefab instances near the player.");
            RegisterCommand("grabsgeneratebuilt", new[] { ArgOnOff }, "Instantly generates all GRABS blueprints as built.");
            RegisterCommand("SpawnWorldObject", new[] { ArgLodType }, "Creates a WorldObjectLocator at raycasted terrain position.");
            RegisterCommand("WorldObjectDisableAll", new string[0], "Disables all world object groups immediately.");
            RegisterCommand("WorldObjectEnableAll", new string[0], "Enables all world object groups immediately.");
            RegisterCommand("vitalsShowDebug", new[] { ArgOnOff }, "Toggles visibility of the vitals debug group overlay.");
            RegisterCommand("logVirtual", new[] { ArgOnOff }, "Toggles virtual log mode for testing log behavior.");
            RegisterCommand("LightningInterval", new[] { ArgFloat, Optional(ArgFloat) }, "Sets the interval in seconds between lightning strikes.");
            RegisterCommand("testingSampleFps", new string[0], "Triggers an FPS sample capture for performance analysis.");
            RegisterCommand("ToggleFireDebug", new[] { ArgOnOff }, "Toggles an overlay showing cooking fire info.");
            RegisterCommand("ShowDebugZones", new[] { ArgOnOff }, "Toggles the VisualWorldDebugGrid system visualizations.");
            RegisterCommand("GotoZone", new[] { ArgString }, "Teleports the player to the center of the specified zone cell.");
            RegisterCommand("ToggleBeamDebug", new[] { ArgOnOff }, "Toggles verbose logging for beam placement.");
            RegisterCommand("ToggleRecordBoltEvents", new[] { ArgOnOff }, "Starts or stops recording Bolt network events.");
            RegisterCommand("ToggleExecuteRecordedBoltEvents", new[] { ArgOnOff }, "Starts or stops replaying Bolt network events.");
            RegisterCommand("ToggleExecuteCallingBoltEvent", new[] { ArgOnOff }, "Starts or stops executing calling event mode.");
            RegisterCommand("ClearRecordedSimulatedBoltEvents", new string[0], "Clears all recorded Bolt network events.");
            RegisterCommand("OutputSnapPointsToFile", new[] { ArgString }, "Writes predicted construction areas and snap points to a text file.");
            RegisterCommand("CountLinkedStructures", new string[0], "Logs counts of linked structures grouped by type.");
            RegisterCommand("ExportLinkedStructuresToJson", new[] { ArgString }, "Exports all linked structures to a JSON file.");
            RegisterCommand("ImportLinkedStructuresFromFile", new[] { ArgString }, "Imports linked structures from a JSON file.");
            RegisterCommand("ToggleSuperStructureRoomsVisualDebug", new[] { ArgOnOff }, "Toggles visual debug overlay of super-structure rooms.");
            RegisterCommand("ToggleStructureResistanceDebug", new[] { ArgOnOff }, "Toggles an overlay displaying structure health and resistance.");
            RegisterCommand("DestroyFreeFormStructure", new[] { ArgGameObject }, "Instantly destroys a freeform structure by name.");
            RegisterCommand("DamageFreeFormStructure", new[] { ArgGameObject, Optional(ArgFloat) }, "Damages a freeform structure by name with the specified amount.");
            RegisterCommand("SetStrengthLevel", new[] { ArgInt }, "Sets the player's strength level to the target.");
            RegisterCommand("GainStrength", new[] { ArgInt, Optional(ArgFloat) }, "Grants the player additional strength points, or optionally over time.");
            RegisterCommand("BuffStats", new[] { Optional(ArgStat) }, "Applies a temporary stat buff for fullness, hydration, and rest.");
            RegisterCommand("LightningHitTreeChance", new[] { ArgFloat }, "Sets the probability that lightning will strike a tree.");
            RegisterCommand("LightningHitTreeMustBeInfrontPlayer", new[] { ArgTrueFalse }, "Restricts lightning strikes to trees in front of the player.");

            CustomCommandManager.RegisterCommands();
        }

        public static void RegisterArgumentPool(string argTypeName, List<string> poolData)
        {
            if (string.IsNullOrEmpty(argTypeName) || poolData == null) return;
            ArgumentPools[argTypeName] = poolData;
        }

        public static void RegisterCommand(string commandName, string[] rawArgumentTypes, string description = "")
        {
            if (string.IsNullOrEmpty(commandName) || rawArgumentTypes == null) return;

            CommandDescriptions[commandName] = description;

            CommandArg[] parsedArgs = new CommandArg[rawArgumentTypes.Length];
            for (int i = 0; i < rawArgumentTypes.Length; i++)
            {
                string raw = rawArgumentTypes[i];
                bool isOpt = raw.StartsWith("[") && raw.EndsWith("]");
                string cleanName = raw.Trim('<', '>', '[', ']');

                parsedArgs[i] = new CommandArg { Name = cleanName, IsOptional = isOpt };
            }

            CommandSignatures[commandName] = parsedArgs;
        }

        public static bool RegisterExternalCommand(string commandName, string description, string[] args, Dictionary<string, string[]> customLists, bool logErrors = true)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                if (logErrors) RedLoader.RLog.Error("[ImprovedDebugConsole] Failed to register external command: commandName is null or empty.");
                return false;
            }

            commandName = commandName.ToLower();

            if (CommandSignatures.ContainsKey(commandName))
            {
                if (logErrors) RedLoader.RLog.Warning($"[ImprovedDebugConsole] External mod is overwriting an existing command: '{commandName}'");
            }

            if (!string.IsNullOrEmpty(description))
            {
                CommandDescriptions[commandName] = description;
            }

            if (customLists != null)
            {
                foreach (var kvp in customLists)
                {
                    CustomAutoFillLists[kvp.Key] = kvp.Value;
                }
            }

            if (args != null && args.Length > 0)
            {
                CommandArg[] parsedArgs = new CommandArg[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    if (string.IsNullOrEmpty(args[i]))
                    {
                        if (logErrors) RedLoader.RLog.Error($"[ImprovedDebugConsole] Failed to register command '{commandName}': argument at index {i} is null or empty.");
                        return false;
                    }

                    string[] parts = args[i].Split(':');
                    string rawName = parts[0];

                    if (string.IsNullOrEmpty(rawName))
                    {
                        if (logErrors) RedLoader.RLog.Error($"[ImprovedDebugConsole] Failed to register command '{commandName}': argument name at index {i} is invalid.");
                        return false;
                    }

                    bool isOptional = rawName.EndsWith("?");
                    string cleanName = isOptional ? rawName.TrimEnd('?') : rawName;

                    parsedArgs[i] = new CommandArg
                    {
                        Name = cleanName,
                        IsOptional = isOptional,
                        Type = parts.Length > 1 ? parts[1].Replace("?", "") : ""
                    };
                }
                CommandSignatures[commandName] = parsedArgs;
            }

            return true;
        }

        public static string Optional(string rawArg)
        {
            if (string.IsNullOrEmpty(rawArg)) return rawArg;
            return $"[{rawArg}]";
        }

        public static void IndexGameDatabases()
        {
            ItemSuggestions.Clear();
            CharacterSuggestions.Clear();
            PrefabSuggestions.Clear();
            WorldEventSuggestions.Clear();

            try
            {
                // Items
                foreach (var item in Sons.Items.Core.ItemDatabaseManager.Items)
                {
                    if (item == null || string.IsNullOrEmpty(item.Name)) continue;
                    ItemSuggestions.Add(item.Name.Replace(" ", ""));
                }

                // Actors
                List<VailActorTypeId> CharacterList = new();
                CharacterList.AddRange(UtilityManager.GetActorTypesOfClass(Sons.Ai.Vail.VailActorClassId.Cannibal));
                CharacterList.AddRange(UtilityManager.GetActorTypesOfClass(Sons.Ai.Vail.VailActorClassId.Creepy));
                CharacterList.AddRange(UtilityManager.GetActorTypesOfClass(Sons.Ai.Vail.VailActorClassId.Animal));
                CharacterList.Add(Sons.Ai.Vail.VailActorTypeId.Virginia);
                CharacterList.Add(Sons.Ai.Vail.VailActorTypeId.Robby);

                foreach (VailActorTypeId actorType in CharacterList)
                {
                    if(actorType == VailActorTypeId.SpongeTest) continue;
                    string name = actorType.ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                        CharacterSuggestions.Add(name.Replace(" ", ""));
                    }
                }

                // Prefabs
                foreach(PrefabDefinition prefab in PrefabManager._instance._definitions)
                {
                    PrefabSuggestions.Add(prefab._id);
                }

                // World events
                var worldEvents = VailWorldEvents._instance;
                if (worldEvents != null && worldEvents._worldEventData != null)
                {
                    VailWorldEventData eventData = worldEvents._worldEventData;

                    foreach (SearchPartyEvent eventDef in eventData._searchPartyEvents)
                    {
                        WorldEventSuggestions.Add(eventDef.name);
                    };
                }

                // Goto Points
                foreach (UnityEngine.GameObject go in UnityEngine.Object.FindObjectsOfType<UnityEngine.GameObject>(true))
                {
                    if (go.name.EndsWith("Goto", StringComparison.OrdinalIgnoreCase))
                    {
                        string pointName = go.name.Substring(0, go.name.Length - 4);
                        if (!GotoSuggestions.Contains(pointName))
                        {
                            GotoSuggestions.Add(pointName);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        public static List<string> GetRawPool(string command, int argIndex)
        {
            if (CommandSignatures.TryGetValue(command, out CommandArg[] args))
            {
                if (argIndex >= 0 && argIndex < args.Length)
                {
                    string currentArgType = !string.IsNullOrEmpty(args[argIndex].Type) ? args[argIndex].Type : args[argIndex].Name;

                    if (ArgumentPools.TryGetValue(currentArgType, out List<string> pool) && pool.Count > 0)
                    {
                        return pool;
                    }

                    if (CustomAutoFillLists.TryGetValue(currentArgType, out string[] customPool) && customPool.Length > 0)
                    {
                        return customPool.ToList();
                    }
                }
            }

            return null;
        }
    }
}
