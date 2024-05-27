using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/FlockBehaviourComposite")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] behaviours;
    public float[] weights;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 move = Vector2.zero;

        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Weights and Behaviours must be of equal length", this);
            return move;
        }

        for(int i = 0; i < behaviours.Length; i++)
        {
            Vector2 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
