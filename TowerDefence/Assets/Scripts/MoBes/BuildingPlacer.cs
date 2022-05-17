using nsBuildingType;
using nsBuildingTypes;
using nsMousePositionHelper;
using nsResourceGenerator;
using nsResourceStorage;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace nsBuildingPlacer
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private ResourceStorage _resourceStorage;

        private MousePositionHelper _mousePositionHelper;
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

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
            CurrentBuildingType = null;
        }

        private void Update()
        {
            bool isLMBDown = Input.GetMouseButtonDown(0);
            bool isRMBDown = Input.GetMouseButtonDown(1);
            bool isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();

            if (isLMBDown && !isPointerOverGameObject && (CurrentBuildingType != null))
            {
                Transform newBuilding = Instantiate(CurrentBuildingType.Prefab, _mousePositionHelper.MouseWorldPosition, Quaternion.identity);
                newBuilding.parent = transform;
                ResourceGenerator resourceGenerator = newBuilding.GetComponent<ResourceGenerator>();
                if (resourceGenerator != null)
                {
                    resourceGenerator.Initialize(_resourceStorage);
                }
                else
                {
                    Debug.Log("Error initializing a building prefab!");
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CurrentBuildingType = _buildingTypes.List[0];
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CurrentBuildingType = _buildingTypes.List[1];
            }

            if (Input.GetKeyDown(KeyCode.Escape) || isRMBDown)
            {
                CurrentBuildingType = null;
            }
        }
    }
}
