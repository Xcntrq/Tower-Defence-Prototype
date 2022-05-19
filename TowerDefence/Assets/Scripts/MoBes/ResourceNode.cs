using nsResourceType;
using UnityEngine;

namespace nsResourceNode
{
    public class ResourceNode : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;

        public ResourceType ResourceType => _resourceType;
    }
}
