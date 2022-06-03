using nsResourceGenerator;
using TMPro;
using UnityEngine;

namespace nsResourceGeneratorText
{
    public class ResourceGeneratorText : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
            _resourceGenerator.OnOverlapCircleAll += ResourceGenerator_OnOverlapCircleAll;
        }

        private void ResourceGenerator_OnOverlapCircleAll(string text)
        {
            _textMeshPro.SetText(text);
        }
    }
}
