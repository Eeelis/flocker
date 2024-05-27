using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/MoveTowardsTarget")]
public class MoveTowardsTargetBehaviour : FilteredFlockBehaviour
{
    [SerializeField] private float radius = 15f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 targetMove = Vector2.zero;

        if (context.Count == 0)
        {
            return targetMove;
        }

        int nTarget = 0;

        List<Transform> filteredContext = (contextFilter == null) ? context : contextFilter.Filter(agent, context);

        foreach(Transform item in filteredContext)
        {
            if (Vector2.Distance(agent.transform.position, item.transform.position) < radius)
            {
                Vector2 targetDirection = item.transform.position - agent.transform.position;
        
                // Normalize the direction to get a unit vector, which indicates direction without scaling by distance
                targetDirection.Normalize();
                
                return targetDirection;
            }
            
        }

        if (nTarget > 0)
        {
            targetMove /= nTarget;
        }

        return targetMove;
    }
}
