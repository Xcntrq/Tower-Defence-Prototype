using UnityEngine;

namespace nsResourceType
{
    [CreateAssetMenu(menuName = "ScObs/ResourceType")]
    public class ResourceType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _amountAtStart;

        public Sprite Sprite => _sprite;
        public int AmountAtStart => _amountAtStart;
    }
}
