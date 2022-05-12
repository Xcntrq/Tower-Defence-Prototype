using nsResourceType;
using nsResourceTypes;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceStorage
{
    public class ResourceStorage : MonoBehaviour
    {
        [SerializeField] private ResourceTypes _resourceTypes;

        private Dictionary<ResourceType, int> _resourceAmounts;

        private void Awake()
        {
            _resourceAmounts = new Dictionary<ResourceType, int>();

            foreach (ResourceType resourceType in _resourceTypes.List)
            {
                _resourceAmounts[resourceType] = 0;
            }
        }

        public void AddResource(ResourceType resourceType, int amount)
        {
            _resourceAmounts[resourceType] += amount;
            LogAmounts();
        }

        private void LogAmounts()
        {
            foreach (ResourceType resourceType in _resourceTypes.List)
            {
                string line = string.Concat(resourceType.name, ": ", _resourceAmounts[resourceType]);
                Debug.Log(line);
            }
        }
    }
}
