using nsResourceGeneratorData;
using UnityEngine;

namespace nsBuildingType
{
    [CreateAssetMenu(menuName = "ScObs/BuildingType")]
    public class BuildingType : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Transform _prefab;
        [SerializeField] private ResourceGeneratorData _resourceGeneratorData;

        public Transform Prefab => _prefab;
        public ResourceGeneratorData ResourceGeneratorData => _resourceGeneratorData;
    }
}
