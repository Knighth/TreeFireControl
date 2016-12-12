using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ICities;
using CitiesSkylinesDetour;

namespace TreeFireControl
{
    public class Mod : IUserMod
    {

        internal const string MOD_NAME = "Tree Fire Control";  //the name of your mod, make it constant in case you use it more then once.
        internal const string MOD_VERSIONSTRING = "1.6.0-f4 Build 02";
        internal const string MOD_DESC = "Adjust rate that trees catch fire";  //keep it short.
        internal const string MOD_LOG_PREFIX = "TreeFireControl";  //This is used by the logging routine incase you want something different then MY_MODS_NAME
        internal const string MOD_CONFIGPATH = "TreeFireControl_Config.xml";
        internal const string MOD_CUSTOM_LOGPATH_DEFAULT = "TreeFireControl_Log.txt";
        internal const string Mod_SteamID = "814903880";
        internal static bool isEnabled = false;  //var we use to track if your mod is enabled. 
        internal static bool isInited = false;   //var we use to track if you've done any needed 'startup' stuff you need to do.
        internal static bool DEBUG_LOG_ON = false;  //holds if user has enabled Debug logging.
        internal static byte DEBUG_LOG_LEVEL = 0;  // hold if user has selected a particular bug log detail level.
        internal static Configuration config;  //hold our configuration data object. 

        

        public string Name
        {
            get
            {
                return MOD_NAME;
            }

        }

        public string Description
        {
            get
            {
                return MOD_DESC;
            }
        }

        //contructor
        public Mod()
        {
            Logger.dbgLog(string.Format("{0} version {1} has been loaded.", Mod.MOD_NAME, MOD_VERSIONSTRING));
        }

        /// <summary>
        /// Fires when user 'enables' the mod or game boots up and mod is already marked 'enabled' when this happens
        /// around the time of the main menu loading.
        /// </summary>
        public void OnEnabled()
        {
            try
            {
                isEnabled = true;
                ReloadConfiguationData();
                Logger.dbgLog(string.Format("{0} version {1} has been enabled. {2}", Mod.MOD_NAME, MOD_VERSIONSTRING, DateTime.Now.ToString()));
            }
            catch (Exception ex)
            { Logger.dbgLog("Error ", ex); }
        }


        /// <summary>
        /// Fire when user or game 'disables' mod.
        /// </summary>
        public void OnDisabled()
        {
            try
            {
                isEnabled = false;
                if (SettingsUI.panel != null)
                {
                    SettingsUI.panel.eventVisibilityChanged -= SettingsUI.SettingsEventVisibilityChanged;
                }

                Logger.dbgLog(MOD_NAME + " has been disabled.");
            }
            catch (Exception ex)
            { Logger.dbgLog("Error ", ex); }
        }

        public void OnSettingsUI(UIHelperBase helper) 
        {
            try
            {
                SettingsUI.BuildSettingsMenu(ref helper);
            }
            catch(Exception ex)
            { Logger.dbgLog("Error ",ex);}
        }

        private void UpdateConfig()
        {
 
        }

        /// <summary>
        /// Called to go fetch the information from the config file and populate\refresh our public vars.
        /// 
        /// </summary>
        /// <param name="bForceNewConfig"></param>
        private static void ReloadConfiguationData(bool bNoReReadConfig = false, bool bForceNewConfig = false)
        {
            try
            {
                if (bForceNewConfig) //manual create new one and save it.
                {
                    config = new Configuration();
                    Configuration.Serialize(MOD_CONFIGPATH, config);
                    if (DEBUG_LOG_ON) { Logger.dbgLog("New configuation file force created."); }
                }

                if (bNoReReadConfig == false)
                {
                    config = Configuration.Deserialize(MOD_CONFIGPATH);
                }
                if (config == null)
                {
                    //auto create new one and save it; we save it just so that we don't have to hit this again in the case some never touches 'options'.
                    //if save exceptions we in theory should still function due to setting things before the exception would happen - in theory.
                    config = new Configuration();
                    config.DebugLogging = false;
                    config.DebugLoggingLevel = 0;
                    config.TreeFireDisasterSpreadRate = 100;
                    config.TreeFireSpreadRate = 100;
                    config.DisableBuildingFires = false;
                    config.UseCustomLogFile = false;
                    Configuration.Serialize(MOD_CONFIGPATH, config);
                    if (DEBUG_LOG_ON) { Logger.dbgLog("New configuation file created."); }
                }
                if (config != null)
                {
                    Configuration.ValidateConfig(ref config);
                    DEBUG_LOG_ON = config.DebugLogging;
                    DEBUG_LOG_LEVEL = config.DebugLoggingLevel;
                }
                if (DEBUG_LOG_ON) { Logger.dbgLog("Configuration data loaded or refreshed."); }
            }
            catch (Exception ex)
            {
                Logger.dbgLog("ReloadConfig Exceptioned:", ex, true);
            }

        }


    }
}
