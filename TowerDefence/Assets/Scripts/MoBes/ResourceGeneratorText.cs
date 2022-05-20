using nsResourceGenerator;
using TMPro;
using UnityEngine;

namespace nsResourceGeneratorText
{
    public class ResourceGeneratorText : MonoBehaviour
    {
        [SerializeField] private ResourceGenerator _resourceGenerator;

        private TextMeshProUGUI _textMeshPro;

        private void OnEnable()
        {
            _resourceGenerator.OnOverlapCircleAll += ResourceGenerator_OnOverlapCircleAll;
        }

        private void OnDisable()
        {
            _resourceGenerator.OnOverlapCircleAll -= ResourceGenerator_OnOverlapCircleAll;
        }

        private void ResourceGenerator_OnOverlapCircleAll(int nearbyResourceNodeCount, int totalAmountPerCycle)
        {
            string text = string.Concat('+', totalAmountPerCycle, ' ', '(', nearbyResourceNodeCount, ')');
            _textMeshPro.SetText(text);
        }

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }
}
