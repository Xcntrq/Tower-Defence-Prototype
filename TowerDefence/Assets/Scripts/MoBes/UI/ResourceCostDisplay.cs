using nsBuildingButtonsDisplay;
using nsPointerEnterExit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace nsResourceCostDisplay
{
    public class ResourceCostDisplay : MonoBehaviour
    {
        [SerializeField] private BuildingButtonsDisplay _buildingButtonsDisplay;

        private Dictionary<object, Vector2> _cachedPositions;
        private RectTransform _rectTransform;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _cachedPositions = new Dictionary<object, Vector2>();
            _rectTransform = GetComponent<RectTransform>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _buildingButtonsDisplay.OnMouseEnter += BuildingButtonsDisplay_OnMouseEnter;
            _buildingButtonsDisplay.OnMouseExit += BuildingButtonsDisplay_OnMouseExit;
            gameObject.SetActive(false);
        }

        private void BuildingButtonsDisplay_OnMouseEnter(object sender, MouseEnterEventArgs e)
        {
            Vector2 anchoredPosition;
            bool isCached = _cachedPositions.ContainsKey(sender);
            if (!isCached)
            {
                RectTransform buttonRectTransform = (sender as PointerEnterExit).GetComponent<RectTransform>();
                anchoredPosition = buttonRectTransform.anchoredPosition;
                anchoredPosition.x += e.PanelRectTransform.anchoredPosition.x;
                anchoredPosition.y += e.PanelRectTransform.rect.height;
                _cachedPositions[sender] = anchoredPosition;
            }
            else
            {
                anchoredPosition = _cachedPositions[sender];
            }
            _rectTransform.anchoredPosition = anchoredPosition;
            _text.SetText(e.Text);
            gameObject.SetActive(true);
        }

        private void BuildingButtonsDisplay_OnMouseExit()
        {
            gameObject.SetActive(false);
        }
    }
}
