using nsBuildingType;
using nsNearbyResourceNodeFinder;
using nsResourceGeneratorData;
using nsResourceNode;
using nsResourceStorage;
using TMPro;
using UnityEngine;

namespace nsResourceGenerator
{
    public class ResourceGenerator : MonoBehaviour
    {
        [SerializeField] private BuildingType _buildingType;

        private NearbyResourceNodeFinder _nearbyResourceNodeFinder;
        private ResourceGeneratorData _resourceGeneratorData;
        private ResourceStorage _resourceStorage;

        private float _timeLeft;
        private float _timeCost;
        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;
        private string _text;

        public void Initialize(ResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
        }

        private void Awake()
        {
            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _resourceGeneratorData = _buildingType.ResourceGeneratorData;
            _timeCost = _resourceGeneratorData.TimeCost;
            _timeLeft = _timeCost;
        }

        private void Start()
        {
            _nearbyResourceNodeCount = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, _resourceGeneratorData.NodeDetectionRadius, _resourceGeneratorData.ResourceType);
            _totalAmountPerCycle = _nearbyResourceNodeCount * _resourceGeneratorData.AmountPerCycle;
            _text = string.Concat('+', _totalAmountPerCycle, ' ', '(', _nearbyResourceNodeCount, ')');
            GetComponentInChildren<TextMeshProUGUI>().SetText(_text);
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _timeLeft += _timeCost;
                _resourceStorage.AddResource(_resourceGeneratorData.ResourceType, _totalAmountPerCycle);
            }
        }
    }
}
