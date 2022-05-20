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
        }

        private void OnDisable()
        {
            _resourceGenerator.OnSetNodeDetectionCircleActive -= ResourceGenerator_OnSetNodeDetectionCircleActive;
        }

        private void ResourceGenerator_OnSetNodeDetectionCircleActive(bool value, float radius)
        {
            _spriteRenderer.enabled = value;
            transform.localScale = new Vector3(radius, radius, 1);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
