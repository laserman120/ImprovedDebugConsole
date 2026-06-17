using HarmonyLib;
using ImprovedDebugConsole.Managers;
using RedLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest;
using TMPro;
using UnityEngine;

namespace ImprovedDebugConsole.Harmony
{
    [HarmonyPatch(typeof(TheForest.DebugConsole), nameof(DebugConsole.DrawConsole))]
    internal class RenderPatches
    {
        public static void Postfix(TheForest.DebugConsole __instance)
        {
            TMP_InputField inputField = __instance.GetComponentInChildren<TMP_InputField>();
            if (inputField == null || __instance._autocompleteLabel == null) return;

            string rawText = inputField.text;
            if (string.IsNullOrEmpty(rawText))
            {
                AdvancedConsoleEngine.ActiveDescriptionText.text = "";
                return;
            }

            if (!TryGetCommandInfo(rawText, out string commandTarget, out int activeArgIndex, out string currentTokenText, out bool hasSpace)) return;

            if (AdvancedConsoleEngine.CommandSignatures.TryGetValue(commandTarget, out AdvancedConsoleEngine.CommandArg[] args))
            {
                string previewText = BuildPreviewString(rawText, args, activeArgIndex, currentTokenText, hasSpace, __instance._autocomplete);
                __instance._autocompleteLabel.text = previewText;

                if (AdvancedConsoleEngine.ActiveDescriptionText != null)
                {
                    AdvancedConsoleEngine.ActiveDescriptionText.text = AdvancedConsoleEngine.CommandDescriptions.TryGetValue(commandTarget, out string desc) ? desc : "";
                }
            }
            else
            {
                if (AdvancedConsoleEngine.ActiveDescriptionText != null)
                {
                    AdvancedConsoleEngine.ActiveDescriptionText.text = "";
                }
            }
        }

        private static bool TryGetCommandInfo(string rawText, out string commandTarget, out int activeArgIndex, out string currentTokenText, out bool hasSpace)
        {
            commandTarget = string.Empty;
            activeArgIndex = 0;
            currentTokenText = string.Empty;
            hasSpace = rawText.Contains(" ");

            string cleanText = rawText.TrimStart();
            string[] tokens = cleanText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return false;

            commandTarget = tokens[0].ToLower();
            bool hasTrailingSpace = rawText.EndsWith(" ");

            activeArgIndex = tokens.Length - 1;
            if (hasTrailingSpace) activeArgIndex++;
            activeArgIndex--;

            currentTokenText = (hasTrailingSpace || activeArgIndex >= tokens.Length - 1) ? "" : tokens.Last();
            return true;
        }

        private static string BuildPreviewString(string rawText, AdvancedConsoleEngine.CommandArg[] args, int activeArgIndex, string currentTokenText, bool hasSpace, string autoComplete)
        {
            bool isTypingActiveArgument = !string.IsNullOrEmpty(currentTokenText);
            System.Text.StringBuilder preview = new System.Text.StringBuilder();

            bool hasGhostText = isTypingActiveArgument && !string.IsNullOrEmpty(autoComplete) && autoComplete.StartsWith(rawText, StringComparison.OrdinalIgnoreCase);

            if (hasGhostText)
            {
                preview.Append(rawText);
                string ghostText = autoComplete.Substring(rawText.Length);
                preview.Append($"<color=#80808066>{ghostText}</color>");
            }
            else
            {
                preview.Append(rawText);
            }

            if (!hasSpace)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    preview.Append($" <color=#80808066>{args[i].Label}</color>");
                }
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (i < activeArgIndex) continue;

                    if (i == activeArgIndex)
                    {
                        if (hasGhostText || isTypingActiveArgument) continue;
                        preview.Append($"<color=#FF8C0066>{args[i].Label}</color>");
                    }
                    else
                    {
                        preview.Append($" <color=#80808066>{args[i].Label}</color>");
                    }
                }
            }

            return preview.ToString();
        }
    }
}
