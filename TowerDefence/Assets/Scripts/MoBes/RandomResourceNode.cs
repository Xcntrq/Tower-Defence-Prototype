using nsIntValue;
using nsResourceNodeList;
using UnityEngine;

namespace nsRandomResourceNode
{
    public class RandomResourceNode : MonoBehaviour
    {
        [SerializeField] private ResourceNodeList _resourceNodeList;
        [SerializeField] private IntValue _seed;

        private void Awake()
        {
            var random = _seed.Value == 0 ? new System.Random() : new System.Random(_seed.Value);
            int i = random.Next(_resourceNodeList.Items.Count);
            Instantiate(_resourceNodeList.Items[i], transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }
    }
}
