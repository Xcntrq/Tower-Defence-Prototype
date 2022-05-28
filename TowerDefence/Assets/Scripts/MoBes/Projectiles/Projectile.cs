using nsEnemy;
using nsHealth;
using nsProjectileData;
using UnityEngine;

namespace nsProjectile
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileData _projectileData;

        private Rigidbody2D _rigidbody2D;
        protected Transform _transform;
        protected Transform _target;
        protected Vector3 _direction;
        private Vector3 _rotation;
        private bool _isInitialized;

        public void Initialize(Transform target)
        {
            _target = target;
            _isInitialized = true;
        }

        protected virtual void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _transform = transform;
            _direction = Vector3.zero;
            _rotation = Vector3.zero;
            _isInitialized = false;
        }

        private void FixedUpdate()
        {
            if (!_isInitialized) return;
            if (_target != null)
            {
                AdjustDirection();
            }
            else
            {
                Destroy(gameObject, _projectileData.TimeToLive);
            }
            _rigidbody2D.velocity = _projectileData.Speed * _direction;
            _rotation.z = Mathf.Rad2Deg * Mathf.Atan2(_direction.y, _direction.x);
            _transform.eulerAngles = _rotation;
        }

        protected abstract void AdjustDirection();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Health health = enemy.GetComponent<Health>();
                if (health != null)
                {
                    health.Decrease(_projectileData.Damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}
