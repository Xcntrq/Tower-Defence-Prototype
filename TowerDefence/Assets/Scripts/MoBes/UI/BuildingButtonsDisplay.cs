using nsBuilding;
using nsBuildingPlacer;
using nsBuildings;
using nsBuildingType;
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
        [SerializeField] private Buildings _buildings;
        [SerializeField] private GameObject _pfItem;
        [SerializeField] private string _cancelText;
        [SerializeField] private Sprite _cancelSprite;
        [SerializeField] private Sprite _buttonDefaultSprite;
        [SerializeField] private Sprite _buttonPressedSprite;

        private Dictionary<Building, GameObject> _items;
        private GameObject _lastItem;

        public event EventHandler<MouseEnterEventArgs> OnMouseEnter;
        public event Action OnMouseExit;

        private void OnEnable()
        {
            _buildingPlacer.OnCurrentBuildingChange += BuildingPlacer_OnCurrentBuildingChange;
        }

        private void OnDisable()
        {
            _buildingPlacer.OnCurrentBuildingChange -= BuildingPlacer_OnCurrentBuildingChange;
        }

        private void BuildingPlacer_OnCurrentBuildingChange(Building building)
        {
            foreach (Building key in _buildings.List)
            {
                _items[key].GetComponent<Image>().sprite = _buttonDefaultSprite;
            }
            if (building == null)
            {
                _lastItem.GetComponent<Button>().interactable = false;
            }
            else
            {
                _lastItem.GetComponent<Button>().interactable = true;
                _items[building].GetComponent<Image>().sprite = _buttonPressedSprite;
            }
        }

        private void Awake()
        {
            int index = 1;
            _items = new Dictionary<Building, GameObject>();
            foreach (Building building in _buildings.List)
            {
                GameObject newItem = Instantiate(_pfItem, transform);
                //The picture of a building on top of the button is also an image, but it has a LayoutElement component, that's how to find it
                newItem.GetComponentInChildren<LayoutElement>().GetComponent<Image>().sprite = building.BuildingType.Sprite;
                newItem.GetComponentInChildren<TextMeshProUGUI>().SetText(index++.ToString());
                newItem.GetComponent<Button>().onClick.AddListener(() => { _buildingPlacer.CurrentBuilding = building; });
                MouseEnterEventArgs mouseEnterEventArgs = new() { BuildingType = building.BuildingType, PanelRectTransform = GetComponent<RectTransform>() };
                newItem.GetComponent<MouseEnterExit>().OnMouseEnter += (object sender, EventArgs e) => { OnMouseEnter?.Invoke(sender, mouseEnterEventArgs); };
                newItem.GetComponent<MouseEnterExit>().OnMouseExit += (object sender, EventArgs e) => { OnMouseExit?.Invoke(); };
                _items[building] = newItem;
            }
            _lastItem = Instantiate(_pfItem, transform);
            _lastItem.GetComponentInChildren<LayoutElement>().GetComponent<Image>().sprite = _cancelSprite;
            _lastItem.GetComponentInChildren<LayoutElement>().GetComponent<AspectRatioFitter>().aspectRatio = 1;
            _lastItem.GetComponentInChildren<TextMeshProUGUI>().SetText(_cancelText);
            _lastItem.GetComponent<Button>().onClick.AddListener(() => { _buildingPlacer.CurrentBuilding = null; });
        }
    }
}
