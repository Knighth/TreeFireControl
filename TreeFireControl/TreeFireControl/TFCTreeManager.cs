using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework;
using ICities;
using UnityEngine;

namespace TreeFireControl
{
    internal static class TFCTreeManager
    {
        public static bool BurnTree(TreeManager tm,uint treeIndex, InstanceManager.Group group, int fireIntensity)
        {
            unsafe
            {
                //TreeManager tm = Singleton<TreeManager>.instance; see above {this} is passed in bydefault by .net as secret first param.
                TreeManager.BurningTree burningTree = new TreeManager.BurningTree();
                //if (Mod.DEBUG_LOG_ON && Mod.DEBUG_LOG_LEVEL > 1)
                //{ Logger.dbgLog("request to burn tree: " + treeIndex.ToString() ); }
                if (treeIndex == 0 || (tm.m_trees.m_buffer[treeIndex].m_flags & 64) != 0)
                {
                    return false;
                }
                if (!Singleton<LoadingManager>.instance.SupportsExpansion(Expansion.NaturalDisasters))
                {
                    return false;
                }
                TreeFireControl_Loader.FireStats.totalburncalls ++;
                //our additions
                if (Mod.DEBUG_LOG_ON && Mod.DEBUG_LOG_LEVEL > 2)
                { Logger.dbgLog(string.Format("ratetree:{0} ratedistree:{1} fireIntensity:{2}", Mod.config.TreeFireSpreadRate.ToString(), Mod.config.TreeFireDisasterSpreadRate.ToString(), fireIntensity.ToString())); }

                
                if (group != null)
                {
                    ushort disaster = group.m_ownerInstance.Disaster;
                    if (disaster != 0)
                    {
                        if (Mod.DEBUG_LOG_ON && Mod.DEBUG_LOG_LEVEL > 2)
                        {
                            Logger.dbgLog("burn req is connected to disaster");
                        }
                        TreeFireControl_Loader.FireStats.totalburncallsdisaster++;
                        if (TreeFireControl.TFCTreeManager.Utils.ShouldWeBurnit(ref group, Mod.config.TreeFireDisasterSpreadRate))
                        {
                            //do nothing == burn tree. and count it toward disaster. 
                                Singleton<DisasterManager>.instance.m_disasters.m_buffer[disaster].m_treeFireCount = Singleton<DisasterManager>.instance.m_disasters.m_buffer[disaster].m_treeFireCount + 1;
                            //end org
                        }
                        else
                        {
                            //retruned false - block the burn call. 
                            TreeFireControl_Loader.FireStats.totalburncallsblockeddisaster ++;
                            return false;
                        }
                    }
                    else //Was not null but not a valid disaster id. handle like normal. 
                    {
                        if (Mod.DEBUG_LOG_ON && Mod.DEBUG_LOG_LEVEL > 2)
                        {
                            Logger.dbgLog("burn req is *not* connected to disaster.");
                        }
                        TreeFireControl_Loader.FireStats.totalburncallsnormal ++;
                        if (!TreeFireControl.TFCTreeManager.Utils.ShouldWeBurnit(ref group, Mod.config.TreeFireSpreadRate))
                        {
                            TreeFireControl_Loader.FireStats.totalburncallsblockednormal++;
                            return false; //no burning. //else let it continue and burn the tree
                        }
                    }

                }
                else //not tied to disasters.
                {
                    if (Mod.DEBUG_LOG_ON && Mod.DEBUG_LOG_LEVEL > 2)
                    {
                        Logger.dbgLog("burn req is *not* connected to disaster. group null");
                    }
                    TreeFireControl_Loader.FireStats.totalburncallsnormal++;
                    if (!TreeFireControl.TFCTreeManager.Utils.ShouldWeBurnit(ref group, Mod.config.TreeFireSpreadRate))
                    {
                        TreeFireControl_Loader.FireStats.totalburncallsblockednormal++;
                        return false; //no burning. //else let it continue
                    }
                    //burnlog;
                }

                //cont org
                burningTree.m_treeIndex = treeIndex;
                burningTree.m_fireIntensity = (byte)fireIntensity;
                burningTree.m_fireDamage = 4;
                InstanceID instanceID = new InstanceID()
                {
                    Tree = treeIndex
                };
                Singleton<InstanceManager>.instance.SetGroup(instanceID, group);
                tm.m_trees.m_buffer[treeIndex].m_flags = (ushort)(tm.m_trees.m_buffer[treeIndex].m_flags | 192);
                tm.m_burningTrees.Add(burningTree);
                return true;
            }
        }

        public static class Utils 
        {
            public static bool ShouldWeBurnit(ref InstanceManager.Group group, int rate)
            {
                try
                {
                    if (rate == 0)
                    {
                        //if (Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL > 1)
                        //{
                        //    Logger.dbgLog("rate 0 abort burn");
                        //}
                        return false; 
                    } //disabled and no disaster override matters.
                    else if(rate == 100) //we're not in use, burn.
                    {
                        //if (Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL > 1)
                        //{
                        //    Logger.dbgLog("rate 100 -> burn");
                        //}
                        return true;
                    }

                    //Allow for\game or user to still "start" the first tree burning of a disaster so long as we're not disabled.
                    if (group != null)
                    {
                        ushort disaster = group.m_ownerInstance.Disaster;
                        if (disaster != 0)
                        {
                            if (Singleton<DisasterManager>.instance.m_disasters.m_buffer[disaster].m_treeFireCount < 1)
                            {
                                return true;  //allow the very first tree to burn.
                            }
                        }
                    }

                    int tmpint = 100; //holds our random value.
                    tmpint = SimulationManager.instance.m_randomizer.Int32(100);
                    if (rate <= tmpint) //if true we burn. // 80 < 100; 80% of the time we will burn, 20% fail rate.
                    {
                        if(Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL >2)
                        { Logger.dbgLog(string.Format(" tree rate:{0} <eq {1} true. return no burn", rate.ToString(), tmpint.ToString())); }

                        return false;
                    }
                    else
                    {
                        if (Mod.DEBUG_LOG_ON & Mod.DEBUG_LOG_LEVEL > 2)
                        { Logger.dbgLog(string.Format("rate:{0} NOT <eq {1} . returning burn.", rate.ToString(), tmpint.ToString())); }
                        return true;
                    }
                }
                catch (Exception ex) { Logger.dbgLog("", ex); }
                return true;  //if error burn.
            }
        
        }
    }
}
