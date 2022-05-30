using nsBuilding;
using nsBuildingPlacer;
using nsNearbyResourceNodeFinder;
using nsResourceGeneratorData;
using nsResourceStorage;
using nsColorable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceGenerator
{
    public class ResourceGenerator : Building
    {
        [SerializeField] private ResourceGeneratorData _resourceGeneratorData;

        private NearbyResourceNodeFinder _nearbyResourceNodeFinder;
        private ResourceStorage _resourceStorage;

        private float _timeLeft;
        private float _cycleTime;
        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;

        public event Action<int, int> OnOverlapCircleAll;
        public event Action<int> OnGetToWork;

        public void Initialize(BuildingPlacer buildingPlacer, ResourceStorage resourceStorage)
        {
            Initialize(buildingPlacer);

            _resourceStorage = resourceStorage;
            FindNearbyResourceNodes();
            if (_nearbyResourceNodeCount > 0) OnGetToWork?.Invoke(_nearbyResourceNodeCount);
        }

        protected override void Awake()
        {
            base.Awake();

            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _cycleTime = _resourceGeneratorData.CycleTime;
            _timeLeft = _cycleTime;
            _totalAmountPerCycle = 0;
            _nearbyResourceNodeCount = 0;
        }

        protected override void Start()
        {
            base.Start();

            OnOverlapCircleAll?.Invoke(_nearbyResourceNodeCount, _totalAmountPerCycle);
        }

        private void Update()
        {
            if (!_isInitialized) return;

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _timeLeft += _cycleTime;
                _resourceStorage.AddResource(_resourceGeneratorData.ResourceType, _totalAmountPerCycle);
            }
        }

        public HashSet<Colorable> FindNearbyResourceNodes()
        {
            HashSet<Colorable> desiredNearbyResourceNodes = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, BuildingType.ActionRadius, _resourceGeneratorData.ResourceType);
            _nearbyResourceNodeCount = desiredNearbyResourceNodes.Count;
            _totalAmountPerCycle = _nearbyResourceNodeCount * _resourceGeneratorData.AmountPerCycle;
            OnOverlapCircleAll?.Invoke(_nearbyResourceNodeCount, _totalAmountPerCycle);
            return desiredNearbyResourceNodes;
        }
    }
}
