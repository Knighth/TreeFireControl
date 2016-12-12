using ColossalFramework;
using ColossalFramework.IO;
using ColossalFramework.UI;
using ColossalFramework.Plugins;
using ColossalFramework.Threading;
using ICities;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace TreeFireControl
{
	public class TreeFireControl_Loader : LoadingExtensionBase
	{
        //public static UIView parentGuiView;     //this holds our refference to the game main UIView object.
        //public static TreeFireControlGUI guiPanel;  //this holds our refference to our actual gui object.
        internal static bool isGuiRunning = false; //this var is set to know if our gui is actually running\is setup
        
        internal static LoadMode CurrentLoadMode;
        internal static TFCStats FireStats = new TFCStats();
        
        public TreeFireControl_Loader() { }

        /// <summary>
        /// As of v1.6 fired When deseralization of all IDataContainers.Deserialize and AfterDeserialize() calls.
        /// in the game have completed. In 1.5 it used to fire earlier it seemed or was at least more deterministic.
        /// </summary>
        /// <param name="loading">the games 'loading' object which doesn't have much use this early</param>
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            try
            {
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("Reloading config before mapload."); }
                TFCHelper.ReloadConfigValues(false, false);
            }
            catch (Exception ex)
            { Logger.dbgLog("Error:", ex, true); }
        }


        /// <summary>
        /// </summary>
        /// <param name="mode">a LoadMode enum (ie newgame,newmap,loadgame,loadmap,newasset,loadassett)</param>
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode); 
            CurrentLoadMode = mode; //save this guy for later.
            try
            {
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("LoadMode:" + mode.ToString()); }
                
                if (Mod.isEnabled == true)
                {
                    // only setup gui when in a real game, not in the asset editor
                    if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame || (int)mode == 11)
                    {
                        if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("Game modes detected, setting up detours."); }
                        SettingsUI.isInGame = true;
                        if (FireStats == null) FireStats = new TFCStats();
                        if (Singleton<LoadingManager>.instance.SupportsExpansion(Expansion.NaturalDisasters))
                        {
                            TreeFireControl.Detours.DeployRedirects(); //only deploy if user has NaturalDisasters.
                        }
                        if (Mod.config.DisableBuildingFires)
                        {
                            if (Singleton<BuildingManager>.exists)
                            {
                                Singleton<BuildingManager>.instance.m_firesDisabled = true;
                                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("BuildingManager.m_firesDisabled set to true"); }
                            }
                        }
                    }
                }
                else
                {
                    //This should technically never happen, but why not account for it anyway.
                    if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("We fired when we were not even enabled active??"); }
                    TreeFireControl.Detours.RemoveRedirects();
                    SettingsUI.isInGame = false;
                }
            }
            catch(Exception ex)
            { Logger.dbgLog("Error:", ex, true); }
        }


        /// <summary>
        /// Fire when map unloading.
        /// </summary>
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();  //see prior comments same concept.
            try
            {
                if(Mod.isEnabled)
                {
                    if (FireStats != null)
                    {
                        FireStats.clearstats();
                    }
                    TreeFireControl.Detours.RemoveRedirects();
                }
                SettingsUI.isInGame = false;

            }
            catch (Exception ex1)
            {
                Logger.dbgLog("Error: \r\n", ex1, true);
            }


        }

        /// <summary>
        /// Fire when released to mainmenu or desktop
        /// </summary>
        public override void OnReleased()
        {
            base.OnReleased();
            try
            {
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("Releasing Completed."); }
            }
            catch (Exception ex1)
            {
                Logger.dbgLog("Error: \r\n", ex1, true);
            }
        }


/*
        /// <summary>
        /// Our private little function to do whatever we need to setup and initialize our Gui screen object.
        /// </summary>
        private static void SetupGui()
        {
            if (Mod.DEBUG_LOG_ON) Logger.dbgLog(" Setting up Gui panel.");
            try
            {
                parentGuiView = null; //make sure we start fresh, even though we could just assume that was set during the last map unload. 
                parentGuiView = UIView.GetAView(); //go get the root screen\view object from Unity via Colosalframework.ui function.

                //if our object is null (it should be) then lets create one, have the game ADD it, and store it and set our isGUIrunning flag. 
                if (guiPanel == null)
                {
                    guiPanel = (TreeFireControlGUI)parentGuiView.AddUIComponent(typeof(TreeFireControlGUI));
                    if (Mod.DEBUG_LOG_ON) Logger.dbgLog(" GUI Created.");
                }
                isGuiRunning = true;
            }
            catch (Exception ex)
            {
                Logger.dbgLog("Error: \r\n", ex,true);
            }

        }
*/

/*
        /// <summary>
        /// Our private little function to do whatever we need to un-setup and un-initialize our Gui screen object.
        /// </summary>
        private static void RemoveGui()
        {

            if (Mod.DEBUG_LOG_ON) Logger.dbgLog(" Removing Gui.");
            try
            {
                if (guiPanel != null)
                {
                    // I've seen some people try to clean up their gui objects with code like this.
                    // I could be wrong but it seem unneccessary in most cases, but I leave it as an example
                    // of something to do in 'removing' your gui object. 
                    // Frankly it seems to me the game cleans these up for you anyway, but in theory doesn't
                    // hurt to do it yourself to maybe trigger garbage collection faster.
                    guiPanel.gameObject.SetActive(false);
                    GameObject.DestroyImmediate(guiPanel.gameObject);
                    guiPanel = null;
                    if (Mod.DEBUG_LOG_ON) Logger.dbgLog("Destroyed GUI objects.");
                }
            }
            catch (Exception ex)
            {
                Logger.dbgLog("Error: ",ex,true);
            }

            isGuiRunning = false;
            if (parentGuiView != null) { parentGuiView = null; } //destroy our reference to primary guiview
        }
*/
	}
}
