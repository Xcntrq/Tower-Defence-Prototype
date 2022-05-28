using nsBuilding;
using nsBuildingDistance;
using UnityEngine;

namespace nsPylonCircle
{
    public class PylonCircle : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private bool _activeOnAwake;

        private void Awake()
        {
            _building.OnBuildingCirclesToggle += Building_OnBuildingCirclesToggle;
            _building.OnBuildingCirclesDistanceChange += Building_OnBuildingCirclesDistanceChange;
            gameObject.SetActive(_activeOnAwake);
        }

        private void Building_OnBuildingCirclesToggle(bool value)
        {
            gameObject.SetActive(value);
        }

        private void Building_OnBuildingCirclesDistanceChange(BuildingDistance buildingDistance)
        {
            transform.localScale = new Vector3(buildingDistance.Max, buildingDistance.Max, 1);
        }
    }
}
