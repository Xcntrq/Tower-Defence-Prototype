using nsResourceType;
using UnityEngine;

namespace nsResourceGeneratorData
{
    [CreateAssetMenu(menuName = "ScObs/ResourceGeneratorData")]
    public class ResourceGeneratorData : ScriptableObject
    {
        [SerializeField] private int _amountPerCycle;
        [SerializeField] private float _cycleTime;
        [SerializeField] private ResourceType _resourceType;

        public int AmountPerCycle => _amountPerCycle;
        public float CycleTime => _cycleTime;
        public ResourceType ResourceType => _resourceType;
    }
}
