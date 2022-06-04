using nsBuildingButtonsDisplay;
using nsColorValue;
using nsPointerEnterExit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nsResourceCostDisplay
{
    public class ResourceCostDisplay : MonoBehaviour
    {
        [SerializeField] private BuildingButtonsDisplay _buildingButtonsDisplay;
        [SerializeField] private ColorValue _transparentColor;

        private Dictionary<object, Vector2> _cachedPositions;
        private RectTransform _rectTransform;
        private Image _image;
        private Color _defaultColor;

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
            _image.color = _defaultColor;
        }

        private void BuildingButtonsDisplay_OnMouseExit()
        {
            _image.color = _transparentColor.Value;
        }

        private void Awake()
        {
            _cachedPositions = new Dictionary<object, Vector2>();
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _defaultColor = _image.color;
            _image.color = _transparentColor.Value;
        }
    }
}
