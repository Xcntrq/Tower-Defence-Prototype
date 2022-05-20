using nsColorValue;
using nsResourceNode;
using UnityEngine;

namespace nsResourceNodeColorChanger
{
    public class ResourceNodeColorChanger : MonoBehaviour
    {
        [SerializeField] private ResourceNode _resourceNode;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private bool _isColorCached;

        private void OnEnable()
        {
            _resourceNode.OnColorChange += ResourceNode_OnColorChange;
        }

        private void OnDisable()
        {
            _resourceNode.OnColorChange -= ResourceNode_OnColorChange;
        }

        private void ResourceNode_OnColorChange(ColorValue colorValue)
        {
            if (colorValue != null)
            {
                if (_isColorCached == false)
                {
                    _defaultColor = _spriteRenderer.color;
                    _isColorCached = true;
                }
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
    }
}
