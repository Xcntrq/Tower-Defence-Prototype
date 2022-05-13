using nsBuildingType;
using nsBuildingTypes;
using nsMousePositionHelper;
using nsResourceGenerator;
using nsResourceStorage;
using UnityEngine;

namespace nsBuildingPlacer
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private BuildingTypes _buildingTypes;
        [SerializeField] private ResourceStorage _resourceStorage;

        private MousePositionHelper _mousePositionHelper;
        private BuildingType _currentBuildingType;

        private void Awake()
        {
            _currentBuildingType = _buildingTypes.List[0];
        }

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Transform newBuilding = Instantiate(_currentBuildingType.Prefab, _mousePositionHelper.MouseWorldPosition, Quaternion.identity);
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
                _currentBuildingType = _buildingTypes.List[0];
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentBuildingType = _buildingTypes.List[1];
            }
        }
    }
}
