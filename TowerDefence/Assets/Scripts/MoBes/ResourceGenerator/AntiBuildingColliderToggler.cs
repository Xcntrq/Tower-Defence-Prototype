using nsResourceGenerator;
using UnityEngine;

namespace nsAntiBuildingColliderToggler
{
    public class AntiBuildingColliderToggler : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private CircleCollider2D _circleCollider2D;

        private void OnEnable()
        {
            _resourceGenerator.OnSetAntiBuildingColliderActive += ResourceGenerator_OnSetAntiBuildingColliderActive;
        }

        private void OnDisable()
        {
            _resourceGenerator.OnSetAntiBuildingColliderActive -= ResourceGenerator_OnSetAntiBuildingColliderActive;
        }

        public void ResourceGenerator_OnSetAntiBuildingColliderActive(bool value)
        {
            _circleCollider2D.enabled = value;
        }

        private void Awake()
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
        }
    }
}
