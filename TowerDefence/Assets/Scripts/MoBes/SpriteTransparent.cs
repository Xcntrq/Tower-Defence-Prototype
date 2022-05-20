using nsColorValue;
using nsResourceGenerator;
using UnityEngine;

namespace nsSpriteTransparent
{
    public class SpriteTransparent : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;
        [SerializeField] private ColorValue _transparentColor;
        [SerializeField] private ColorValue _opaqueColor;

        private SpriteRenderer _spriteRenderer;

        private void OnEnable()
        {
            _resourceGenerator.OnSetTransparent += ResourceGenerator_OnSetTransparent;
        }

        private void OnDisable()
        {
            _resourceGenerator.OnSetTransparent -= ResourceGenerator_OnSetTransparent;
        }

        public void ResourceGenerator_OnSetTransparent(bool value)
        {
            _spriteRenderer.color = value ? _transparentColor.Value : _opaqueColor.Value;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
