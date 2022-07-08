using HarmonyLib;
using LSTY.Sdtd.PatronsMod;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSTY.Sdtd.PatronsMod.HarmonyPatchers
{
    /// <summary>
    /// Fix loading type failure error
    /// </summary>
    [HarmonyPatch(typeof(AstarData), nameof(AstarData.FindGraphTypes))]
    public static class AstarData_FindGraphTypes_Patcher
    {
        [HarmonyPrefix]
        public static bool Prefix(AstarData __instance)
        {
            var list = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                try
                {
                    foreach (Type type in assemblies[i].GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(NavGraph)))
                        {
                            list.Add(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CustomLogger.Warn(ex, "Load assembly {0} failed", assemblies[i].Location);
                }
            }

            var setMethod = typeof(AstarData).GetProperty(nameof(AstarData.graphTypes), BindingFlags.Public | BindingFlags.Instance).SetMethod;
            setMethod.Invoke(__instance, new object[] { list.ToArray() });

            return false;
        }
    }
}
