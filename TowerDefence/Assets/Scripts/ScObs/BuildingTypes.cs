using nsBuildingType;
using System.Collections.Generic;
using UnityEngine;

namespace nsBuildingTypes
{
    [CreateAssetMenu(menuName = "ScObs/BuildingTypes")]
    public class BuildingTypes : ScriptableObject
    {
        [SerializeField] private List<BuildingType> _list;

        public List<BuildingType> List => _list;
    }
}
