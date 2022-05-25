using nsAntiBuildingColliderPainter;
using nsBuildingPlacer;
using nsBuildingType;
using nsBuildingTypes;
using nsColorValue;
using nsMousePositionHelper;
using nsResourceGenerator;
using nsResourceStorage;
using nsSpriteParent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nsGhostBuilding
{
    public class GhostBuilding : MonoBehaviour
    {
        [SerializeField] private ResourceStorage _resourceStorage;
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private ColorValue _resourceNodeHighlightColor;
        [SerializeField] private ColorValue _overlappingColliderColor;
        [SerializeField] private ColorValue _enabledForBuildingColor;
        [SerializeField] private ColorValue _disabledForBuildingColor;
        [SerializeField] private float _tooltipDelay;

        private Dictionary<BuildingType, ResourceGenerator> _resourceGeneratorGhosts;
        private ResourceGenerator _currentResourceGeneratorGhost;
        private BuildingType _currentBuildingType;
        private MousePositionHelper _mousePositionHelper;
        private Vector3 _mousePosition;

        private HashSet<SpriteParent> _cachedColorables;
        private HashSet<Collider2D> _overlappingColliders;
        private HashSet<SpriteParent> _colorableColliders;
        private HashSet<SpriteParent> _resourceNodes;

        private bool _isSpaceEmpty;
        private bool _isAffordable;
        private bool _isWithinPylonRange;
        private bool _isCurrentlyBuildable;
        private bool _isPointerOverGameObject;
        private bool _isLMBDown;
        private string _text;

        public event Action<string, float> OnBuildOrderError;

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
            _currentBuildingType = buildingType;
            if (_currentResourceGeneratorGhost != null)
            {
                foreach (SpriteParent colorable in _cachedColorables)
                {
                    colorable.ApplyColor(null);
                }
                _cachedColorables.Clear();
                _currentResourceGeneratorGhost.gameObject.SetActive(false);
            }
            if (_currentBuildingType != null)
            {
                if (_mousePositionHelper != null) transform.position = _mousePositionHelper.MouseWorldPosition;
                _currentResourceGeneratorGhost = _resourceGeneratorGhosts[buildingType];
                _currentResourceGeneratorGhost.gameObject.SetActive(true);
                _buildingPlacer.SetBuildingCirclesActiveAll(true);
            }
            else
            {
                _buildingPlacer.SetBuildingCirclesActiveAll(false);
            }
        }

        private void Awake()
        {
            _resourceGeneratorGhosts = new Dictionary<BuildingType, ResourceGenerator>();
            _currentBuildingType = null;
            _mousePositionHelper = null;

            _cachedColorables = new HashSet<SpriteParent>();
            _overlappingColliders = new HashSet<Collider2D>();
            _colorableColliders = new HashSet<SpriteParent>();
            _resourceNodes = new HashSet<SpriteParent>();

            foreach (BuildingType buildingType in _buildingTypes.List)
            {
                ResourceGenerator resourceGeneratorGhost = Instantiate(buildingType.ResourceGenerator, transform, false);
                _resourceGeneratorGhosts[buildingType] = resourceGeneratorGhost;
                resourceGeneratorGhost.SetNodeDetectionCircleActive(true);
                resourceGeneratorGhost.SetAntiBuildingColliderActive(false);
                resourceGeneratorGhost.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            if (_currentBuildingType == null) return;

            _mousePosition = _mousePositionHelper.MouseWorldPosition;
            transform.position = _mousePosition;

            //Set colors to nodes and colliders
            _resourceNodes = _currentResourceGeneratorGhost.FindNearbyResourceNodes();
            _overlappingColliders = _currentResourceGeneratorGhost.GetOverlappingColliders();
            _isSpaceEmpty = _overlappingColliders.Count == 0;

            _colorableColliders.Clear();
            foreach (Collider2D collider in _overlappingColliders)
            {
                AntiBuildingColliderPainter antiBuildingColliderPainter = collider.GetComponent<AntiBuildingColliderPainter>();
                if (antiBuildingColliderPainter != null) _colorableColliders.Add(antiBuildingColliderPainter.SpriteParent);
            }

            _resourceNodes.ExceptWith(_colorableColliders);
            _cachedColorables.ExceptWith(_resourceNodes);
            _cachedColorables.ExceptWith(_colorableColliders);
            foreach (SpriteParent colorable in _resourceNodes)
            {
                colorable.ApplyColor(_resourceNodeHighlightColor);
            }
            foreach (SpriteParent colorable in _colorableColliders)
            {
                colorable.ApplyColor(_overlappingColliderColor);
            }
            foreach (SpriteParent colorable in _cachedColorables)
            {
                colorable.ApplyColor(null);
            }
            _cachedColorables = _resourceNodes;
            _cachedColorables.UnionWith(_colorableColliders);

            //Set color to the building prefab
            _isAffordable = _resourceStorage.IsAffordable(_currentBuildingType);
            _isWithinPylonRange = _currentResourceGeneratorGhost.IsWithinPylonRange();
            _isCurrentlyBuildable = _isSpaceEmpty && _isAffordable && _isWithinPylonRange;
            _currentResourceGeneratorGhost.ApplyColor(_isCurrentlyBuildable ? _enabledForBuildingColor : _disabledForBuildingColor);

            //Build order
            _isLMBDown = Input.GetMouseButtonDown(0);
            _isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
            if (_isLMBDown && !_isPointerOverGameObject)
            {
                if (_isCurrentlyBuildable)
                {
                    _resourceStorage.TakeResources(_currentBuildingType);
                    _buildingPlacer.PlaceBuilding(_mousePosition);
                }
                else
                {
                    _text = "";
                    if (!_isSpaceEmpty)
                    {
                        if (_text != "") _text += "<br>";
                        _text += "- Something is in the way, man!";
                    }
                    if (!_isAffordable)
                    {
                        if (_text != "") _text += "<br>";
                        _text += "- Can't afford! Get rich, yo!";
                    }
                    if (!_isWithinPylonRange)
                    {
                        if (_text != "") _text += "<br>";
                        _text += "- Too far from the other buildings, lonely and sad!";
                    }
                    OnBuildOrderError?.Invoke(_text, _tooltipDelay);
                }
            }
        }
    }
}
