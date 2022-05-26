using nsColorValue;
using nsHealth;
using UnityEngine;
using UnityEngine.UI;

namespace nsHealthBar
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _valueBar;
        [SerializeField] private ColorValue _highColor;
        [SerializeField] private ColorValue _mediumColor;
        [SerializeField] private ColorValue _lowColor;

        //private void OnEnable()
        //{
        //    _health.OnValueChange += Health_OnValueChange;
        //}
        //
        //private void OnDisable()
        //{
        //    _health.OnValueChange -= Health_OnValueChange;
        //}

        private void Health_OnValueChange(float valueNormalized)
        {
            switch (valueNormalized)
            {
                case > 0.99f:
                    gameObject.SetActive(false);
                    return;
                case > 0.66f:
                    _image.color = _highColor.Value;
                    break;
                case > 0.33f:
                    _image.color = _mediumColor.Value;
                    break;
                default:
                    _image.color = _lowColor.Value;
                    break;
            }
            Vector3 newLocalScale = _valueBar.localScale;
            newLocalScale.x = valueNormalized;
            _valueBar.localScale = newLocalScale;
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            _health.OnValueChange += Health_OnValueChange;
            gameObject.SetActive(false);
        }
    }
}
