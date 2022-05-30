using UnityEngine;

namespace nsProjectileData
{
    [CreateAssetMenu(menuName = "ScObs/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _timeToLive;

        public int Damage => _damage;
        public float Speed => _speed;
        public float TimeToLive => _timeToLive;
    }
}
