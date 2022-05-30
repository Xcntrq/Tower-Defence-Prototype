using nsResourceGeneratorData;
using nsResourceGeneratorDatas;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nsTimeTicker
{
    public class TimeTicker : MonoBehaviour
    {
        [SerializeField] private ResourceGeneratorDatas _resourceGeneratorDatas;
        [SerializeField] private float _secondsInTick;

        private Dictionary<ResourceGeneratorData, int> _tickLimits;
        private Dictionary<ResourceGeneratorData, int> _currentTicks;
        private float _timeLeft;

        public event Action<ResourceGeneratorData, int> OnTick;

        private void Awake()
        {
            _timeLeft = _secondsInTick;
            _tickLimits = new Dictionary<ResourceGeneratorData, int>();
            _currentTicks = new Dictionary<ResourceGeneratorData, int>();
            foreach (ResourceGeneratorData resourceGeneratorData in _resourceGeneratorDatas.List)
            {
                _tickLimits[resourceGeneratorData] = resourceGeneratorData.TicksInCycle;
                _currentTicks[resourceGeneratorData] = 0;
            }
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _timeLeft += _secondsInTick;
                foreach (ResourceGeneratorData resourceGeneratorData in _resourceGeneratorDatas.List)
                {
                    _currentTicks[resourceGeneratorData] += 1;
                    _currentTicks[resourceGeneratorData] %= _tickLimits[resourceGeneratorData];
                    OnTick?.Invoke(resourceGeneratorData, _currentTicks[resourceGeneratorData]);
                }
            }
        }
    }
}
