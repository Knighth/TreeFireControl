using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CitiesSkylinesDetour;

namespace TreeFireControl
{
    static class Detours
    {
        private static bool m_isActive = false;
        internal static Dictionary<MethodInfo, RedirectCallsState> redirectDic = new Dictionary<MethodInfo, RedirectCallsState>();

        public static bool isActive
        {
            get{return m_isActive;}
        }


                /// <summary>
        /// This guy is our wrapper to doing the detours. it does the detour and then adds the returned
        /// RedirectCallState object too our dictionary for later reversal.
        /// </summary>
        /// <param name="type1">The original type of the method we're detouring</param>
        /// <param name="type2">Our replacement type of the method we're detouring</param>
        /// <param name="p">The original method\function name</param>
        private static void RedirectCalls(Type type1, Type type2, string p)
        {
            var bindflags1 = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var bindflags2 = BindingFlags.Static | BindingFlags.Public;
            var theMethod = type1.GetMethod(p, bindflags1);
            //var replacementMethod = type2.GetMethod(p,bindflags2);
            //if (theMethod == null || replacementMethod == null)
            //{
            //    Logger.dbgLog("Failed to locate function: " + p + ((theMethod == null) ? "  orignal":"  replacement"));
            //}
            //if (Mod.DEBUG_LOG_ON)
            //{
                //redirectDic.Add(theMethod, RedirectionHelper.RedirectCalls(theMethod, type2.GetMethod(p, bindflags2), true)); //makes the actual detour and stores the callstate info.
            //}
            //else 
            //{
                redirectDic.Add(theMethod, RedirectionHelper.RedirectCalls(theMethod, type2.GetMethod(p, bindflags2), false)); //makes the actual detour and stores the callstate info.
            //}

                //if (Mod.DEBUG_LOG_ON)
                //{
                    //Logger.dbgLog(p.ToString() + " redirected");
                //}

            //RedirectionHelper.RedirectCalls(type1.GetMethod(p, bindflags1), type2.GetMethod(p, bindflags2), false);
        }



        /// <summary>
        /// Sets up our redirects of our replacement methods.
        /// </summary>
        public static void DeployRedirects()
        {
            if (m_isActive) 
            { 
                Logger.dbgLog("Redirects already active.");
                return; 
            }

            try
            {
                RedirectCalls(typeof(TreeManager), typeof(TFCTreeManager), "BurnTree");
//                RedirectCalls(typeof(BuildingDecoration), typeof(LimitBuildingDecoration), "ClearDecorations");
//                RedirectCalls(typeof(BuildingDecoration), typeof(LimitBuildingDecoration), "SaveProps");
//                RedirectCalls(typeof(NaturalResourceManager), typeof(LimitNaturalResourceManager), "TreesModified");
//                RedirectCalls(typeof(TreeTool), typeof(LimitTreeTool), "ApplyBrush");

               
//                RedirectCalls(typeof(TreeManager.Data), typeof(LimitTreeManager.Data), "Deserialize");
//                RedirectCalls(typeof(TreeManager.Data), typeof(LimitTreeManager.Data), "Serialize");
//                RedirectCalls(typeof(TreeManager.Data), typeof(LimitTreeManager.Data), "AfterDeserialize");

/*
                MethodInfo[] methods = typeof(LimitTreeManager).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
                for (int i = 0; i < (int)methods.Length; i++)
                {
                    MethodInfo methodInfo = methods[i];
                    RedirectCalls(typeof(TreeManager), typeof(LimitTreeManager), methodInfo.Name);
                }
*/
 //               RedirectCalls(typeof(LimitCommonBuildingAI), typeof(CommonBuildingAI), "TrySpreadFire");

//                RedirectCalls(typeof(CommonBuildingAI), typeof(LimitCommonBuildingAI), "HandleFireSpread");
//                RedirectCalls(typeof(DisasterHelpers), typeof(LimitDisasterHelpers), "DestroyTrees");
//                RedirectCalls(typeof(FireCopterAI), typeof(LimitFireCopterAI), "FindBurningTree");
//                RedirectCalls(typeof(ForestFireAI), typeof(LimitForestFireAI), "FindClosestTree");


                //If windoverride enabled, otherwise don't.
//                if (USE_NO_WINDEFFECTS){RedirectCalls(typeof(WeatherManager), typeof(LimitWeatherManager), "CalculateSelfHeight");}

                m_isActive = true;
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("Redirected calls."); }
                if (Mod.DEBUG_LOG_ON && Mod.DEBUG_LOG_LEVEL >1) 
                {
                    foreach (var keypair in redirectDic)
                    {
                        Logger.dbgLog(string.Format("Detoured: {0} ref:{1}", keypair.Key.Name ,keypair.Value.f.ToString()));
                    }
                }
            }
            catch (Exception exception1)
            {
                Logger.dbgLog("Setup error:",exception1,true);
            }
        }

        /// <summary>
        /// Reverses our redirects from ours back to C/O's
        /// </summary>
        public static void RemoveRedirects()
        {
            if (m_isActive == false) 
            {
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("Redirects are not active no need to reverse."); }
                return; 
            }
            if (redirectDic.Count == 0)
            {
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("No state entries exists to revert. clearing state?"); }
                m_isActive = false; 
                return;
            }
            try
            {
                foreach (var keypair in redirectDic)
                {
                    RedirectionHelper.RevertRedirect(keypair.Key, keypair.Value);
                }
                redirectDic.Clear();
                m_isActive = false;
                if (Mod.DEBUG_LOG_ON) { Logger.dbgLog("Reverted redirected calls."); }
            }
            catch (Exception exception1)
            { Logger.dbgLog("ReverseSetup error:",exception1,true); }
        }

    }
  
}
