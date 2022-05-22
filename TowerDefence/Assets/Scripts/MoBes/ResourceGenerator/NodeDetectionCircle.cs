using nsResourceGenerator;
using UnityEngine;

namespace nsNodeDetectionCircle
{
    public class NodeDetectionCircle : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            _resourceGenerator.OnSetNodeDetectionCircleActive += ResourceGenerator_OnSetNodeDetectionCircleActive;
            _resourceGenerator.OnSetNodeDetectionCircleDistance += ResourceGenerator_OnSetNodeDetectionCircleDistance;
        }

        private void OnDisable()
        {
            _resourceGenerator.OnSetNodeDetectionCircleActive -= ResourceGenerator_OnSetNodeDetectionCircleActive;
            _resourceGenerator.OnSetNodeDetectionCircleDistance -= ResourceGenerator_OnSetNodeDetectionCircleDistance;
        }

        private void ResourceGenerator_OnSetNodeDetectionCircleActive(bool value)
        {
            _spriteRenderer.enabled = value;
        }

        private void ResourceGenerator_OnSetNodeDetectionCircleDistance(float radius)
        {
            transform.localScale = new Vector3(radius, radius, 1);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
