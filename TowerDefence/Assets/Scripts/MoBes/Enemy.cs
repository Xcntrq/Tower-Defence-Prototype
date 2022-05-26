using nsBuildingPlacer;
using nsHealth;
using nsResourceGenerator;
using nsSpriteParent;
using System.Collections;
using UnityEngine;

namespace nsEnemy
{
    public class Enemy : SpriteParent
    {
        [SerializeField] private BuildingPlacer _buildingPlacer;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _detectionRadius;

        private Rigidbody2D _rigidbody2D;
        private Transform _transform;
        private Transform _target;
        private Vector3 _direction;

        private Vector3 _defaultScale;
        private Vector3 _invertedScale;

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
            _rigidbody2D.velocity = _direction * _speed;
            _transform.localScale = _direction.x >= 0 ? _defaultScale : _invertedScale;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ResourceGenerator resourceGenerator = collision.gameObject.GetComponent<ResourceGenerator>();
            if (resourceGenerator != null)
            {
                Health health = resourceGenerator.GetComponent<Health>();
                if (health != null)
                {
                    health.Decrease(_damage);
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
                    foreach (ResourceGenerator resourceGenerator in _buildingPlacer.PlacedResourceGenerators)
                    {
                        Transform newTransform = resourceGenerator.transform;
                        float newDistance = Vector3.Distance(_transform.position, newTransform.position);
                        if (newDistance < minDistance)
                        {
                            minDistance = newDistance;
                            newTarget = newTransform;
                        }
                    }
                    _target = newTarget;
                }

                Collider2D[] allNearbyColliders = Physics2D.OverlapCircleAll(_transform.position, _detectionRadius);
                foreach (Collider2D nearbyCollider in allNearbyColliders)
                {
                    ResourceGenerator resourceGenerator = nearbyCollider.GetComponent<ResourceGenerator>();
                    if (resourceGenerator == null) continue;
                    bool isPlaced = _buildingPlacer.PlacedResourceGenerators.Contains(resourceGenerator);
                    if (!isPlaced) continue;
                    if (_target == null)
                    {
                        _target = resourceGenerator.transform;
                        continue;
                    }
                    Transform newTransform = resourceGenerator.transform;
                    float currentDistance = Vector3.Distance(_transform.position, _target.position);
                    float newDistance = Vector3.Distance(_transform.position, newTransform.position);
                    if (newDistance < currentDistance) _target = newTransform;
                }

                yield return waitForSeconds;
            }
        }
    }
}
