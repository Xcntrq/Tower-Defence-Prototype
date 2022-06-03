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
using System.Collections;

namespace nsBuilding
{
    public enum BuildingState
    {
        Undefined,
        Building,
        Working
    }

    public abstract class Building : Colorable, IHealthCarrier
    {
        [SerializeField] private BuildingDistance _buildingDistance;
        [SerializeField] private BuildingType _buildingType;
        [SerializeField] private List<Transform> _buildingBlocks;

        protected BuildingState _buildingState;
        private bool _isGhost;

        protected ResourceStorage _resourceStorage;
        private BuildingPlacer _buildingPlacer;
        private Collider2D _collider2D;

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
            _isGhost = false;
            _resourceStorage = resourceStorage;
            _buildingPlacer = buildingPlacer;
            _buildingState = BuildingState.Building;
            StartCoroutine(StartBuilding(_buildingType.BuildingTimerDelay));
        }

        protected virtual void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _buildingState = BuildingState.Undefined;
        }

        protected virtual void Start()
        {
            OnBuildingCirclesDistanceChange?.Invoke(_buildingDistance);
            OnActionRadiusChange?.Invoke(_buildingType.ActionRadius);
            SetActive(_isGhost);
        }

        protected virtual void OnDestroy()
        {
            if (_buildingPlacer != null) _buildingPlacer.ForgetBuilding(this);
        }

        private void OnMouseEnter()
        {
            if (_buildingState != BuildingState.Undefined) OnMouseEnterCustom?.Invoke(null);
        }

        private void OnMouseExit()
        {
            if (_buildingState != BuildingState.Undefined) OnMouseExitCustom?.Invoke(null);
        }

        public void BecomeGhost()
        {
            _isGhost = true;
            _collider2D.isTrigger = true;
            ActionRangeCircleToggle(true);
            OnAntiBuildingColliderToggle?.Invoke(false);
        }

        protected void ActionRangeCircleToggle(bool value)
        {
            OnActionRangeCircleToggle?.Invoke(value);
        }

        public virtual void SetBuildingCirclesActive(bool value)
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

        private IEnumerator StartBuilding(float delay)
        {
            var waitForSeconds = new WaitForSeconds(delay);

            foreach (Transform buildingBlock in _buildingBlocks)
            {
                foreach (Transform child in buildingBlock)
                {
                    child.gameObject.SetActive(true);
                    yield return waitForSeconds;
                }
            }

            SetActive(true);
            StartWorking();
        }

        protected virtual void StartWorking()
        {
            _buildingState = BuildingState.Working;
        }
    }
}
