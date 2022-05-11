using UnityEngine;

namespace nsResourceType
{
    [CreateAssetMenu(menuName = "ScObs/ResourceType")]
    public class ResourceType : ScriptableObject
    {
        [SerializeField] private string _name;
    }
}
