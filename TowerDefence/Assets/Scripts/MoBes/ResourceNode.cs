using nsResourceType;
using nsColorable;
using UnityEngine;

namespace nsResourceNode
{
    public class ResourceNode : Colorable
    {
        [SerializeField] private ResourceType _resourceType;

        public ResourceType ResourceType => _resourceType;
    }
}
