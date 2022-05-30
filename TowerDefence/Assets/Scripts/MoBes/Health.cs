using nsIHealthCarrier;
using System;
using UnityEngine;

namespace nsHealth
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _iHealthCarrier;

        private int _value;
        private int _maxValue;
        private IHealthCarrier _healthCarrier;

        public float ValueNormalized => (float)_value / _maxValue;

        public event Action<float> OnValueChange;

        private void Awake()
        {
            _healthCarrier = _iHealthCarrier as IHealthCarrier;
        }

        private void Start()
        {
            _maxValue = _healthCarrier.MaxHealth;
            _value = _maxValue;
            OnValueChange?.Invoke(ValueNormalized);
        }

        public void Decrease(int value)
        {
            _value = Mathf.Clamp(_value - value, 0, _value);
            OnValueChange?.Invoke(ValueNormalized);
            if (_value == 0) Destroy(gameObject);
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.T)) Decrease(10);
        //}
    }
}
