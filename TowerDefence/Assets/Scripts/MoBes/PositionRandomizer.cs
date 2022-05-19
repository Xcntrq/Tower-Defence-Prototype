using nsIntValue;
using UnityEngine;

namespace nsPositionRandomizer
{
    public class PositionRandomizer : MonoBehaviour
    {
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private IntValue _seed;

        private void Awake()
        {
            var random = _seed.Value == 0 ? new System.Random() : new System.Random(_seed.Value);

            int xOffsetInt = random.Next(_minValue, _maxValue + 1);
            int yOffsetInt = random.Next(_minValue, _maxValue + 1);

            float xOffsetFloat = (float)xOffsetInt / 100;
            float yOffsetFloat = (float)yOffsetInt / 100;

            transform.Translate(xOffsetFloat, yOffsetFloat, 0);
        }
    }
}
