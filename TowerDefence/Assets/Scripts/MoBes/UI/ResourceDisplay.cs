using nsResourceStorage;
using nsResourceType;
using nsResourceTypes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nsResourceDisplay
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private ResourceTypes _resourceTypes;
        [SerializeField] private GameObject _pfItem;
        [SerializeField] private ResourceStorage _resourceStorage;

        private Dictionary<ResourceType, TextMeshProUGUI> _items;

        private void OnEnable()
        {
            _resourceStorage.OnAmountChange += ResourceStorage_OnAmountChanged;
        }

        private void OnDisable()
        {
            _resourceStorage.OnAmountChange -= ResourceStorage_OnAmountChanged;
        }

        private void Awake()
        {
            _items = new Dictionary<ResourceType, TextMeshProUGUI>();
            foreach (ResourceType resourceType in _resourceTypes.List)
            {
                GameObject newItem = Instantiate(_pfItem, transform);
                newItem.GetComponentInChildren<Image>().sprite = resourceType.Sprite;
                _items[resourceType] = newItem.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void ResourceStorage_OnAmountChanged(ResourceType resourceType, int amount)
        {
            string newText = amount.ToString();
            _items[resourceType].SetText(newText);
        }

        //private void UpdateItems()
        //{
        //    foreach (ResourceType resourceType in _resourceTypes.List)
        //    {
        //        string value = _resourceStorage.GetResourceAmount(resourceType).ToString();
        //        _items[resourceType].SetText(value);
        //    }
        //}
    }
}
