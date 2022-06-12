using nsBuilding;
using nsResourceCost;
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

        public event Action<ResourceType, int> OnAmountChange;

        private void Awake()
        {
            _resourceAmounts = new Dictionary<ResourceType, int>();
            foreach (ResourceType resourceType in _resourceTypes.List)
            {
                _resourceAmounts[resourceType] = resourceType.AmountAtStart;
            }
        }

        private void Start()
        {
            //The subscribers need to have passed their OnEnable callbacks for this line
            //Which is why this is in Start and not in Awake
            foreach (ResourceType resourceType in _resourceTypes.List)
            {
                OnAmountChange?.Invoke(resourceType, _resourceAmounts[resourceType]);
            }
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            _resourceAmounts[resourceType] += amount;
            OnAmountChange?.Invoke(resourceType, _resourceAmounts[resourceType]);
        }

        public void TakeResources(Building building)
        {
            foreach (ResourceCost resourceCost in building.BuildingType.ResourceCosts)
            {
                _resourceAmounts[resourceCost.ResourceType] -= resourceCost.Value;
                OnAmountChange?.Invoke(resourceCost.ResourceType, _resourceAmounts[resourceCost.ResourceType]);
            }
        }

        public bool IsAffordable(Building building)
        {
            foreach (ResourceCost resourceCost in building.BuildingType.ResourceCosts)
            {
                if (_resourceAmounts[resourceCost.ResourceType] < resourceCost.Value) return false;
            }
            return true;
        }
    }
}
