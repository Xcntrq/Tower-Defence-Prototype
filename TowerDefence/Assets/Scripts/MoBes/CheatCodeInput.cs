using nsResourceStorage;
using nsResourceType;
using nsResourceTypes;
using UnityEngine;

namespace nsCheatCodeInput
{
    public class CheatCodeInput : MonoBehaviour
    {
        [SerializeField] private ResourceStorage _resourceStorage;
        [SerializeField] private ResourceTypes _resourceTypes;
        [SerializeField] private string _moneyCode;

        private string _currentInput;

        private void Awake()
        {
            _currentInput = string.Empty;
        }

        private void Update()
        {
            foreach (char c in Input.inputString)
            {
                if ((c == '\n') || (c == '\r')) continue;
                if (_currentInput.Length > 4) _currentInput = _currentInput.Remove(0, 1);
                _currentInput = string.Concat(_currentInput, c);
            }
            if (_currentInput == _moneyCode)
            {
                foreach (ResourceType resourceType in _resourceTypes.List)
                {
                    _resourceStorage.AddResource(resourceType, 50000);
                }
                _currentInput = string.Empty;
            }
        }
    }
}
