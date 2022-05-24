using nsResourceCost;
using nsResourceGenerator;
using nsResourceGeneratorData;
using System.Collections.Generic;
using UnityEngine;

namespace nsBuildingType
{
    [CreateAssetMenu(menuName = "ScObs/BuildingType")]
    public class BuildingType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private KeyCode _keyCode;
        [SerializeField] private ResourceGenerator _resourceGenerator;
        [SerializeField] private ResourceGeneratorData _resourceGeneratorData;
        [SerializeField] private List<ResourceCost> _resourceCosts;

        public Sprite Sprite => _sprite;
        public KeyCode KeyCode => _keyCode;
        public ResourceGenerator ResourceGenerator => _resourceGenerator;
        public ResourceGeneratorData ResourceGeneratorData => _resourceGeneratorData;
        public List<ResourceCost> ResourceCosts => _resourceCosts;
    }
}
