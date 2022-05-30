using nsBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace nsBuildings
{
    [CreateAssetMenu(menuName = "ScObs/Buildings")]
    public class Buildings : ScriptableObject
    {
        [SerializeField] private List<Building> _list;

        public List<Building> List => _list;
    }
}
