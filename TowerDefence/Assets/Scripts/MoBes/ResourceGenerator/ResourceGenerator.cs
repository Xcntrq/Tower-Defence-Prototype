using nsBuilding;
using nsBuildingPlacer;
using nsNearbyResourceNodeFinder;
using nsResourceGeneratorData;
using nsResourceStorage;
using nsColorable;
using System;
using System.Collections.Generic;
using UnityEngine;
using nsTimeTicker;

namespace nsResourceGenerator
{
    public class ResourceGenerator : Building
    {
        [SerializeField] private ResourceGeneratorData _resourceGeneratorData;

        private NearbyResourceNodeFinder _nearbyResourceNodeFinder;
        private ResourceStorage _resourceStorage;
        private TimeTicker _timeTicker;

        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;
        private float _inverseLimit;

        public event Action<int, int> OnOverlapCircleAll;
        public event Action<float> OnProgressChange;
        public event Action<int> OnGetToWork;

        public void Initialize(BuildingPlacer buildingPlacer, ResourceStorage resourceStorage, TimeTicker timeTicker)
        {
            Initialize(buildingPlacer);

            _timeTicker = timeTicker;
            _timeTicker.OnTick += TimeTicker_OnTick;
            _resourceStorage = resourceStorage;
            FindNearbyResourceNodes();
            if (_nearbyResourceNodeCount > 0) OnGetToWork?.Invoke(_nearbyResourceNodeCount);
        }

        protected override void Awake()
        {
            base.Awake();

            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _inverseLimit = 1f / (_resourceGeneratorData.TicksInCycle - 1);
            _nearbyResourceNodeCount = 0;
            _totalAmountPerCycle = 0;
        }

        protected override void Start()
        {
            base.Start();

            OnOverlapCircleAll?.Invoke(_nearbyResourceNodeCount, _totalAmountPerCycle);
        }

        private void TimeTicker_OnTick(ResourceGeneratorData resourceGeneratorData, int tick)
        {
            if (_resourceGeneratorData != resourceGeneratorData) return;
            if (tick == 0) _resourceStorage.AddResource(_resourceGeneratorData.ResourceType, _totalAmountPerCycle);
            OnProgressChange?.Invoke(tick * _inverseLimit);
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
