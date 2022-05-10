using nsBuildingType;
using nsBuildingTypes;
using nsMousePositionHelper;
using UnityEngine;

namespace nsBuildingPlacer
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private BuildingTypes _buildingTypes;

        private BuildingType _currentBuildingType;

        private MousePositionHelper _mousePositionHelper;

        private void Start()
        {
            _currentBuildingType = _buildingTypes.List[0];
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Transform newBuilding = Instantiate(_currentBuildingType.Prefab, _mousePositionHelper.MouseWorldPosition, Quaternion.identity);
                newBuilding.parent = transform;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentBuildingType = _buildingTypes.List[0];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentBuildingType = _buildingTypes.List[1];
            }
        }
    }
}
