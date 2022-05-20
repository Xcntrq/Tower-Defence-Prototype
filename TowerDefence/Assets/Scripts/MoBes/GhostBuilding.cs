using nsBuildingPlacer;
using nsBuildingType;
using nsBuildingTypes;
using nsMousePositionHelper;
using nsNearbyResourceNodeFinder;
using nsResourceGenerator;
using System.Collections.Generic;
using UnityEngine;

namespace nsGhostBuilding
{
    public class GhostBuilding : MonoBehaviour
    {
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private BuildingPlacer _buildingPlacer;

        private Dictionary<BuildingType, ResourceGenerator> _resourceGeneratorGhosts;

        private NearbyResourceNodeFinder _nearbyResourceNodeFinder;
        private MousePositionHelper _mousePositionHelper;

        private BuildingType _currentBuildingType;
        private ResourceGenerator _currentResourceGeneratorGhost;

        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;

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
            if (_currentResourceGeneratorGhost != null) _currentResourceGeneratorGhost.gameObject.SetActive(false);
            if (_currentBuildingType != null)
            {
                if (_mousePositionHelper != null) transform.position = _mousePositionHelper.MouseWorldPosition;
                _currentResourceGeneratorGhost = _resourceGeneratorGhosts[buildingType];
                _currentResourceGeneratorGhost.gameObject.SetActive(true);
            }
        }

        private void Awake()
        {
            _resourceGeneratorGhosts = new Dictionary<BuildingType, ResourceGenerator>();
            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _mousePositionHelper = null;
            _currentBuildingType = null;

            foreach (BuildingType buildingType in _buildingTypes.List)
            {
                ResourceGenerator resourceGeneratorGhost = Instantiate(buildingType.ResourceGenerator, transform, false);
                _resourceGeneratorGhosts[buildingType] = resourceGeneratorGhost;
                resourceGeneratorGhost.SetTransparent(true);
                resourceGeneratorGhost.SetNodeDetectionCircleActive(true);
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

            transform.position = _mousePositionHelper.MouseWorldPosition;
            _nearbyResourceNodeCount = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, _currentBuildingType.ResourceGeneratorData.NodeDetectionRadius, _currentBuildingType.ResourceGeneratorData.ResourceType);
            _totalAmountPerCycle = _nearbyResourceNodeCount * _currentBuildingType.ResourceGeneratorData.AmountPerCycle;
            _currentResourceGeneratorGhost.OnOverlapCircleAll?.Invoke(_nearbyResourceNodeCount, _totalAmountPerCycle);
        }
    }
}
