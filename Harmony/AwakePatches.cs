using HarmonyLib;
using ImprovedDebugConsole.Managers;
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
    [HarmonyPatch(typeof(TheForest.DebugConsole), nameof(DebugConsole.Awake))]
    internal class ConsoleUISetupPatches
    {
        public static void Postfix(TheForest.DebugConsole __instance)
        {
            if (AdvancedConsoleEngine.ActiveDescriptionText != null) return;

            TMP_Text originalLabel = __instance._autocompleteLabel;
            if (originalLabel == null) return;

            Transform consoleInputTextArea = originalLabel.transform.parent;
            Transform consoleInput = consoleInputTextArea.parent;
            Transform consoleLayout = consoleInput.parent;

            if (consoleLayout == null) return;

            GameObject containerObj = new GameObject("AdvancedConsoleDescContainer");
            RectTransform containerRect = containerObj.AddComponent<RectTransform>();
            UnityEngine.UI.LayoutElement layoutElement = containerObj.AddComponent<UnityEngine.UI.LayoutElement>();

            containerRect.sizeDelta = new Vector2(110f, 24f); 

            layoutElement.flexibleWidth = 1f;
            layoutElement.preferredWidth = 9999f;

            layoutElement.minHeight = 22f;
            layoutElement.preferredHeight = 22f;
            layoutElement.flexibleHeight = 0f;

            containerObj.transform.SetParent(consoleLayout, false);
            containerObj.transform.SetSiblingIndex(consoleInput.GetSiblingIndex() + 1);


            GameObject bgObj = new GameObject("AdvancedConsoleDescBg");
            RectTransform bgRect = bgObj.AddComponent<RectTransform>();
            bgObj.AddComponent<CanvasRenderer>();
            UnityEngine.UI.Image bgImage = bgObj.AddComponent<UnityEngine.UI.Image>();
            bgObj.transform.SetParent(containerObj.transform, false);

            bgRect.anchorMin = new Vector2(0, 0);
            bgRect.anchorMax = new Vector2(1, 1);
            bgRect.offsetMin = new Vector2(-2000f, 0); 
            bgRect.offsetMax = new Vector2(2000f, 0);  

            UnityEngine.UI.Image consoleImage = consoleInput.GetComponent<UnityEngine.UI.Image>();
            if (consoleImage != null)
            {
                bgImage.sprite = consoleImage.sprite;
                bgImage.color = consoleImage.color;
                bgImage.type = consoleImage.type;
                bgImage.pixelsPerUnitMultiplier = consoleImage.pixelsPerUnitMultiplier;
            }
            else
            {
                bgImage.color = new Color(0, 0, 0, 0.9f);
            }

            GameObject descObj = UnityEngine.Object.Instantiate(originalLabel.gameObject, containerObj.transform);
            descObj.name = "AdvancedConsoleDescriptionText";
            descObj.transform.SetAsLastSibling(); 

            TMP_Text descText = descObj.GetComponent<TMP_Text>();
            descText.text = "";
            descText.fontSize = originalLabel.fontSize * 0.75f;
            descText.fontStyle = FontStyles.Italic;
            descText.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            descText.alignment = TextAlignmentOptions.MidlineLeft;

            RectTransform descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0, 0);
            descRect.anchorMax = new Vector2(1, 1);
            descRect.pivot = new Vector2(0.5f, 0.5f);
            descRect.offsetMin = new Vector2(15f, 0);
            descRect.offsetMax = new Vector2(-15f, 0);

            AdvancedConsoleEngine.ActiveDescriptionText = descText;
        }
    }
}
