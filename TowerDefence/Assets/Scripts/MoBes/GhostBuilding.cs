using nsBuildingPlacer;
using nsBuildings;
using nsColorValue;
using nsIColorable;
using nsMousePositionHelper;
using nsResourceGenerator;
using nsResourceStorage;
using nsColorable;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using nsBuilding;

namespace nsGhostBuilding
{
    public class GhostBuilding : MonoBehaviour
    {
        [SerializeField] private Buildings _buildings;
        [SerializeField] private ResourceStorage _resourceStorage;
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private ColorValue _resourceNodeHighlightColor;
        [SerializeField] private ColorValue _overlappingColliderColor;
        [SerializeField] private ColorValue _enabledForBuildingColor;
        [SerializeField] private ColorValue _disabledForBuildingColor;
        [SerializeField] private float _tooltipDelay;

        private MousePositionHelper _mousePositionHelper;
        private Vector3 _mousePosition;

        private Dictionary<Building, Building> _buildingGhosts;
        private Building _currentBuildingGhost;
        private Building _currentBuilding;
        private ResourceGenerator _resourceGenerator;

        private HashSet<Collider2D> _overlappingColliders;
        private HashSet<Colorable> _colorableColliders;
        private HashSet<Colorable> _cachedColorables;
        private HashSet<Colorable> _resourceNodes;

        private bool _isSpaceEmpty;
        private bool _isAffordable;
        private bool _isWithinPylonRange;
        private bool _isCurrentlyBuildable;
        private bool _isPointerOverGameObject;
        private bool _isLMBDown;
        private string _text;
        private bool _wasOutOfBounds;
        private bool _isOutOfBounds;

        public event Action<string, float> OnBuildOrderError;

        private void BuildingPlacer_OnCurrentBuildingChange(Building building)
        {
            _currentBuilding = building;
            if (_currentBuildingGhost != null)
            {
                foreach (Colorable colorable in _cachedColorables)
                {
                    colorable.ChangeColor(null);
                }
                _cachedColorables.Clear();
                _currentBuildingGhost.gameObject.SetActive(false);
            }
            if (building != null)
            {
                if (_mousePositionHelper != null) transform.position = _mousePositionHelper.MouseWorldPosition;
                _currentBuildingGhost = _buildingGhosts[building];
                _currentBuildingGhost.gameObject.SetActive(true);
                _buildingPlacer.SetBuildingCirclesActiveAll(true);
                gameObject.SetActive(true);
            }
            else
            {
                _currentBuildingGhost = null;
                _buildingPlacer.SetBuildingCirclesActiveAll(false);
                gameObject.SetActive(false);
            }
        }

        private void BuildingPlacer_OnGameOver()
        {
            BuildingPlacer_OnCurrentBuildingChange(null);
            Destroy(gameObject);
        }

        private void Awake()
        {
            _buildingGhosts = new Dictionary<Building, Building>();
            _mousePositionHelper = null;
            _currentBuildingGhost = null;
            _currentBuilding = null;

            _overlappingColliders = new HashSet<Collider2D>();
            _colorableColliders = new HashSet<Colorable>();
            _cachedColorables = new HashSet<Colorable>();
            _resourceNodes = new HashSet<Colorable>();

            foreach (Building building in _buildings.List)
            {
                Building newBuilding = Instantiate(building, transform, false);
                _buildingGhosts[building] = newBuilding;
                newBuilding.BecomeGhost();
                newBuilding.gameObject.SetActive(false);
            }

            _buildingPlacer.OnCurrentBuildingChange += BuildingPlacer_OnCurrentBuildingChange;
            _buildingPlacer.OnGameOver += BuildingPlacer_OnGameOver;
        }

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            _isOutOfBounds = (Input.mousePosition.x < 0) || (Input.mousePosition.y < 0) || (Input.mousePosition.x >= Screen.width) || (Input.mousePosition.y >= Screen.height);
            if (_isOutOfBounds || (_currentBuilding == null) || _wasOutOfBounds)
            {
                _wasOutOfBounds = _isOutOfBounds;
                return;
            }

            _mousePosition = _mousePositionHelper.MouseWorldPosition;
            transform.position = _mousePosition;

            //Set colors to nodes and colliders
            _resourceGenerator = _currentBuildingGhost as ResourceGenerator;
            _resourceNodes = _resourceGenerator == null ? new HashSet<Colorable>() : _resourceGenerator.FindNearbyResourceNodes();
            _overlappingColliders = _currentBuildingGhost.GetOverlappingColliders();
            _isSpaceEmpty = _overlappingColliders.Count == 0;

            _colorableColliders.Clear();
            foreach (Collider2D collider in _overlappingColliders)
            {
                IColorableCarrier colorableCarrier = collider.GetComponent<IColorableCarrier>();
                if (colorableCarrier != null) _colorableColliders.Add(colorableCarrier.Colorable);
            }

            _resourceNodes.ExceptWith(_colorableColliders);
            _cachedColorables.ExceptWith(_resourceNodes);
            _cachedColorables.ExceptWith(_colorableColliders);
            foreach (Colorable colorable in _resourceNodes)
            {
                colorable.ChangeColor(_resourceNodeHighlightColor);
            }
            foreach (Colorable colorable in _colorableColliders)
            {
                colorable.ChangeColor(_overlappingColliderColor);
            }
            foreach (Colorable colorable in _cachedColorables)
            {
                if (colorable != null) colorable.ChangeColor(null);
            }
            _cachedColorables = _resourceNodes;
            _cachedColorables.UnionWith(_colorableColliders);

            //Set color to the building prefab
            _isAffordable = _resourceStorage.IsAffordable(_currentBuilding);
            _isWithinPylonRange = _currentBuildingGhost.IsWithinPylonRange();
            _isCurrentlyBuildable = _isSpaceEmpty && _isAffordable && _isWithinPylonRange;
            _currentBuildingGhost.ChangeColor(_isCurrentlyBuildable ? _enabledForBuildingColor : _disabledForBuildingColor);

            //Build order
            _isLMBDown = Input.GetMouseButtonDown(0);
            _isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
            if (_isLMBDown && !_isPointerOverGameObject)
            {
                //_isSpaceEmpty = _currentResourceGeneratorGhost.GetOverlappingColliders().Count == 0;
                //_isCurrentlyBuildable = _isSpaceEmpty && _isAffordable && _isWithinPylonRange;
                if (_isCurrentlyBuildable)
                {
                    _resourceStorage.TakeResources(_currentBuilding);
                    _buildingPlacer.PlaceBuilding(_mousePosition);
                }
                else
                {
                    _text = "";
                    if (!_isSpaceEmpty)
                    {
                        if (_text != "") _text += "<br>";
                        _text += "- Something's in the way, man!";
                    }
                    if (!_isAffordable)
                    {
                        if (_text != "") _text += "<br>";
                        _text += "- Can't afford! Get rich, yo!";
                    }
                    if (!_isWithinPylonRange)
                    {
                        if (_text != "") _text += "<br>";
                        _text += "- Too far away from the other buildings, lonely and sad!";
                    }
                    OnBuildOrderError?.Invoke(_text, _tooltipDelay);
                }
            }
        }

        private void OnDestroy()
        {
            _buildingPlacer.OnCurrentBuildingChange -= BuildingPlacer_OnCurrentBuildingChange;
            _buildingPlacer.OnGameOver -= BuildingPlacer_OnGameOver;
        }
    }
}
