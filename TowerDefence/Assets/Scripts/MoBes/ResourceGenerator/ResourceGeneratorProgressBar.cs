using nsResourceGenerator;
using UnityEngine;

namespace nsResourceGeneratorProgressBar
{
    public class ResourceGeneratorProgressBar : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private RectTransform _rectTransform;
        private Canvas _canvas;
        private bool _isActive;

        private void Awake()
        {
            _isActive = false;
            _rectTransform = GetComponent<RectTransform>();
            _resourceGenerator.OnProgressChange += ResourceGenerator_OnProgressChange;
            _canvas = GetComponentInParent<Canvas>();
            _canvas.gameObject.SetActive(false);
        }

        private void ResourceGenerator_OnProgressChange(float scale)
        {
            if (!_isActive)
            {
                _canvas.gameObject.SetActive(true);
                _isActive = true;
            }
            Vector3 newLocalScale = _rectTransform.localScale;
            newLocalScale.x = scale;
            _rectTransform.localScale = newLocalScale;
        }
    }
}
