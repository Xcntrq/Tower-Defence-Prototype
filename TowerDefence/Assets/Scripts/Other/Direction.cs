using UnityEngine;

namespace nsDirection
{
    public class Direction
    {
        private readonly System.Random _random;

        public Direction()
        {
            _random = new System.Random();
        }

        public Vector3 GetRandom()
        {
            float x = (float)_random.NextDouble() - 0.5f;
            float y = (float)_random.NextDouble() - 0.5f;
            return new Vector3(x, y, 0).normalized;
        }
    }
}
