using RedLoader;
using SonsSdk.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;
using TheForest;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static ImprovedDebugConsole.Managers.AdvancedConsoleEngine;

namespace ImprovedDebugConsole.Managers
{
    internal class CustomCommandManager
    {
        public static void RegisterCommands()
        {
            RegisterCommand("removelight", new[] { Optional("all") }, "Removes the last spawned light, or type 'all' to remove them all.");
            RegisterCommand("strongcavelight", new[] { Optional(ArgFloatOff) }, "Enables a powerful cave light. Accepts an optional float multiplier, or 'off'.");
        }


        private static GameObject _customCaveLightGo;

        // Remove Light
        [DebugCommand("removelight")]
        private void RemoveLight(string amountArgs)
        {
            bool removeAll = amountArgs?.ToLower() == "all";
            int removedCount = 0;

            // Find, filter, and sort descending by name so 0002 comes before 0001
            var targetLights = UnityEngine.Object.FindObjectsOfType<Light>()
                .Where(l => l.gameObject.name.StartsWith("SpawnedLight"))
                .OrderByDescending(l => l.gameObject.name)
                .ToList();

            foreach (var light in targetLights)
            {
                UnityEngine.Object.Destroy(light.gameObject);
                removedCount++;
                if (!removeAll) break;
            }

            if (removedCount > 0)
                RLog.Msg($"[ImprovedDebugConsole] Removed {removedCount} spawned light(s).");
            else
                RLog.Msg("[ImprovedDebugConsole] No spawned lights found to remove.");
        }

        // Strong Cave Light
        [DebugCommand("strongcavelight")]
        private static void StrongCaveLight(string multiplierArg)
        {
            if (LocalPlayer.Transform == null)
            {
                RLog.Error("[ImprovedDebugConsole] Cannot create light: LocalPlayer is null.");
                return;
            }

            if (multiplierArg?.ToLower() == "off")
            {
                if (_customCaveLightGo) UnityEngine.Object.Destroy(_customCaveLightGo);
                RLog.Msg("[ImprovedDebugConsole] Strong Cave Light destroyed.");
                return;
            }

            float multiplier = 1f;
            if (!string.IsNullOrEmpty(multiplierArg))
            {
                float.TryParse(multiplierArg, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out multiplier);
            }

            if (_customCaveLightGo)
            {
                UnityEngine.Object.Destroy(_customCaveLightGo);
            }

            try
            {
                _customCaveLightGo = new GameObject("CustomStrongCaveLight");
                _customCaveLightGo.transform.parent = LocalPlayer.Transform;
                _customCaveLightGo.transform.position = LocalPlayer.Transform.position + Vector3.up * 2f + Vector3.forward * 0.2f;

                _customCaveLightGo.AddComponent<Light>().type = LightType.Point;
                HDAdditionalLightData hdLight = _customCaveLightGo.AddComponent<HDAdditionalLightData>();

                hdLight.luxAtDistance = 10f;
                hdLight.SetIntensity(5000f * multiplier, LightUnit.Lux);
                hdLight.SetRange(100f * multiplier);
                hdLight.affectsVolumetric = false;

                _customCaveLightGo.SetActive(true);
                RLog.Msg($"[ImprovedDebugConsole] Strong Cave Light enabled (Multiplier: {multiplier})");
            }
            catch (Exception ex)
            {
                RLog.Error($"[ImprovedDebugConsole] Error creating Strong Cave Light: {ex}");
            }
        }
    }
}
