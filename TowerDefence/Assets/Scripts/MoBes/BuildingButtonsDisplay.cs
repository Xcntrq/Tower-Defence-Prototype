using nsBuildingPlacer;
using nsBuildingType;
using nsBuildingTypes;
using nsMouseEnterExit;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nsBuildingButtonsDisplay
{
    public class MouseEnterEventArgs : EventArgs
    {
        public BuildingType BuildingType { get; set; }
        public RectTransform PanelRectTransform { get; set; }
    }

    public class BuildingButtonsDisplay : MonoBehaviour
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private GameObject _pfItem;
        [SerializeField] private string _cancelText;
        [SerializeField] private Sprite _cancelSprite;
        [SerializeField] private Sprite _buttonDefaultSprite;
        [SerializeField] private Sprite _buttonPressedSprite;

        private Dictionary<BuildingType, GameObject> _items;
        private GameObject _lastItem;

        public event EventHandler<MouseEnterEventArgs> OnMouseEnter;
        public event Action OnMouseExit;

        private void OnEnable()
        {
            _buildingPlacer.OnCurrentBuildingTypeChange += BuildingPlacer_OnCurrentBuildingTypeChange;
        }

        private void OnDisable()
        {
            _buildingPlacer.OnCurrentBuildingTypeChange -= BuildingPlacer_OnCurrentBuildingTypeChange;
        }

        private void BuildingPlacer_OnCurrentBuildingTypeChange(BuildingType buildingType)
        {
            foreach (BuildingType key in _buildingTypes.List)
            {
                _items[key].GetComponent<Image>().sprite = _buttonDefaultSprite;
            }
            if (buildingType == null)
            {
                _lastItem.GetComponent<Button>().interactable = false;
            }
            else
            {
                _lastItem.GetComponent<Button>().interactable = true;
                _items[buildingType].GetComponent<Image>().sprite = _buttonPressedSprite;
            }
        }

        private void Awake()
        {
            int index = 1;
            _items = new Dictionary<BuildingType, GameObject>();
            foreach (BuildingType buildingType in _buildingTypes.List)
            {
                GameObject newItem = Instantiate(_pfItem, transform);
                //The picture of a building on top of the button is also an image, but it has a LayoutElement component, that's how to find it
                newItem.GetComponentInChildren<LayoutElement>().GetComponent<Image>().sprite = buildingType.Sprite;
                newItem.GetComponentInChildren<TextMeshProUGUI>().SetText(index++.ToString());
                newItem.GetComponent<Button>().onClick.AddListener(() => { _buildingPlacer.CurrentBuildingType = buildingType; });
                MouseEnterEventArgs mouseEnterEventArgs = new() { BuildingType = buildingType, PanelRectTransform = GetComponent<RectTransform>() };
                newItem.GetComponentInChildren<MouseEnterExit>().OnMouseEnter += (object sender, EventArgs e) => { OnMouseEnter?.Invoke(sender, mouseEnterEventArgs); };
                newItem.GetComponentInChildren<MouseEnterExit>().OnMouseExit += (object sender, EventArgs e) => { OnMouseExit?.Invoke(); };
                _items[buildingType] = newItem;
            }
            _lastItem = Instantiate(_pfItem, transform);
            _lastItem.GetComponentInChildren<LayoutElement>().GetComponent<Image>().sprite = _cancelSprite;
            _lastItem.GetComponentInChildren<LayoutElement>().GetComponent<AspectRatioFitter>().aspectRatio = 1;
            _lastItem.GetComponentInChildren<TextMeshProUGUI>().SetText(_cancelText);
            _lastItem.GetComponent<Button>().onClick.AddListener(() => { _buildingPlacer.CurrentBuildingType = null; });
        }
    }
}
