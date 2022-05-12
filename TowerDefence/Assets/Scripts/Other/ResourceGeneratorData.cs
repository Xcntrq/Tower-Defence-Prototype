using nsResourceType;
using System;
using UnityEngine;

namespace nsResourceGeneratorData
{
    [Serializable]
    public class ResourceGeneratorData
    {
        [SerializeField] private float _timeCost;
        [SerializeField] private ResourceType _resourceType;

        public float TimeCost => _timeCost;
        public ResourceType ResourceType => _resourceType;
    }
}
