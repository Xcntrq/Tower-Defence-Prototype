using nsBuilding;
using nsBuildingDistance;
using UnityEngine;

namespace nsAntiBuildingColliderToggler
{
    public class AntiBuildingColliderToggler : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private bool _activeOnAwake;

        private void Awake()
        {
            _building.OnAntiBuildingColliderToggle += Building_OnAntiBuildingColliderToggle;
            _building.OnBuildingCirclesDistanceChange += Building_OnBuildingCirclesDistanceChange;
            gameObject.SetActive(_activeOnAwake);
        }

        private void Building_OnAntiBuildingColliderToggle(bool value)
        {
            gameObject.SetActive(value);
        }

        private void Building_OnBuildingCirclesDistanceChange(BuildingDistance buildingDistance)
        {
            transform.localScale = new Vector3(buildingDistance.Min, buildingDistance.Min, 1);
        }
    }
}
