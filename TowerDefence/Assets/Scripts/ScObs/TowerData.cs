using UnityEngine;

namespace nsTowerData
{
    [CreateAssetMenu(menuName = "ScObs/TowerData")]
    public class TowerData : ScriptableObject
    {
        [SerializeField] private float _searchCooldown;
        [SerializeField] private float _attackCooldown;

        public float SearchCooldown => _searchCooldown;
        public float AttackCooldown => _attackCooldown;
    }
}
