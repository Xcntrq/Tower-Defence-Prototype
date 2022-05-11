using nsResourceType;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceTypes
{
    [CreateAssetMenu(menuName = "ScObs/ResourceTypes")]
    public class ResourceTypes : ScriptableObject
    {
        [SerializeField] private List<ResourceType> _list;

        public List<ResourceType> List => _list;
    }
}
