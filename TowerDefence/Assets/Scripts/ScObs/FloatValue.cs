using UnityEngine;

namespace nsFloatValue
{
    [CreateAssetMenu(menuName = "ScObs/FloatValue")]
    public class FloatValue : ScriptableObject
    {
        [SerializeField] private float _value;

        public float Value => _value;
    }
}
