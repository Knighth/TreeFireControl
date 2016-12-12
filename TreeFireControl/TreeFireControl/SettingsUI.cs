using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace TreeFireControl
{
    internal static class SettingsUI
    {
        private static UITextField m_FireSettingTxtUIref;
        internal static UISlider sliderFireRateValue;
        private static UITextField m_FireSettingDisasterTxtUIref;
        internal static UISlider sliderFireRateDisasterValue;
        internal static UILabel txtMessageState;
        internal static UIButton btnClearStats;
        internal static UIScrollablePanel panel;
        internal static bool isInGame = false;

        private static void OnLoggingChecked(bool en)
        {
            try
            {
                if (en)
                {
                    Mod.DEBUG_LOG_ON = true;
                    Mod.DEBUG_LOG_LEVEL = 2;
                    Mod.config.DebugLogging = Mod.DEBUG_LOG_ON;
                    Mod.config.DebugLoggingLevel = Mod.DEBUG_LOG_LEVEL;
                    Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config);
                    if (txtMessageState != null)
                    {
                        object[] tmpvars = new object[] { isInGame.ToString(), Detours.isActive.ToString(), TreeFireControl_Loader.FireStats.totalburncalls.ToString(), TreeFireControl_Loader.FireStats.totalburncallsnormal.ToString(), TreeFireControl_Loader.FireStats.totalburncallsdisaster.ToString(), TreeFireControl_Loader.FireStats.totalburncallsblockednormal.ToString(), TreeFireControl_Loader.FireStats.totalburncallsblockeddisaster.ToString() };
                        txtMessageState.text = string.Format("StateInfo: isInGame:{0} DetoursActive:{1}\n Total:{2} Totalnorm:{3} Totaldist:{4}\n Totalnormskipped:{5} Totaldistskipped:{6}", tmpvars);
                        if (!txtMessageState.isVisible)
                        { txtMessageState.Show(); }
                        if (!btnClearStats.isVisible)
                        { btnClearStats.Show(); }
                    }
                }
                else //off
                {
                    Mod.DEBUG_LOG_ON = false;
                    Mod.DEBUG_LOG_LEVEL = 0;
                    Mod.config.DebugLogging = Mod.DEBUG_LOG_ON;
                    Mod.config.DebugLoggingLevel = Mod.DEBUG_LOG_LEVEL;
                    Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config);
                    if (txtMessageState != null)
                    {
                        txtMessageState.Hide();
                        btnClearStats.Hide();
                    }
                }
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("User set logger to :" + en.ToString()); }

            }
            catch(Exception ex)
            { Logger.dbgLog("Error ", ex); }

        }

