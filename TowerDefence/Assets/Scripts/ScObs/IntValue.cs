using UnityEngine;

namespace nsIntValue
{
    [CreateAssetMenu(menuName = "ScObs/IntValue")]
    public class IntValue : ScriptableObject
    {
        [SerializeField] private int _value;

        public int Value => _value;
    }
}
