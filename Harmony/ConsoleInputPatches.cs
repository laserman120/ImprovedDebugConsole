using HarmonyLib;
using ImprovedDebugConsole.Managers;
using RedLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest;

namespace ImprovedDebugConsole.Harmony
{
    [HarmonyPatch(typeof(DebugConsole), nameof(DebugConsole.HandleConsoleInput))]
    public static class ConsoleFloatSanitizerPatch
    {
        public static void Prefix(ref string consoleInput)
        {
            if (string.IsNullOrWhiteSpace(consoleInput)) return;

            var partsList = new List<string>();
            bool inQuotes = false;
            string currentPart = "";
            foreach (char c in consoleInput)
            {
                if (c == '\"' || c == '\'') inQuotes = !inQuotes;

                if (c == ' ' && !inQuotes)
                {
                    if (!string.IsNullOrEmpty(currentPart)) partsList.Add(currentPart);
                    currentPart = "";
                }
                else
                {
                    currentPart += c;
                }
            }
            if (!string.IsNullOrEmpty(currentPart)) partsList.Add(currentPart);
            string[] parts = partsList.ToArray();
            if (parts.Length < 2) return;

            string commandName = parts[0];

            if (AdvancedConsoleEngine.CommandSignatures.TryGetValue(commandName, out var signature))
            {
                bool modified = false;

                for (int i = 0; i < signature.Length && (i + 1) < parts.Length; i++)
                {
                    if (AdvancedConsoleEngine.FloatTypes.Contains(signature[i].Name))
                    {
                        if (parts[i + 1].Contains("."))
                        {
                            parts[i + 1] = parts[i + 1].Replace('.', ',');
                            modified = true;
                        }
                    }
                }

                if (modified)
                {
                    consoleInput = string.Join(" ", parts);
                }
            }
        }
    }
}
