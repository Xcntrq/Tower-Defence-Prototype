using nsColorValue;
using System;
using UnityEngine;

namespace nsSpriteParent
{
    public abstract class SpriteParent : MonoBehaviour
    {
        public Action<ColorValue> OnApplyColor;

        public void ApplyColor(ColorValue colorValue)
        {
            OnApplyColor?.Invoke(colorValue);
        }
    }
}
