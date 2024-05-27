using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/PhysicsLayerFilter")]
public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask layerMask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();

        foreach(Transform item in original)
        {
            if (layerMask == (layerMask | (1 << item.gameObject.layer)))
            {
                filtered.Add(item);
            }
        }
        
        return filtered;
    }
}
