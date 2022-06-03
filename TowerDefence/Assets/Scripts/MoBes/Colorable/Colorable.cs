using nsColorValue;
using System;
using UnityEngine;

namespace nsColorable
{
    public abstract class Colorable : MonoBehaviour
    {
        public event Action<ColorValue> OnColorChange;
        public event Action<bool> OnSetActive;

        public void ChangeColor(ColorValue colorValue)
        {
            OnColorChange?.Invoke(colorValue);
        }

        protected void SetActive(bool value)
        {
            OnSetActive?.Invoke(value);
        }
    }
}
