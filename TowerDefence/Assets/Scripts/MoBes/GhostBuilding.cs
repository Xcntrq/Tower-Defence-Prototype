using nsBuildingPlacer;
using nsBuildingType;
using nsBuildingTypes;
using nsColorValue;
using nsMousePositionHelper;
using nsResourceGenerator;
using nsResourceNode;
using System.Collections.Generic;
using UnityEngine;

namespace nsGhostBuilding
{
    public class GhostBuilding : MonoBehaviour
    {
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private ColorValue _resourceNodeHighlightColor;

        private Dictionary<BuildingType, ResourceGenerator> _resourceGeneratorGhosts;
        private ResourceGenerator _currentResourceGeneratorGhost;
        private BuildingType _currentBuildingType;
        private MousePositionHelper _mousePositionHelper;
        private HashSet<ResourceNode> _currentlyHighlightedResourceNodes;

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
                foreach (ResourceNode resourceNode in _currentlyHighlightedResourceNodes)
                {
                    resourceNode.ChangeColor(null);
                }
                _currentResourceGeneratorGhost.gameObject.SetActive(false);
            }
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
            _currentlyHighlightedResourceNodes = new HashSet<ResourceNode>();
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

            HashSet<ResourceNode> desiredNearbyResourceNodes = _currentResourceGeneratorGhost.FindNearbyResourceNodes();
            foreach (ResourceNode resourceNode in desiredNearbyResourceNodes)
            {
                resourceNode.ChangeColor(_resourceNodeHighlightColor);
            }

            _currentlyHighlightedResourceNodes.ExceptWith(desiredNearbyResourceNodes);
            foreach (ResourceNode resourceNode in _currentlyHighlightedResourceNodes)
            {
                resourceNode.ChangeColor(null);
            }

            _currentlyHighlightedResourceNodes = desiredNearbyResourceNodes;
        }
    }
}
