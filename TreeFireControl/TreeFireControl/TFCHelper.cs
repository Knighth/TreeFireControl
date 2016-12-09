//using ICities;
//using ColossalFramework.IO;
//using ColossalFramework.Plugins;
//using ColossalFramework.Packaging;
//using ColossalFramework.Steamworks;
//using ColossalFramework;
//using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TreeFireControl
{
    public class TFCHelper
    {

        /// <summary>
        /// Called to either initially load, or force a reload our config file var; called by mod initialization and again at mapload. 
        /// </summary>
        /// <param name="bForceReread">Set to true to flush the old object and create a new one.</param>
        /// <param name="bNoReloadVars">Set this to true to NOT reload the values from the new read of config file to our class level counterpart vars</param>
        public static void ReloadConfigValues(bool bForceReread, bool bNoReloadVars)
        {
            try
            {

                if (bForceReread)
                {
                    Mod.config = null;
                    if (Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL >= 1) { Logger.dbgLog("Config wipe requested."); }
                }
                Mod.config = Configuration.Deserialize(Mod.MOD_CONFIGPATH);
                if (Mod.config == null)
                {
                    Mod.config = new Configuration();
                    //reset of setting should pull defaults
                    Logger.dbgLog("Existing config was null. Created new one.");
                    Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config); //let's write it.
                }
                if (Mod.config != null && bNoReloadVars == false) //set\refresh our vars by default.
                {
                    Mod.DEBUG_LOG_ON = Mod.config.DebugLogging;
                    Mod.DEBUG_LOG_LEVEL = Mod.config.DebugLoggingLevel;
                    if (Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL >= 2) { Logger.dbgLog("Vars refreshed"); }
                }
                if (Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL >= 2) { Logger.dbgLog(string.Format("Reloaded Config data ({0}:{1})", bForceReread.ToString(), bNoReloadVars.ToString() )); }
            }
            catch (Exception ex)
            { Logger.dbgLog("Exception while loading config values.", ex, true); }

        }


    }

}
