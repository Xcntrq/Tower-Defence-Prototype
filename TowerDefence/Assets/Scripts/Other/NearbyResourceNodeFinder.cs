using nsResourceNode;
using nsResourceType;
using UnityEngine;

namespace nsNearbyResourceNodeFinder
{
    public class NearbyResourceNodeFinder
    {
        public int OverlapCircleAll(Vector2 point, float radius, ResourceType resourceType)
        {
            int nearbyResourceNodeCount = 0;
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(point, radius);
            foreach (Collider2D nearbyCollider in nearbyColliders)
            {
                ResourceNode resourceNode = nearbyCollider.GetComponent<ResourceNode>();
                if (resourceNode != null)
                {
                    if (resourceNode.ResourceType == resourceType) nearbyResourceNodeCount++;
                }
            }
            return nearbyResourceNodeCount;
        }
    }
}
