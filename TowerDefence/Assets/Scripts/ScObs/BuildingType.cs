using nsColorValue;
using nsResourceCost;
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
        [SerializeField] private float _buildingTimerDelay;
        //[SerializeField] private ColorValue _colorWhite;
        //[SerializeField] private ColorValue _colorTransparent;
        [SerializeField] private List<ResourceCost> _resourceCosts;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public KeyCode KeyCode => _keyCode;
        public int MaxHealth => _maxHealth;
        public float ActionRadius => _actionRadius;
        public float BuildingTimerDelay => _buildingTimerDelay;
        //public ColorValue ColorWhite => _colorWhite;
        //public ColorValue ColorTransparent => _colorTransparent;
        public List<ResourceCost> ResourceCosts => _resourceCosts;
    }
}
