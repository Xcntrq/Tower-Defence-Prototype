using nsMousePositionHelper;
using UnityEngine;

namespace nsBuildingPlacer
{
    public class BuildingPlacer : MonoBehaviour
    {
        [SerializeField] private Transform _pfWoodHarvester;

        private MousePositionHelper _mousePositionHelper;

        private void Start()
        {
            _mousePositionHelper = new MousePositionHelper(Camera.main);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(_pfWoodHarvester, _mousePositionHelper.MouseWorldPosition, Quaternion.identity);
            }
        }
    }
}
