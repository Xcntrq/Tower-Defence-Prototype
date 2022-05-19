using nsColorValue;
using System.Collections.Generic;
using UnityEngine;

namespace nsColorList
{
    [CreateAssetMenu(menuName = "ScObs/ColorList")]
    public class ColorList : ScriptableObject
    {
        [SerializeField] private List<ColorValue> _items;

        public List<ColorValue> Items => _items;
    }
}
