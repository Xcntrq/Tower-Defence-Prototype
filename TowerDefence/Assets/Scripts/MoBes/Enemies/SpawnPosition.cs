using UnityEngine;

namespace nsSpawnPosition
{
    public class SpawnPosition : MonoBehaviour
    {
        [SerializeField] private bool _startActive;

        private void Awake()
        {
            gameObject.SetActive(_startActive);
        }
    }
}
