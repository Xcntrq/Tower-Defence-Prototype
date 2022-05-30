using nsBuildingButtonsDisplay;
using nsResourceCost;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace nsResourceCostDisplayText
{
    public class ResourceCostDisplayText : MonoBehaviour
    {
        [SerializeField] private BuildingButtonsDisplay _buildingButtonsDisplay;

        private Dictionary<object, string> _cachedTexts;
        private TextMeshProUGUI _text;

        private void OnEnable()
        {
            _buildingButtonsDisplay.OnMouseEnter += BuildingButtonsDisplay_OnMouseEnter;
            _buildingButtonsDisplay.OnMouseExit += BuildingButtonsDisplay_OnMouseExit;
        }

        private void OnDisable()
        {
            _buildingButtonsDisplay.OnMouseEnter -= BuildingButtonsDisplay_OnMouseEnter;
            _buildingButtonsDisplay.OnMouseExit -= BuildingButtonsDisplay_OnMouseExit;
        }

        private void BuildingButtonsDisplay_OnMouseEnter(object sender, MouseEnterEventArgs e)
        {
            string text;
            bool isCached = _cachedTexts.ContainsKey(sender);
            if (!isCached)
            {
                text = e.BuildingType.Name;
                foreach (ResourceCost resourceCost in e.BuildingType.ResourceCosts)
                {
                    text += "<br>";
                    string color = ColorUtility.ToHtmlStringRGB(resourceCost.ResourceType.Color);
                    string name = resourceCost.ResourceType.Name;
                    int cost = resourceCost.Value;
                    text += name + ": <color=#" + color + ">" + cost + "</color>";
                }
                _cachedTexts[sender] = text;
            }
            else
            {
                text = _cachedTexts[sender];
            }
            _text.SetText(text);
        }

        private void BuildingButtonsDisplay_OnMouseExit()
        {
            _text.SetText("");
        }

        private void Awake()
        {
            _cachedTexts = new Dictionary<object, string>();
            _text = GetComponent<TextMeshProUGUI>();
            _text.SetText("");
        }
    }
}
