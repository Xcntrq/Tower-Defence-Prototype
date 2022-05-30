using nsColorValue;
using System;
using UnityEngine;

namespace nsColorable
{
    public abstract class Colorable : MonoBehaviour
    {
        public event Action<ColorValue> OnColorChange;

        public void ChangeColor(ColorValue colorValue)
        {
            OnColorChange?.Invoke(colorValue);
        }
    }
}
