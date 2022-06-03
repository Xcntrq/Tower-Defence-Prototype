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
        private TimeTicker _timeTicker;

        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;
        private float _inverseLimit;

        public event Action<string> OnOverlapCircleAll;
        public event Action<float> OnProgressChange;
        public event Action<int> OnGetToWork;

        public void Initialize(BuildingPlacer buildingPlacer, ResourceStorage resourceStorage, TimeTicker timeTicker)
        {
            Initialize(buildingPlacer, resourceStorage);

            _timeTicker = timeTicker;
            _timeTicker.OnTick += TimeTicker_OnTick;
        }

        protected override void Awake()
        {
            base.Awake();

            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _inverseLimit = 1f / (_resourceGeneratorData.TicksInCycle - 1);
            _nearbyResourceNodeCount = 0;
            _totalAmountPerCycle = 0;
            _timeTicker = null;
        }

        protected override void Start()
        {
            base.Start();

            OnOverlapCircleAll?.Invoke(string.Empty);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_timeTicker != null) _timeTicker.OnTick -= TimeTicker_OnTick;
        }

        private void TimeTicker_OnTick(ResourceGeneratorData resourceGeneratorData, int tick)
        {
            if ((_buildingState != BuildingState.Working) || (_resourceGeneratorData != resourceGeneratorData)) return;
            if (tick == 0) _resourceStorage.AddResource(_resourceGeneratorData.ResourceType, _totalAmountPerCycle);
            OnProgressChange?.Invoke(tick * _inverseLimit);
        }

        protected override void StartWorking()
        {
            base.StartWorking();

            FindNearbyResourceNodes();
            if (_nearbyResourceNodeCount > 0) OnGetToWork?.Invoke(_nearbyResourceNodeCount);
        }

        public HashSet<Colorable> FindNearbyResourceNodes()
        {
            HashSet<Colorable> desiredNearbyResourceNodes = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, BuildingType.ActionRadius, _resourceGeneratorData.ResourceType);
            _nearbyResourceNodeCount = desiredNearbyResourceNodes.Count;
            _totalAmountPerCycle = _nearbyResourceNodeCount * _resourceGeneratorData.AmountPerCycle;
            string text = string.Concat('+', _totalAmountPerCycle, ' ', '(', _nearbyResourceNodeCount, ')');
            OnOverlapCircleAll?.Invoke(text);
            return desiredNearbyResourceNodes;
        }
    }
}
