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

            if (isLMBDown && (CurrentBuildingType != null) && !isPointerOverGameObject)
            {
                ResourceGenerator resourceGenerator = Instantiate(CurrentBuildingType.ResourceGenerator, _mousePositionHelper.MouseWorldPosition, Quaternion.identity, transform);
                resourceGenerator.Initialize(_resourceStorage);
            }

            foreach (BuildingType buildingType in _buildingTypes.List)
            {
                bool isKeyPressed = Input.GetKeyDown(buildingType.KeyCode);
                if (isKeyPressed) CurrentBuildingType = buildingType;
            }

            if (Input.GetKeyDown(KeyCode.Escape) || isRMBDown)
            {
                CurrentBuildingType = null;
            }
        }
    }
}
