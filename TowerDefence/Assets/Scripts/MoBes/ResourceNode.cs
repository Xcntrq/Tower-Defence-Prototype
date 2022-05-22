using nsResourceType;
using nsSpriteParent;
using UnityEngine;

namespace nsResourceNode
{
    public class ResourceNode : SpriteParent
    {
        [SerializeField] private ResourceType _resourceType;

        public ResourceType ResourceType => _resourceType;
    }
}
