using nsColorValue;
using UnityEngine;

namespace nsResourceType
{
    [CreateAssetMenu(menuName = "ScObs/ResourceType")]
    public class ResourceType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private ColorValue _color;
        [SerializeField] private int _amountAtStart;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public Color Color => _color.Value;
        public int AmountAtStart => _amountAtStart;
    }
}
