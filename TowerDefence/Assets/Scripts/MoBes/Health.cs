using nsResourceGenerator;
using System;
using UnityEngine;

namespace nsHealth
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private int _value;
        private int _maxValue;

        public float ValueNormalized => (float)_value / _maxValue;

        public event Action<float> OnValueChange;

        private void Awake()
        {
            _resourceGenerator.OnSetHealthActive += (bool value) => { enabled = value; };
        }

        private void Start()
        {
            _maxValue = _resourceGenerator.BuildingType.MaxHealth;
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
