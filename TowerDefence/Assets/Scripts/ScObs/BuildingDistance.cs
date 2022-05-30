using UnityEngine;

namespace nsBuildingDistance
{
    [CreateAssetMenu(menuName = "ScObs/BuildingDistance")]
    public class BuildingDistance : ScriptableObject
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;
    }
}
