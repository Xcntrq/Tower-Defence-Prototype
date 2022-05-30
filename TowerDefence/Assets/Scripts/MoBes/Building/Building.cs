using nsBuildingDistance;
using nsBuildingType;
using nsIHealthCarrier;
using nsColorable;
using System;
using UnityEngine;
using nsBuildingPlacer;
using System.Collections.Generic;

namespace nsBuilding
{
    public abstract class Building : Colorable, IHealthCarrier
    {
        [SerializeField] private BuildingDistance _buildingDistance;
        [SerializeField] private BuildingType _buildingType;

        private BuildingPlacer _buildingPlacer;
        private Collider2D _collider2D;
        protected bool _isInitialized;

        public int MaxHealth => _buildingType.MaxHealth;
        public BuildingType BuildingType => _buildingType;

        //Building Circles
        public event Action<BuildingDistance> OnBuildingCirclesDistanceChange;
        public event Action<bool> OnBuildingCirclesToggle;

        //Action Range Circle
        public event Action<float> OnActionRadiusChange;
        public event Action<bool> OnActionRangeCircleToggle;

        //Anti-Building Collider
        public event Action<bool> OnAntiBuildingColliderToggle;

        public void Initialize(BuildingPlacer buildingPlacer)
        {
            _buildingPlacer = buildingPlacer;
            _isInitialized = true;
        }

        protected virtual void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _isInitialized = false;
        }

        protected virtual void Start()
        {
            OnBuildingCirclesDistanceChange?.Invoke(_buildingDistance);
            OnActionRadiusChange?.Invoke(_buildingType.ActionRadius);
        }

        private void OnDestroy()
        {
            if (_buildingPlacer != null) _buildingPlacer.ForgetBuilding(this);
        }

        public void BecomeGhost()
        {
            _collider2D.isTrigger = true;
            OnActionRangeCircleToggle?.Invoke(true);
            OnAntiBuildingColliderToggle?.Invoke(false);
        }

        public void SetBuildingCirclesActive(bool value)
        {
            OnBuildingCirclesToggle?.Invoke(value);
        }

        public HashSet<Collider2D> GetOverlappingColliders()
        {
            var colliders = new List<Collider2D>();
            ContactFilter2D contactFilter2D = new ContactFilter2D().NoFilter();
            _collider2D.OverlapCollider(contactFilter2D, colliders);
            return new HashSet<Collider2D>(colliders);
        }

        public bool IsWithinPylonRange()
        {
            bool isWithinPylonRange = false;
            Vector2 point = (Vector2)transform.position + _collider2D.offset;
            Collider2D[] allOverlappingColliders = Physics2D.OverlapCircleAll(point, _buildingDistance.Max);
            foreach (Collider2D collider in allOverlappingColliders)
            {
                Building building = collider.GetComponent<Building>();
                isWithinPylonRange |= (building != null) && (building != this);
            }
            return isWithinPylonRange;
        }
    }
}
