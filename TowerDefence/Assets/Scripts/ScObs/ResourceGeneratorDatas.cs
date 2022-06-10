using nsResourceGeneratorData;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceGeneratorDatas
{
    [CreateAssetMenu(menuName = "ScObs/ResourceGeneratorDatas")]
    public class ResourceGeneratorDatas : ScriptableObject
    {
        [SerializeField] private List<ResourceGeneratorData> _list;

        public List<ResourceGeneratorData> List => _list;
    }
}
