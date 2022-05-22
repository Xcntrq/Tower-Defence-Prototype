using nsBuildingDistance;
using nsResourceGenerator;
using UnityEngine;

namespace nsAntiBuildingCircle
{
    public class AntiBuildingCircle : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            _resourceGenerator.OnSetBuildingCirclesActive += ResourceGenerator_OnSetBuildingCirclesActive;
            _resourceGenerator.OnSetBuildingCirclesDistance += ResourceGenerator_OnSetBuildingCirclesDistance;
        }

        private void OnDisable()
        {
            _resourceGenerator.OnSetBuildingCirclesActive -= ResourceGenerator_OnSetBuildingCirclesActive;
            _resourceGenerator.OnSetBuildingCirclesDistance -= ResourceGenerator_OnSetBuildingCirclesDistance;
        }

        public void ResourceGenerator_OnSetBuildingCirclesActive(bool value)
        {
            _spriteRenderer.enabled = value;
        }

        public void ResourceGenerator_OnSetBuildingCirclesDistance(BuildingDistance buildingDistance)
        {
            transform.localScale = new Vector3(buildingDistance.Min, buildingDistance.Min, 1);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
