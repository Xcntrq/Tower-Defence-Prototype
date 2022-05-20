using nsBuildingType;
using nsNearbyResourceNodeFinder;
using nsResourceGeneratorData;
using nsResourceNode;
using nsResourceStorage;
using System;
using System.Collections.Generic;
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
        private bool _isInitialized;

        public Action<int, int> OnOverlapCircleAll;
        public Action<bool> OnSetTransparent;
        public Action<bool, float> OnSetNodeDetectionCircleActive;

        public void Initialize(ResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
            _isInitialized = true;
        }

        private void Awake()
        {
            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _resourceGeneratorData = _buildingType.ResourceGeneratorData;
            _timeCost = _resourceGeneratorData.TimeCost;
            _timeLeft = _timeCost;
            _isInitialized = false;
        }

        private void Start()
        {
            FindNearbyResourceNodes();
        }

        private void Update()
        {
            if (!_isInitialized) return;

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _timeLeft += _timeCost;
                _resourceStorage.AddResource(_resourceGeneratorData.ResourceType, _totalAmountPerCycle);
            }
        }

        public HashSet<ResourceNode> FindNearbyResourceNodes()
        {
            HashSet<ResourceNode> desiredNearbyResourceNodes = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, _resourceGeneratorData.NodeDetectionRadius, _resourceGeneratorData.ResourceType);
            _nearbyResourceNodeCount = desiredNearbyResourceNodes.Count;
            _totalAmountPerCycle = _nearbyResourceNodeCount * _resourceGeneratorData.AmountPerCycle;
            OnOverlapCircleAll?.Invoke(_nearbyResourceNodeCount, _totalAmountPerCycle);
            return desiredNearbyResourceNodes;
        }

        public void SetTransparent(bool value)
        {
            OnSetTransparent?.Invoke(value);
        }

        public void SetNodeDetectionCircleActive(bool value)
        {
            OnSetNodeDetectionCircleActive?.Invoke(value, _resourceGeneratorData.NodeDetectionRadius);
        }
    }
}
