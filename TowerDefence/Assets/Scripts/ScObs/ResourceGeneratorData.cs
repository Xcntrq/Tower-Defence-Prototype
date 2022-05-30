using nsResourceType;
using UnityEngine;

namespace nsResourceGeneratorData
{
    [CreateAssetMenu(menuName = "ScObs/ResourceGeneratorData")]
    public class ResourceGeneratorData : ScriptableObject
    {
        [SerializeField] private int _ticksInCycle;
        [SerializeField] private int _amountPerCycle;
        [SerializeField] private ResourceType _resourceType;

        public int TicksInCycle => _ticksInCycle;
        public int AmountPerCycle => _amountPerCycle;
        public ResourceType ResourceType => _resourceType;
    }
}
