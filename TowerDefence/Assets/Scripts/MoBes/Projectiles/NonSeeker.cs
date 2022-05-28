using nsProjectile;
using UnityEngine;

namespace nsNonSeeker
{
    public class NonSeeker : Projectile
    {
        private bool _isDirectionSet;

        protected override void Awake()
        {
            base.Awake();

            _isDirectionSet = false;
        }

        protected override void AdjustDirection()
        {
            if (_isDirectionSet) return;
            _direction = (_target.position - _transform.position).normalized;
            _isDirectionSet = true;
        }
    }
}
