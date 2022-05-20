using System.Collections.Generic;
using UnityEngine;

namespace nsSpriteList
{
    [CreateAssetMenu(menuName = "ScObs/SpriteList")]
    public class SpriteList : ScriptableObject
    {
        [SerializeField] private List<Sprite> _items;

        public List<Sprite> Items => _items;
    }
}
