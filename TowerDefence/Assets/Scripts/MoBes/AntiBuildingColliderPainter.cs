using nsIColorable;
using nsColorable;
using UnityEngine;

namespace nsAntiBuildingColliderPainter
{
    public class AntiBuildingColliderPainter : MonoBehaviour, IColorableCarrier
    {
        [SerializeField] private Colorable _colorable;

        public Colorable Colorable => _colorable;
    }
}
