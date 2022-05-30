using UnityEngine;

namespace nsEnemyData
{
    [CreateAssetMenu(menuName = "ScObs/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _detectionRadius;

        public int MaxHealth => _maxHealth;
        public int Damage => _damage;
        public float Speed => _speed;
        public float DetectionRadius => _detectionRadius;
    }
}
