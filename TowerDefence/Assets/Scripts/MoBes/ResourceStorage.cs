using nsResourceType;
using nsResourceTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceStorage
{
    public class ResourceStorage : MonoBehaviour
    {
        [SerializeField] private ResourceTypes _resourceTypes;

        private Dictionary<ResourceType, int> _resourceAmounts;

        public Action<ResourceType, int> OnAmountChange;

        private void Awake()
        {
            _resourceAmounts = new Dictionary<ResourceType, int>();
        }

        private void Start()
        {
            foreach (ResourceType resourceType in _resourceTypes.List)
            {
                _resourceAmounts[resourceType] = 0;

                //The subscribers need to have passed their OnEnable callbacks for this line
                //Which is why this is in Start and not in Awake
                OnAmountChange?.Invoke(resourceType, _resourceAmounts[resourceType]);
            }
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            _resourceAmounts[resourceType] += amount;
            OnAmountChange?.Invoke(resourceType, _resourceAmounts[resourceType]);
        }

        //public int GetResourceAmount(ResourceType resourceType)
        //{
        //    return _resourceAmounts[resourceType];
        //}
    }
}
