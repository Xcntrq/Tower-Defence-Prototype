using nsBuildingDistance;
using nsBuildingType;
using nsNearbyResourceNodeFinder;
using nsResourceGeneratorData;
using nsResourceStorage;
using nsSpriteParent;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsResourceGenerator
{
    public class ResourceGenerator : SpriteParent
    {
        [SerializeField] private BuildingType _buildingType;
        [SerializeField] private BuildingDistance _buildingDistance;

        private NearbyResourceNodeFinder _nearbyResourceNodeFinder;
        private ResourceGeneratorData _resourceGeneratorData;
        private ResourceStorage _resourceStorage;
        private BoxCollider2D _boxCollider2D;

        private float _timeLeft;
        private float _timeCost;
        private int _nearbyResourceNodeCount;
        private int _totalAmountPerCycle;
        private bool _isInitialized;

        public Action<int, int> OnOverlapCircleAll;

        public Action<bool> OnSetNodeDetectionCircleActive;
        public Action<float> OnSetNodeDetectionCircleDistance;

        public Action<bool> OnSetAntiBuildingColliderActive;

        public Action<bool> OnSetBuildingCirclesActive;
        public Action<BuildingDistance> OnSetBuildingCirclesDistance;

        public Action<int> OnGetToWork;

        public void Initialize(ResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
            FindNearbyResourceNodes();
            if (_nearbyResourceNodeCount > 0) OnGetToWork?.Invoke(_nearbyResourceNodeCount);
            _isInitialized = true;
        }

        private void Awake()
        {
            _nearbyResourceNodeFinder = new NearbyResourceNodeFinder();
            _resourceGeneratorData = _buildingType.ResourceGeneratorData;
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _timeCost = _resourceGeneratorData.TimeCost;
            _timeLeft = _timeCost;
            _isInitialized = false;
            _totalAmountPerCycle = 0;
        }

        private void Start()
        {
            OnSetBuildingCirclesDistance?.Invoke(_buildingDistance);
            OnSetNodeDetectionCircleDistance?.Invoke(_resourceGeneratorData.NodeDetectionRadius);
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

        public HashSet<SpriteParent> FindNearbyResourceNodes()
        {
            HashSet<SpriteParent> desiredNearbyResourceNodes = _nearbyResourceNodeFinder.OverlapCircleAll(transform.position, _resourceGeneratorData.NodeDetectionRadius, _resourceGeneratorData.ResourceType);
            _nearbyResourceNodeCount = desiredNearbyResourceNodes.Count;
            _totalAmountPerCycle = _nearbyResourceNodeCount * _resourceGeneratorData.AmountPerCycle;
            OnOverlapCircleAll?.Invoke(_nearbyResourceNodeCount, _totalAmountPerCycle);
            return desiredNearbyResourceNodes;
        }

        public HashSet<Collider2D> GetOverlappingColliders()
        {
            var colliders = new List<Collider2D>();
            ContactFilter2D contactFilter2D = new ContactFilter2D().NoFilter();
            _boxCollider2D.OverlapCollider(contactFilter2D, colliders);
            return new HashSet<Collider2D>(colliders);
        }

        public bool IsWithinPylonRange()
        {
            bool isWithinPylonRange = false;
            Vector2 point = (Vector2)transform.position + _boxCollider2D.offset;
            Collider2D[] allOverlappingColliders = Physics2D.OverlapCircleAll(point, _buildingDistance.Max);
            foreach (Collider2D collider in allOverlappingColliders)
            {
                ResourceGenerator resourceGenerator = collider.GetComponent<ResourceGenerator>();
                isWithinPylonRange |= (resourceGenerator != null) && (resourceGenerator != this);
            }
            return isWithinPylonRange;
        }

        public void SetNodeDetectionCircleActive(bool value)
        {
            OnSetNodeDetectionCircleActive?.Invoke(value);
        }

        public void SetAntiBuildingColliderActive(bool value)
        {
            OnSetAntiBuildingColliderActive?.Invoke(value);
        }

        public void SetBuildingCirclesActive(bool value)
        {
            OnSetBuildingCirclesActive?.Invoke(value);
        }
    }
}
