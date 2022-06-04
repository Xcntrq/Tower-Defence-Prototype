using nsColorValue;
using UnityEngine;

namespace nsSpriteColorer
{
    public class SpriteColorer : MonoBehaviour
    {
        [SerializeField] private ColorValue _colorValue;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().color = _colorValue.Value;
        }
    }
}
