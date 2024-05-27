using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; }}

    [SerializeField] private ParticleSystem oneShotParticleSystem;
    private Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; }}

    private void Awake()
    {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 movement)
    {
        transform.up = movement;
        transform.position += (Vector3)movement * Time.deltaTime;

    }

    public void PlayOneShotEffect()
    {
        oneShotParticleSystem.Stop();
        oneShotParticleSystem.Play();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

}
