using UnityEngine;

namespace nsBuildingType
{
    [CreateAssetMenu(menuName = "ScObs/BuildingType")]
    public class BuildingType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Transform _prefab;

        public Transform Prefab => _prefab;
    }
}
