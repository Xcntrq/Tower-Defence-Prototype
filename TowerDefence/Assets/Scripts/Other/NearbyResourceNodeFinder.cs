using nsResourceNode;
using nsResourceType;
using nsSpriteParent;
using System.Collections.Generic;
using UnityEngine;

namespace nsNearbyResourceNodeFinder
{
    public class NearbyResourceNodeFinder
    {
        public HashSet<SpriteParent> OverlapCircleAll(Vector2 point, float radius, ResourceType resourceType)
        {
            var desiredNearbyResourceNodes = new HashSet<SpriteParent>();
            Collider2D[] allNearbyColliders = Physics2D.OverlapCircleAll(point, radius);
            foreach (Collider2D nearbyCollider in allNearbyColliders)
            {
                ResourceNode resourceNode = nearbyCollider.GetComponent<ResourceNode>();
                if (resourceNode != null)
                {
                    if (resourceNode.ResourceType == resourceType) desiredNearbyResourceNodes.Add(resourceNode);
                }
            }
            return desiredNearbyResourceNodes;
        }
    }
}
