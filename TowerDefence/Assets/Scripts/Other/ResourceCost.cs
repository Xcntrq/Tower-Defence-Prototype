using nsResourceType;
using System;
using UnityEngine;

namespace nsResourceCost
{
    [Serializable]
    public class ResourceCost
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _value;

        public ResourceType ResourceType => _resourceType;
        public int Value => _value;
    }
}
