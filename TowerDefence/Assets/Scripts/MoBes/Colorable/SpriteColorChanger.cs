using nsColorValue;
using nsColorable;
using UnityEngine;

namespace nsSpriteColorChanger
{
    public class SpriteColorChanger : MonoBehaviour
    {
        [SerializeField] private Colorable _colorable;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private bool _isColorCached;

        private void OnEnable()
        {
            _colorable.OnColorChange += Colorable_OnApplyColor;
        }

        private void OnDisable()
        {
            _colorable.OnColorChange -= Colorable_OnApplyColor;
        }

        private void Colorable_OnApplyColor(ColorValue colorValue)
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
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = Color.white;
            _isColorCached = false;
        }

        private void Start()
        {
            //Can't cache the color on Awake because it may be randomized in a different component's Awake
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
