using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest;

namespace ImprovedDebugConsole.Harmony
{
    [HarmonyPatch(typeof(TheForest.ConsoleInputAutoCompleteColumn), nameof(ConsoleInputAutoCompleteColumn.AtCapacity))]
    internal class ColumnCapacityPatch
    {
        public static void Postfix(TheForest.ConsoleInputAutoCompleteColumn __instance, ref bool __result)
        {
            if (__result) return;

            if (__instance._commandTextsInstances != null && __instance._commandTextsInstances.Count >= 33)
            {
                bool hasInactive = __instance.TryGetFromPool(out var dummy);

                if (!hasInactive)
                {
                    __result = true;
                }
            }
        }
    }
}
