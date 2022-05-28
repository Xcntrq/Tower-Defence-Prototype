using nsBuilding;
using UnityEngine;

namespace nsActionRangeCircle
{
    public class ActionRangeCircle : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private bool _activeOnAwake;

        private void Awake()
        {
            _building.OnActionRangeCircleToggle += Building_OnActionRangeCircleToggle;
            _building.OnActionRadiusChange += Building_OnActionRadiusChange;
            gameObject.SetActive(_activeOnAwake);
        }

        private void Building_OnActionRangeCircleToggle(bool value)
        {
            gameObject.SetActive(value);
        }

        private void Building_OnActionRadiusChange(float radius)
        {
            transform.localScale = new Vector3(radius, radius, 1);
        }
    }
}
