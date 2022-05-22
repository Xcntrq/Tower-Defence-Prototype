using nsColorValue;
using nsSpriteParent;
using UnityEngine;

namespace nsSpriteColorChanger
{
    public class SpriteColorChanger : MonoBehaviour
    {
        [SerializeField] private SpriteParent _spriteParent;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private bool _isColorCached;

        private void OnEnable()
        {
            _spriteParent.OnApplyColor += SpriteParent_OnApplyColor;
        }

        private void OnDisable()
        {
            _spriteParent.OnApplyColor -= SpriteParent_OnApplyColor;
        }

        private void SpriteParent_OnApplyColor(ColorValue colorValue)
        {
            if (colorValue != null)
            {
                CacheColor();
                _spriteRenderer.color = colorValue.Value;
            }
            if ((colorValue == null) && _isColorCached)
            {
                _spriteRenderer.color = _defaultColor;
            }
        }

        private void Awake()
        {
            //Can't cache the color on Awake because it may be randomized in a different component's Awake
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = Color.white;
            _isColorCached = false;
        }

        private void Start()
        {
            CacheColor();
        }

        private void CacheColor()
        {
            if (_isColorCached) return;
            _defaultColor = _spriteRenderer.color;
            _isColorCached = true;
        }
    }
}
