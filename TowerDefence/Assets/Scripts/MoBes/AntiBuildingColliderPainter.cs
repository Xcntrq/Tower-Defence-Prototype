using nsSpriteParent;
using UnityEngine;

namespace nsAntiBuildingColliderPainter
{
    public class AntiBuildingColliderPainter : MonoBehaviour
    {
        [SerializeField] private SpriteParent _spriteParent;

        public SpriteParent SpriteParent => _spriteParent;
    }
}
