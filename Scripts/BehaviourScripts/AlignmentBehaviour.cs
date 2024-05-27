using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/AlignmentBehaviour")]
public class AlignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 alignmentMove = Vector2.zero;

        List<Transform> filteredContext = (contextFilter == null) ? context : contextFilter.Filter(agent, context);

        foreach(Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }

        alignmentMove /= context.Count;

        return alignmentMove;
    }
}
