using nsColorValue;
using nsGhostBuilding;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nsTooltipDisplay
{
    public class TooltipDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private GhostBuilding _ghostBuilding;
        [SerializeField] private ColorValue _transparentColor;

        private RectTransform _rectTransform;
        private Color _defaultColor;
        private Image _image;
        private bool _isActive;

        private void OnEnable()
        {
            _ghostBuilding.OnBuildOrderError += GhostBuilding_OnBuildOrderError;
        }

        private void OnDisable()
        {
            _ghostBuilding.OnBuildOrderError -= GhostBuilding_OnBuildOrderError;
        }

        private void GhostBuilding_OnBuildOrderError(string text, float tooltipDelay)
        {
            StopAllCoroutines();
            UpdatePosition();
            _isActive = true;
            _image.color = _defaultColor;
            StartCoroutine(HideAfterDelay(tooltipDelay));
        }

        private void Awake()
        {
            _isActive = false;
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _defaultColor = _image.color;
            _image.color = _transparentColor.Value;
        }

        private void Update()
        {
            if (_isActive) UpdatePosition();
        }

        private IEnumerator HideAfterDelay(float tooltipDelay)
        {
            yield return new WaitForSeconds(tooltipDelay);
            _image.color = _transparentColor.Value;
            _isActive = false;
        }

        private void UpdatePosition()
        {
            Vector2 anchoredPosition = Input.mousePosition / _canvasRectTransform.localScale.x;
            if (anchoredPosition.x < 0) anchoredPosition.x = anchoredPosition.x = 0;
            if (anchoredPosition.y < 0) anchoredPosition.y = anchoredPosition.y = 0;
            if (anchoredPosition.x + _rectTransform.rect.width > _canvasRectTransform.rect.width) anchoredPosition.x = _canvasRectTransform.rect.width - _rectTransform.rect.width;
            if (anchoredPosition.y + _rectTransform.rect.height > _canvasRectTransform.rect.height) anchoredPosition.y = _canvasRectTransform.rect.height - _rectTransform.rect.height;
            _rectTransform.anchoredPosition = anchoredPosition;
        }
    }
}
