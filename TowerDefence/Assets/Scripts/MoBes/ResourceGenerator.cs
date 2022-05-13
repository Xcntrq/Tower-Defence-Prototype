using nsBuildingType;
using nsResourceStorage;
using UnityEngine;

namespace nsResourceGenerator
{
    public class ResourceGenerator : MonoBehaviour
    {
        [SerializeField] private BuildingType _buildingType;

        private ResourceStorage _resourceStorage;

        private float _timeLeft;
        private float _timeCost;

        public void Initialize(ResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
        }

        private void Awake()
        {
            _timeCost = _buildingType.ResourceGeneratorData.TimeCost;
            _timeLeft = _timeCost;
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _timeLeft += _timeCost;
                _resourceStorage.AddResource(_buildingType.ResourceGeneratorData.ResourceType, 1);
            }
        }
    }
}
