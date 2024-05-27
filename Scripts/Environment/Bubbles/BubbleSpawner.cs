using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private List<Color> bubbleColors = new List<Color>();
    [SerializeField] private Bubble bubblePrefab;
    [SerializeField] private float frequency;
    [SerializeField] private int poolSize;

    private Queue<Bubble> bubblePool = new Queue<Bubble>();
    private List<Bubble> activeBubbles = new List<Bubble>();

    private void Start()
    {
        InitializeBubblePool();
        StartCoroutine(SpawnBubbles());
    }

    private void InitializeBubblePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bubble bubble = Instantiate(bubblePrefab, transform);
            bubble.gameObject.SetActive(false);
            bubblePool.Enqueue(bubble);
        }
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(frequency, frequency * 3));

            // Remove active bubbles that are off-screen
            for (int i = activeBubbles.Count - 1; i >= 0; i--)
            {
                if (activeBubbles[i].transform.position.y > 25)
                {
                    ReturnBubbleToPool(activeBubbles[i]);
                }
            }

            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        Bubble bubble = GetBubbleFromPool();

        if (bubble != null)
        {
            bubble.RandomizeFeatures(true);
            bubble.transform.position = new Vector2(Random.Range(-28, 28), -20);
            bubble.gameObject.SetActive(true);
            activeBubbles.Add(bubble);
        }
    }

    private Bubble GetBubbleFromPool()
    {
        if (bubblePool.Count > 0)
        {
            return bubblePool.Dequeue();
        }

        Bubble newBubble = Instantiate(bubblePrefab);
        newBubble.gameObject.SetActive(false);
        return newBubble;
    }

    private void ReturnBubbleToPool(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
        activeBubbles.Remove(bubble);
        bubblePool.Enqueue(bubble);
    }
}