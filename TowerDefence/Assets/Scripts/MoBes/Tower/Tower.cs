using nsBuilding;
using nsEnemy;
using nsProjectile;
using nsProjectileOrigin;
using nsTowerData;
using System.Collections;
using UnityEngine;

namespace nsTower
{
    public class Tower : Building
    {
        [SerializeField] private TowerData _towerData;
        [SerializeField] private Projectile _projectile;
        [SerializeField] private ProjectileOrigin _projectileOrigin;

        private Transform _target;

        public override void SetBuildingCirclesActive(bool value)
        {
            base.SetBuildingCirclesActive(value);

            //ActionRangeCircleToggle(value);
        }

        protected override void StartWorking()
        {
            base.StartWorking();

            StartCoroutine(TargetSearch(_towerData.SearchCooldown));
            StartCoroutine(Attack(_towerData.AttackCooldown));
        }

        private IEnumerator TargetSearch(float cooldown)
        {
            var waitForSeconds = new WaitForSeconds(cooldown);

            while (_buildingState == BuildingState.Working)
            {
                yield return waitForSeconds;

                Collider2D[] allNearbyColliders = Physics2D.OverlapCircleAll(transform.position, BuildingType.ActionRadius);

                float minDistance = float.MaxValue;
                foreach (Collider2D nearbyCollider in allNearbyColliders)
                {
                    Enemy enemy = nearbyCollider.GetComponent<Enemy>();
                    if (enemy == null) continue;
                    Transform currentTransform = enemy.Aim;
                    float currentDistance = Vector3.Distance(transform.position, currentTransform.position);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        _target = currentTransform;
                    }
                }
            }
        }

        private IEnumerator Attack(float cooldown)
        {
            var waitForSeconds = new WaitForSeconds(cooldown);

            while (_buildingState == BuildingState.Working)
            {
                yield return waitForSeconds;

                if (_target == null) continue;

                Projectile newProjectile = Instantiate(_projectile, transform, false);
                newProjectile.transform.position = _projectileOrigin.transform.position;
                newProjectile.Initialize(_target);
            }
        }
    }
}
