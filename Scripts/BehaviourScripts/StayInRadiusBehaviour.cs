using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/StayInRadius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    public Vector2 centre;
    public float radius = 15f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 centreOffset = centre - (Vector2)agent.transform.position;
        float t = centreOffset.magnitude / radius;

        if (t < 0.9f)
        {
            return Vector2.zero;
        }

        return centreOffset * t * t;
    }
}
