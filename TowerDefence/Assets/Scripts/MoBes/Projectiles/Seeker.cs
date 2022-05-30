using nsProjectile;

namespace nsSeeker
{
    public class Seeker : Projectile
    {
        protected override void AdjustDirection()
        {
            _direction = (_target.position - _transform.position).normalized;
        }
    }
}
