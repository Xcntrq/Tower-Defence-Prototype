using nsResourceType;
using System;
using UnityEngine;

namespace nsResourceGeneratorData
{
    [Serializable]
    public class ResourceGeneratorData
    {
        [SerializeField] private int _amountPerCycle;
        [SerializeField] private float _timeCost;
        [SerializeField] private float _nodeDetectionRadius;
        [SerializeField] private ResourceType _resourceType;

        public int AmountPerCycle => _amountPerCycle;
        public float TimeCost => _timeCost;
        public float NodeDetectionRadius => _nodeDetectionRadius;
        public ResourceType ResourceType => _resourceType;
    }
}
