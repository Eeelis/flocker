using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flock : MonoBehaviour
{
    [SerializeField] private FlockAgent flockAgentPrefab;
    [SerializeField] private FlockBehaviour flockBehaviour;

    [Range(10, 500)]
    [SerializeField] private int startingSize = 250;

    [Range(1f, 100f)]
    [SerializeField] private float driveFactor = 10f;

    [Range(1f, 100f)]
    [SerializeField] private float maxSpeed = 5f;

    [Range(1f, 10f)]
    [SerializeField] private float neighbourRadius = 1.5f;

    [Range(0f, 1f)]
    [SerializeField] private float avoidanceMultiplier = 0.5f;

    [SerializeField] private bool playRandomPianoNotes;
    [SerializeField] private int randomPianoNoteChance;

    private List<FlockAgent> agents = new List<FlockAgent>();
    private AudioSource audioSource;
    private const float agentDensity = 0.08f;
    private float squareMaxSpeed;
    private float squareNeighbourRadius;
    private float squareAvoidanceRadius;

    [HideInInspector]
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; }}

    private void Start()
    {
        squareAvoidanceRadius = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceMultiplier * avoidanceMultiplier;

        for (int i = 0; i < startingSize; i++)
        {
            FlockAgent newAgent = Instantiate(
                flockAgentPrefab,
                Random.insideUnitCircle * startingSize * agentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360)),
                transform
            );

            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }

        if (playRandomPianoNotes)
        {
            InvokeRepeating("PlayRandomPianoNotes", 2.5f, 2.5f);
        }
    }

    private void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyTransforms(agent);
            Vector2 movement = flockBehaviour.CalculateMove(agent, context, this);
            movement *= driveFactor;

            if (movement.sqrMagnitude > squareMaxSpeed)
            {
                movement = movement.normalized * maxSpeed;
            }

            agent.Move(movement);
        }
    }

    List<Transform> GetNearbyTransforms(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);

        foreach(Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }

        return context;
    }

    void PlayRandomPianoNotes()
	{
        int x = Random.Range(0,randomPianoNoteChance);
		
		if( x == 0)
        {
            List<FlockAgent> agentsOnScreen = agents.Where(agent => IsOnScreen(agent.gameObject)).ToList();
            int randomIndex = Random.Range(0, agentsOnScreen.Count);
            agentsOnScreen[randomIndex].PlayOneShotEffect();

            // Convert agent x position to a value between -1 and 1
            float halfScreenWidth = Screen.width / 2f;
            float distanceFromCenter = Camera.main.WorldToScreenPoint(agentsOnScreen[randomIndex].transform.position).x - halfScreenWidth;
            float panning = distanceFromCenter / halfScreenWidth;

            AudioManager.Instance.PlayRandomPiano(1f, panning);
        }
	}

    bool IsOnScreen(GameObject agent)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(agent.transform.position);
        bool isOnScreen = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        return isOnScreen;
    }
}

