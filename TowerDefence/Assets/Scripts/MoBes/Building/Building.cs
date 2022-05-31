using nsBuildingDistance;
using nsBuildingType;
using nsIHealthCarrier;
using nsColorable;
using System;
using UnityEngine;
using nsBuildingPlacer;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using nsResourceStorage;

namespace nsBuilding
{
    public abstract class Building : Colorable, IHealthCarrier
    {
        [SerializeField] private BuildingDistance _buildingDistance;
        [SerializeField] private BuildingType _buildingType;

        protected ResourceStorage _resourceStorage;
        private BuildingPlacer _buildingPlacer;
        private Collider2D _collider2D;
        protected bool _isInitialized;

        public int MaxHealth => _buildingType.MaxHealth;
        public BuildingType BuildingType => _buildingType;
        public ResourceStorage ResourceStorage => _resourceStorage;

        //Building Circles
        public event Action<BuildingDistance> OnBuildingCirclesDistanceChange;
        public event Action<bool> OnBuildingCirclesToggle;

        //Action Range Circle
        public event Action<float> OnActionRadiusChange;
        public event Action<bool> OnActionRangeCircleToggle;

        //Anti-Building Collider
        public event Action<bool> OnAntiBuildingColliderToggle;

        //Deconstruct Button
        public event Action<PointerEventData> OnMouseEnterCustom;
        public event Action<PointerEventData> OnMouseExitCustom;

        public void Initialize(BuildingPlacer buildingPlacer, ResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
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

        protected virtual void OnDestroy()
        {
            if (_buildingPlacer != null) _buildingPlacer.ForgetBuilding(this);
        }

        private void OnMouseEnter()
        {
            if (_isInitialized) OnMouseEnterCustom?.Invoke(null);
        }

        private void OnMouseExit()
        {
            if (_isInitialized) OnMouseExitCustom?.Invoke(null);
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
            Collider2D[] allOverlappingColliders = Physics2D.OverlapCircleAll(point, _buildingDistance.Max - 1);
            foreach (Collider2D collider in allOverlappingColliders)
            {
                Building building = collider.GetComponent<Building>();
                isWithinPylonRange |= (building != null) && (building != this);
            }
            return isWithinPylonRange;
        }
    }
}
