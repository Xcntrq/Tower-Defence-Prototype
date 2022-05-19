using nsColorList;
using nsIntValue;
using nsSpriteList;
using UnityEngine;

namespace nsSpriteRendererRandomizer
{
    public class SpriteRendererRandomizer : MonoBehaviour
    {
        [SerializeField] private SpriteList _spriteList;
        [SerializeField] private ColorList _colorList;
        [SerializeField] private IntValue _seed;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null) return;

            var random = _seed.Value == 0 ? new System.Random() : new System.Random(_seed.Value);

            int i = random.Next(_spriteList.Items.Count);
            _spriteRenderer.sprite = _spriteList.Items[i];
            i = random.Next(_colorList.Items.Count);
            _spriteRenderer.color = _colorList.Items[i].Value;
        }
    }
}
