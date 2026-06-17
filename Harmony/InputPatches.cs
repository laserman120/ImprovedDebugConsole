using HarmonyLib;
using ImprovedDebugConsole.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImprovedDebugConsole.Harmony
{
    [HarmonyPatch(typeof(TheForest.DebugConsole), "CommandAutoComplete")]
    internal class TabCompletionPatches
    {
        public static bool Prefix(TheForest.DebugConsole __instance)
        {
            TMP_InputField inputField = __instance.GetComponentInChildren<TMP_InputField>();
            if (inputField == null) return true;

            string rawText = inputField.text;
            if (!rawText.Contains(" ")) return true;

            string cleanText = rawText.TrimStart();
            string[] tokens = cleanText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return true;

            string commandTarget = tokens[0].ToLower();

            if (AdvancedConsoleEngine.CommandSignatures.TryGetValue(commandTarget, out AdvancedConsoleEngine.CommandArg[] argLabels))
            {
                var shortList = __instance._commandShortList;

                if (shortList != null && shortList.Count > 0)
                {
                    bool hasTrailingSpace = rawText.EndsWith(" ");
                    int activeArgIndex = tokens.Length - 1;
                    if (hasTrailingSpace) activeArgIndex++;
                    activeArgIndex--;

                    string currentTokenText = (hasTrailingSpace || activeArgIndex >= tokens.Length - 1) ? "" : tokens.Last();

                    string topMatch = shortList[0];

                    topMatch = topMatch.Replace("<color=#FF8C00ff>", "").Replace("</color>", "");

                    string baseContext = rawText;
                    if (!string.IsNullOrEmpty(currentTokenText))
                    {
                        baseContext = rawText.Substring(0, rawText.Length - currentTokenText.Length);
                    }

                    string fullCommand = baseContext + topMatch;

                    __instance._autocomplete = "";
                    __instance.SetConsoleInputValue(fullCommand, false);

                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(TheForest.DebugConsole), "BuildCommandShortList")]
    internal class InputPatches
    {
        public static void Postfix(TheForest.DebugConsole __instance)
        {
            TMP_InputField inputField = __instance.GetComponentInChildren<TMP_InputField>();
            if (inputField == null) return;

            string rawText = inputField.text;
            if (!rawText.Contains(" ")) return;

            string cleanText = rawText.TrimStart();
            string[] tokens = cleanText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return;

            string commandTarget = tokens[0].ToLower();

            if (Managers.AdvancedConsoleEngine.CommandSignatures.TryGetValue(commandTarget, out Managers.AdvancedConsoleEngine.CommandArg[] argLabels))
            {
                bool hasTrailingSpace = rawText.EndsWith(" ");
                int activeArgIndex = tokens.Length - 1;
                if (hasTrailingSpace) activeArgIndex++;
                activeArgIndex--;

                string currentTokenText = (hasTrailingSpace || activeArgIndex >= tokens.Length - 1) ? "" : tokens.Last();

                var rawPool = Managers.AdvancedConsoleEngine.GetRawPool(commandTarget, activeArgIndex);
                var shortList = __instance._commandShortList;

                if (shortList != null)
                {
                    shortList.Clear();

                    if (rawPool != null && rawPool.Count > 0)
                    {
                        var matches = rawPool.Where(item =>
                            string.IsNullOrEmpty(currentTokenText) ||
                            item.IndexOf(currentTokenText, StringComparison.OrdinalIgnoreCase) >= 0
                        ).ToList();

                        matches.Sort((a, b) =>
                        {
                            bool aStarts = a.StartsWith(currentTokenText, StringComparison.OrdinalIgnoreCase);
                            bool bStarts = b.StartsWith(currentTokenText, StringComparison.OrdinalIgnoreCase);

                            if (aStarts && !bStarts) return -1;
                            if (!aStarts && bStarts) return 1;

                            int lenCmp = a.Length.CompareTo(b.Length);
                            if (lenCmp != 0) return lenCmp;

                            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
                        });

                        foreach (var item in matches)
                        {
                            if (string.IsNullOrEmpty(currentTokenText))
                            {
                                shortList.Add(item);
                            }
                            else
                            {
                                int matchIdx = item.IndexOf(currentTokenText, StringComparison.OrdinalIgnoreCase);
                                if (matchIdx >= 0)
                                {
                                    string before = item.Substring(0, matchIdx);
                                    string match = item.Substring(matchIdx, currentTokenText.Length);
                                    string after = item.Substring(matchIdx + currentTokenText.Length);
                                    shortList.Add($"{before}<color=#FF8C00ff>{match}</color>{after}");
                                }
                                else
                                {
                                    shortList.Add(item);
                                }
                            }
                        }

                        if (matches.Count > 0)
                        {
                            string baseContext = rawText;
                            if (!string.IsNullOrEmpty(currentTokenText))
                            {
                                baseContext = rawText.Substring(0, rawText.Length - currentTokenText.Length);
                            }
                            __instance._autocomplete = baseContext + matches[0];
                        }
                        else
                        {
                            __instance._autocomplete = null;
                        }
                    }
                    else
                    {
                        __instance._autocomplete = null;
                    }
                }
            }
        }
    }
}
