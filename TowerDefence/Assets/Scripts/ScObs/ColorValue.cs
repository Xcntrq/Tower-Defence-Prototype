using UnityEngine;

namespace nsColorValue
{
    [CreateAssetMenu(menuName = "ScObs/ColorValue")]
    public class ColorValue : ScriptableObject
    {
        [SerializeField] private Color _value;

        public Color Value => _value;
    }
}