//        private static void OnTxtValueChanged(string thevalue)
//        {}
//        private static void OnTxtValueSubmitted(string thevalue)
//        { }
        private static void OnFireRateChanged(float thevalue)
        {
            try
            {
                if (m_FireSettingTxtUIref != null)
                {
                    m_FireSettingTxtUIref.text = thevalue.ToString();
                    if ((int)thevalue <= 0) { m_FireSettingTxtUIref.text = "Disabled"; }
                    if ((int)thevalue > 99) { m_FireSettingTxtUIref.text = "Original"; }
                }
                Mod.config.TreeFireSpreadRate = (int)thevalue;
                Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config);
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("User set normal tree fire rate to :" + thevalue.ToString()); }
            }
            catch(Exception ex)
            { Logger.dbgLog("Error ", ex); }

        }

        private static void OnFireRateChangedDisaster(float thevalue)
        {
            try
            {
                if (m_FireSettingDisasterTxtUIref != null)
                {
                    m_FireSettingDisasterTxtUIref.text = thevalue.ToString();
                    if ((int)thevalue <= 0) { m_FireSettingDisasterTxtUIref.text = "Disabled"; }
                    if ((int)thevalue > 99) { m_FireSettingDisasterTxtUIref.text = "Original"; }
                }
                Mod.config.TreeFireDisasterSpreadRate = (int)thevalue;
                Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config);
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("User set Disaster tree fire rate to :" + thevalue.ToString()); }
            }
            catch (Exception ex)
            { Logger.dbgLog("Error ", ex); }

        }

        private static void OnDisableFires(bool thevalue)
        {
            try
            {
                Mod.config.DisableBuildingFires = thevalue;
                if (isInGame && Singleton<BuildingManager>.exists)
                {
                    Singleton<BuildingManager>.instance.m_firesDisabled = thevalue;
                }
                Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config);
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("User set disable buildingfires to :" + thevalue.ToString());}
            }
            catch (Exception ex)
            { Logger.dbgLog("Error ", ex); }


        }

        internal static void SettingsEventVisibilityChanged(UIComponent component, bool value)
        {
            if (value) //.show
            {
                //update our data on screen cause OnSettingsUI only set it up the first time.
                if (txtMessageState != null)
                {
                    if (Mod.DEBUG_LOG_ON)
                    {
                        object[] tmpvars = new object[] { isInGame.ToString(), Detours.isActive.ToString(), TreeFireControl_Loader.FireStats.totalburncalls.ToString(), TreeFireControl_Loader.FireStats.totalburncallsnormal.ToString(), TreeFireControl_Loader.FireStats.totalburncallsdisaster.ToString(), TreeFireControl_Loader.FireStats.totalburncallsblockednormal.ToString(), TreeFireControl_Loader.FireStats.totalburncallsblockeddisaster.ToString() };
                        txtMessageState.text = string.Format("StateInfo: isInGame:{0} DetoursActive:{1}\n Total:{2} Totalnorm:{3} Totaldist:{4}\n Totalnormskipped:{5} Totaldistskipped:{6}", tmpvars);
                        if (!txtMessageState.isVisible)
                        { txtMessageState.Show(); }
                        if (!btnClearStats.isVisible)
                        { btnClearStats.Show(); }
                    }

                }
                if (m_FireSettingTxtUIref != null && m_FireSettingDisasterTxtUIref != null)
                {
                    m_FireSettingTxtUIref.text = Mod.config.TreeFireSpreadRate.ToString();
                    m_FireSettingDisasterTxtUIref.text = Mod.config.TreeFireDisasterSpreadRate.ToString();
                    if ((int)Mod.config.TreeFireSpreadRate <= 0) { m_FireSettingTxtUIref.text = "Disabled"; }
                    if ((int)Mod.config.TreeFireSpreadRate > 99) { m_FireSettingTxtUIref.text = "Original"; }
                    if ((int)Mod.config.TreeFireDisasterSpreadRate <= 0) { m_FireSettingDisasterTxtUIref.text = "Disabled"; }
                    if ((int)Mod.config.TreeFireDisasterSpreadRate > 99) { m_FireSettingDisasterTxtUIref.text = "Original"; }
                }
                sliderFireRateValue.value = (float)Mod.config.TreeFireSpreadRate;
                sliderFireRateDisasterValue.value = (float)Mod.config.TreeFireDisasterSpreadRate;
            }
//            else //hiding
//            {
//                Configuration.Serialize(Mod.MOD_CONFIGPATH, Mod.config);
//            }
        }


        //internal static void ClearStats_Clicked()
        internal static void ClearStats_Clicked(UIComponent component, UIMouseEventParameter p)
        {
            if (TreeFireControl_Loader.FireStats != null)
            {
                TreeFireControl_Loader.FireStats.clearstats();
            }
        }

        public static void BuildSettingsMenu(ref UIHelperBase helper)
        {
            try
            {
                UIHelper hp = (UIHelper)helper;
                panel = (UIScrollablePanel)hp.self;
                if (panel != null)
                {
                    panel.eventVisibilityChanged += SettingsEventVisibilityChanged;
                }
                UIHelperBase group = helper.AddGroup(Mod.MOD_NAME); //Title of your settings options panel, keep it short.
                group.AddSpace(10);
                sliderFireRateValue = (UISlider)group.AddSlider("Tree Fire Spread Rate", 0.0f, 100.0f, 5.0f, (float)Mod.config.TreeFireSpreadRate, OnFireRateChanged);
                m_FireSettingTxtUIref = (UITextField)group.AddTextfield("Rate", Mod.config.TreeFireSpreadRate.ToString(), delegate(string str) { }, delegate(string str) { });
                m_FireSettingTxtUIref.text = sliderFireRateValue.value.ToString();
                m_FireSettingTxtUIref.tooltip = "Current setting of the tree fire rate relative to the base game";
                group.AddSpace(16);

                sliderFireRateDisasterValue = (UISlider)group.AddSlider("Tree Fire Spread Rate (Disasters)", 0.0f, 100.0f, 5.0f, (float)Mod.config.TreeFireSpreadRate, OnFireRateChangedDisaster);
                group.AddSpace(10);
                m_FireSettingDisasterTxtUIref = (UITextField)group.AddTextfield("Rate", Mod.config.TreeFireDisasterSpreadRate.ToString(), delegate(string str) { }, delegate(string str) { });
                m_FireSettingDisasterTxtUIref.text = sliderFireRateDisasterValue.value.ToString();
                m_FireSettingDisasterTxtUIref.tooltip = "Current setting of the tree fire rate when part of disaster relative to the base game";
                if ((int)Mod.config.TreeFireSpreadRate <= 0) { m_FireSettingTxtUIref.text = "Disabled"; }
                if ((int)Mod.config.TreeFireSpreadRate > 99) { m_FireSettingTxtUIref.text = "Original"; }
                if ((int)Mod.config.TreeFireDisasterSpreadRate <= 0) { m_FireSettingDisasterTxtUIref.text = "Disabled"; }
                if ((int)Mod.config.TreeFireDisasterSpreadRate > 99) { m_FireSettingDisasterTxtUIref.text = "Original"; }
                group.AddSpace(12);
                UICheckBox chkDisableBuildingFires = (UICheckBox)group.AddCheckbox("Disable normal building fires", Mod.config.DisableBuildingFires, OnDisableFires);
                chkDisableBuildingFires.tooltip = "Will disable buildings from catching fire via normal methods, existing fires will continue till they burn out\n Does not effect disaster trigged building fires.\nThis was only included for convenience for people using no-fire mods to not have to have 2 mods";
                group.AddSpace(10);
                UICheckBox chkEnableLogging = (UICheckBox)group.AddCheckbox("Enable Logging", Mod.DEBUG_LOG_ON, OnLoggingChecked);
                chkEnableLogging.tooltip = "Enables logging of debug data to your log file.";
                group.AddSpace(20);
                UIButton btnResetAllTrees = (UIButton)group.AddButton("Extinguish All Tree Fires", ResetAllBurningTrees);
                btnResetAllTrees.tooltip = "Resets the flags on all trees to mark them not burned or damaged,\n then wipes the burningtree list, ground is not touched however.";
                group.AddSpace(12);
                UILabel txtMessage;
                txtMessage = panel.AddUIComponent<UILabel>();
                txtMessage.text = "Note all options can be changed during game play, and are effective immediately.\nYou must be in game obviously to use the Extinguish button.";
                group.AddSpace(10);
                UILabel txtMessage2;
                txtMessage2 = panel.AddUIComponent<UILabel>();
                txtMessage2.text = "Version: " + Mod.MOD_VERSIONSTRING; 
                group.AddSpace(12);
                txtMessageState = panel.AddUIComponent<UILabel>();
                object[] tmpvars = new object[] { isInGame.ToString(), Detours.isActive.ToString(), TreeFireControl_Loader.FireStats.totalburncalls.ToString(), TreeFireControl_Loader.FireStats.totalburncallsnormal.ToString(), TreeFireControl_Loader.FireStats.totalburncallsdisaster.ToString(), TreeFireControl_Loader.FireStats.totalburncallsblockednormal.ToString(), TreeFireControl_Loader.FireStats.totalburncallsblockeddisaster.ToString() };
                txtMessageState.text = string.Format("StateInfo: isInGame:{0} DetoursActive:{1}\n Total:{2} Totalnorm:{3} Totaldist:{4}\n Totalnormskipped:{5} Totaldistskipped:{6}", tmpvars);
                txtMessageState.Hide();
 //               panel.autoLayout = false;
//                btnClearStats = (UIButton)group.AddButton("Clear Stats", ClearStats_Clicked);
                btnClearStats =  (UIButton)panel.AddUIComponent<UIButton>();
                btnClearStats.autoSize = false;
                btnClearStats.size = new Vector2(130, 28);
                btnClearStats.normalBgSprite = "ButtonMenu";
                btnClearStats.hoveredTextColor = new Color32(7, 137, 255, 255);
                btnClearStats.pressedTextColor = new Color32(30,30,44,255);
                btnClearStats.playAudioEvents = true;
//                panel.AddUIComponent<UIButton>();
                btnClearStats.text = "ClearStats";
                btnClearStats.eventClick += ClearStats_Clicked;
//                btnClearStats.relativePosition = new Vector3(txtMessageState.relativePosition.x, txtMessageState.relativePosition.y + 42f);
                btnClearStats.Hide();
                if (Mod.DEBUG_LOG_ON)
                { txtMessage.Show(); btnClearStats.Show(); }
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("UI setup completed"); }
            }
            catch(Exception ex)
            { Logger.dbgLog("Error ", ex); }

            //group.AddCheckbox("Auto Show On Map Load", config.UseAlternateKeyBinding, OnUseAlternateKeyBinding); //<-- last part is function you want called when clicked\unclicked.
            //group.AddCheckbox("Use Alternate Keybinding", config.UseAlternateKeyBinding, OnUseAlternateKeyBinding); //<-- last part is function you want called when clicked\unclicked.
            //group.AddCheckbox("Enable Verbose Logging", DEBUG_LOG_ON, OnLoggingChecked); //<-- last part is function you want called when clicked\unclicked. 

        }


        private static void ResetAllBurningTrees()
        {
            if (isInGame == false) { return; }

            try
            {
                TreeManager tm = Singleton<TreeManager>.instance;
                TreeInstance.Flags tflags;
                ushort sourceFlags = 0;
                int counter = 0; int counter2 = 0;
                for (int i = 1; i < tm.m_trees.m_buffer.Length; i++)
                {
                    sourceFlags = tm.m_trees.m_buffer[i].m_flags;
                    tflags = (TreeInstance.Flags)sourceFlags;
                    if (HasTreeFlags(tflags, TreeInstance.Flags.Created) && (HasTreeFlags(tflags, TreeInstance.Flags.Burning) | HasTreeFlags(tflags, TreeInstance.Flags.FireDamage)))
                    {
                        tflags &= ~(TreeInstance.Flags.Burning | TreeInstance.Flags.FireDamage);
                        tm.m_trees.m_buffer[i].m_flags = (ushort)tflags;
                        counter++;
                    }
                }

//                if (ClearBurningBufferToo)
//                {
                      counter2 = tm.m_burningTrees.m_size;
                      tm.m_burningTrees.Clear();
                      tm.m_burningTrees.Trim();
//                }
                Logger.dbgLog(string.Format("User reset all {0} burning trees, and {1} in the burningfastlist.", counter.ToString(), counter2.ToString()));
            }
            catch (Exception ex)
            {
                Logger.dbgLog("Error: ", ex);
            }


        }

        public static bool HasTreeFlags(TreeInstance.Flags somevalue, TreeInstance.Flags flagtocheck)
        {
            return (somevalue & flagtocheck) == flagtocheck;
        }

    }
}
