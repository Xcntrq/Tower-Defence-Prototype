using nsGhostBuilding;
using System.Collections;
using TMPro;
using UnityEngine;

namespace nsTooltipDisplayText
{
    public class TooltipDisplayText : MonoBehaviour
    {
        [SerializeField] private GhostBuilding _ghostBuilding;

        private TextMeshProUGUI _text;

        private void OnEnable()
        {
            _ghostBuilding.OnBuildOrderError += GhostBuilding_OnBuildOrderError;
        }

        private void OnDisable()
        {
            _ghostBuilding.OnBuildOrderError -= GhostBuilding_OnBuildOrderError;
        }

        private void GhostBuilding_OnBuildOrderError(string text, float tooltipDelay)
        {
            _text.SetText(text);
            StopAllCoroutines();
            StartCoroutine(HideAfterDelay(tooltipDelay));
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.SetText("");
        }

        IEnumerator HideAfterDelay(float tooltipDelay)
        {
            yield return new WaitForSeconds(tooltipDelay);
            _text.SetText("");
        }
    }
}
