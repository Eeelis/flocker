using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/AvoidanceBehaviour")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 avoidanceMove = Vector2.zero;

        if (context.Count == 0)
        {
            return avoidanceMove;
        }

        int nAvoid = 0;

        List<Transform> filteredContext = (contextFilter == null) ? context : contextFilter.Filter(agent, context);

        foreach(Transform item in filteredContext)
        {
            Vector2 closestPoint = item.gameObject.GetComponent<Collider2D>().ClosestPoint(agent.transform.position);

            if (Vector2.SqrMagnitude(closestPoint - (Vector2)agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid ++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
            
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }

        return avoidanceMove;
    }
}
