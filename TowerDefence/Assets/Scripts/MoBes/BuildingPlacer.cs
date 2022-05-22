using nsBuildingType;
using nsBuildingTypes;
using nsResourceGenerator;
using nsResourceStorage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsBuildingPlacer
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private ResourceStorage _resourceStorage;

        private List<ResourceGenerator> _placedResourceGenerators;
        private bool _areBuildingCirclesActive;
        private BuildingType _currentBuildingType;

        public BuildingType CurrentBuildingType
        {
            get
            {
                return _currentBuildingType;
            }
            set
            {
                _currentBuildingType = _currentBuildingType == value ? null : value;
                OnCurrentBuildingTypeChange?.Invoke(_currentBuildingType);
            }
        }

        public Action<BuildingType> OnCurrentBuildingTypeChange;

        private void Awake()
        {
            _placedResourceGenerators = new List<ResourceGenerator>();
            _areBuildingCirclesActive = false;
        }

        private void Start()
        {
            CurrentBuildingType = _buildingTypes.List[0];
            PlaceBuilding(Vector3.zero);
            CurrentBuildingType = null;
        }

        private void Update()
        {
            foreach (BuildingType buildingType in _buildingTypes.List)
            {
                bool isKeyPressed = Input.GetKeyDown(buildingType.KeyCode);
                if (isKeyPressed) CurrentBuildingType = buildingType;
            }

            bool isRMBDown = Input.GetMouseButtonDown(1);
            if (Input.GetKeyDown(KeyCode.Escape) || isRMBDown)
            {
                CurrentBuildingType = null;
            }
        }

        public void PlaceBuilding(Vector3 position)
        {
            ResourceGenerator resourceGenerator = Instantiate(CurrentBuildingType.ResourceGenerator, position, Quaternion.identity, transform);
            resourceGenerator.Initialize(_resourceStorage);
            _placedResourceGenerators.Add(resourceGenerator);
            SetBuildingCirclesActiveAll(_areBuildingCirclesActive);
        }

        public void SetBuildingCirclesActiveAll(bool value)
        {
            _areBuildingCirclesActive = value;
            foreach (ResourceGenerator resourceGenerator in _placedResourceGenerators)
            {
                resourceGenerator.SetBuildingCirclesActive(value);
            }
        }
    }
}
