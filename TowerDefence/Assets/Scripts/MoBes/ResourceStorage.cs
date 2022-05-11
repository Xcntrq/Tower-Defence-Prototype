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
    }
}
