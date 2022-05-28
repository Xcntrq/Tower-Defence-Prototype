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
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _actionRadius;
        [SerializeField] private List<ResourceCost> _resourceCosts;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public KeyCode KeyCode => _keyCode;
        public int MaxHealth => _maxHealth;
        public float ActionRadius => _actionRadius;
        public List<ResourceCost> ResourceCosts => _resourceCosts;
    }
}
