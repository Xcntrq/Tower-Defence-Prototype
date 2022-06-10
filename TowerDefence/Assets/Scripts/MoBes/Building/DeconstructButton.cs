using nsBuilding;
using nsResourceCost;
using nsResourceStorage;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace nsDeconstructButton
{
    public class DeconstructButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Building _building;
        [SerializeField] private float _showupDelay;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
            _building.OnMouseEnterCustom += OnPointerEnter;
            _building.OnMouseExitCustom += OnPointerExit;
            gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartCoroutine(Deactivate());
        }

        private IEnumerator Deactivate()
        {
            var waitForSeconds = new WaitForSeconds(_showupDelay);
            yield return waitForSeconds;
            gameObject.SetActive(false);
        }

        private void OnButtonClick()
        {
            foreach (ResourceCost resourceCost in _building.BuildingType.ResourceCosts)
            {
                _building.ResourceStorage.AddResource(resourceCost.ResourceType, resourceCost.Value / 2);
            }
            Destroy(_building.gameObject);
        }
    }
}
