using nsResourceNode;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceNodeList
{
    [CreateAssetMenu(menuName = "ScObs/ResourceNodeList")]
    public class ResourceNodeList : ScriptableObject
    {
        [SerializeField] private List<ResourceNode> _items;

        public List<ResourceNode> Items => _items;
    }
}
