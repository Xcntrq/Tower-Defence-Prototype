using nsColorValue;
using nsResourceType;
using System;
using UnityEngine;

namespace nsResourceNode
{
    public class ResourceNode : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;

        public ResourceType ResourceType => _resourceType;

        public Action<ColorValue> OnColorChange;

        public void ChangeColor(ColorValue colorValue)
        {
            OnColorChange?.Invoke(colorValue);
        }
    }
}
