using nsBuildingPlacer;
using nsHealth;
using nsColorable;
using System.Collections;
using UnityEngine;
using nsBuilding;
using nsIHealthCarrier;
using nsEnemyData;

namespace nsEnemy
{
    public class Enemy : Colorable, IHealthCarrier
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Transform _aim;

        private Rigidbody2D _rigidbody2D;
        private Transform _transform;
        private Transform _target;
        private Vector3 _direction;

        private Vector3 _defaultScale;
        private Vector3 _invertedScale;

        public Transform Aim => _aim;
        public int MaxHealth => _enemyData.MaxHealth;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _transform = transform;
            _defaultScale = _transform.localScale;
            _invertedScale = _transform.localScale;
            _invertedScale.x *= -1;
            _target = null;
            var random = new System.Random();
            float cooldown = 0.5f + (float)random.NextDouble() / 2;
            StartCoroutine(TargetSearch(cooldown));
        }

        private void FixedUpdate()
        {
            if (_target == null) return;
            _direction = (_target.position - _transform.position).normalized;
            _rigidbody2D.velocity = _direction * _enemyData.Speed;
            _transform.localScale = _direction.x >= 0 ? _defaultScale : _invertedScale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Building building = collision.gameObject.GetComponent<Building>();
            if (building != null)
            {
                Health health = building.GetComponent<Health>();
                if (health != null)
                {
                    health.Decrease(_enemyData.Damage);
                    Destroy(gameObject);
                }
            }
        }

        private IEnumerator TargetSearch(float cooldown)
        {
            var waitForSeconds = new WaitForSeconds(cooldown);

            while (true)
            {
                if (_target == null)
                {
                    Transform newTarget = null;
                    float minDistance = float.MaxValue;
                    foreach (Building building in _buildingPlacer.PlacedBuildings)
                    {
                        Transform newTransform = building.transform;
                        float newDistance = Vector3.Distance(_transform.position, newTransform.position);
                        if (newDistance < minDistance)
                        {
                            minDistance = newDistance;
                            newTarget = newTransform;
                        }
                    }
                    _target = newTarget;
                }

                Collider2D[] allNearbyColliders = Physics2D.OverlapCircleAll(_transform.position, _enemyData.DetectionRadius);
                foreach (Collider2D nearbyCollider in allNearbyColliders)
                {
                    Building building = nearbyCollider.GetComponent<Building>();
                    if (building == null) continue;
                    bool isPlaced = _buildingPlacer.PlacedBuildings.Contains(building);
                    if (!isPlaced) continue;
                    if (_target == null)
                    {
                        _target = building.transform;
                        continue;
                    }
                    Transform newTransform = building.transform;
                    float currentDistance = Vector3.Distance(_transform.position, _target.position);
                    float newDistance = Vector3.Distance(_transform.position, newTransform.position);
                    if (newDistance < currentDistance) _target = newTransform;
                }

                yield return waitForSeconds;
            }
        }
    }
}
