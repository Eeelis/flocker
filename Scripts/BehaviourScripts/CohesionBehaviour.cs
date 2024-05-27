using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/CohesionBehaviour")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 cohesionMove = Vector2.zero;

        if (context.Count == 0)
        {
            return cohesionMove;
        }

        List<Transform> filteredContext = (contextFilter == null) ? context : contextFilter.Filter(agent, context);

        foreach(Transform item in filteredContext)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;

        cohesionMove -= (Vector2)agent.transform.position;

        return cohesionMove;
    }
}
