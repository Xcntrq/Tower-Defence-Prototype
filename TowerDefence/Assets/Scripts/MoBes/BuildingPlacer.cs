using nsBuilding;
using nsBuildings;
using nsResourceGenerator;
using nsResourceStorage;
using nsTower;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsBuildingPlacer
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private Buildings _buildings;
        [SerializeField] private ResourceStorage _resourceStorage;

        private HashSet<Building> _placedBuildings;
        private Building _currentBuilding;
        private bool _areBuildingCirclesActive;

        public HashSet<Building> PlacedBuildings
        {
            get
            {
                if (_placedBuildings == null) _placedBuildings = new HashSet<Building>();
                return _placedBuildings;
            }
        }

        public Building CurrentBuilding
        {
            get
            {
                return _currentBuilding;
            }
            set
            {
                _currentBuilding = _currentBuilding == value ? null : value;
                OnCurrentBuildingChange?.Invoke(_currentBuilding);
            }
        }

        public event Action<Building> OnCurrentBuildingChange;

        private void Awake()
        {
            _areBuildingCirclesActive = false;
        }

        private void Start()
        {
            CurrentBuilding = _buildings.List[0];
            PlaceBuilding(Vector3.zero);
            CurrentBuilding = null;
        }

        private void Update()
        {
            foreach (Building building in _buildings.List)
            {
                bool isKeyPressed = Input.GetKeyDown(building.BuildingType.KeyCode);
                if (isKeyPressed) CurrentBuilding = building;
            }

            bool isRMBDown = Input.GetMouseButtonDown(1);
            if (Input.GetKeyDown(KeyCode.Escape) || isRMBDown)
            {
                CurrentBuilding = null;
            }
        }

        public void PlaceBuilding(Vector3 position)
        {
            if (CurrentBuilding == null) return;
            Building building = Instantiate(CurrentBuilding, position, Quaternion.identity, transform);
            if (building is ResourceGenerator) (building as ResourceGenerator).Initialize(this, _resourceStorage);
            if (building is Tower) building.Initialize(this);
            PlacedBuildings.Add(building);
            SetBuildingCirclesActiveAll(_areBuildingCirclesActive);
        }

        public void ForgetBuilding(Building building)
        {
            PlacedBuildings.Remove(building);
        }

        public void SetBuildingCirclesActiveAll(bool value)
        {
            _areBuildingCirclesActive = value;
            foreach (Building building in PlacedBuildings)
            {
                building.SetBuildingCirclesActive(value);
            }
        }
    }
}
