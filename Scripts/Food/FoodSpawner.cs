using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;

    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private float minDistanceFromAgents = 5.0f;
    [SerializeField] private float minDistanceFromFood = 10.0f;
    [SerializeField] private float frequency;
    [SerializeField] private int poolSize;
    [SerializeField] private int maxFood;
   
    private List<GameObject> activeFood = new List<GameObject>();
    private Queue<GameObject> foodPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        InitializeFoodPool();
        StartCoroutine(SpawnFoods());
    }

    private void InitializeFoodPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject food = Instantiate(foodPrefab, transform);
            food.SetActive(false);
            foodPool.Enqueue(food);
        }
    }

    private IEnumerator SpawnFoods()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(frequency, frequency * 2));

            if (activeFood.Count >= maxFood) { continue; }

            SpawnFood();
        }
    }

    public void ReturnFoodToPool(GameObject food)
    {
        food.SetActive(false);
        activeFood.Remove(food);
        foodPool.Enqueue(food);
    }

    private void SpawnFood()
    {
        if (foodPool.Count == 0)
        {
            GameObject newFood = Instantiate(foodPrefab, transform);
            newFood.SetActive(false);
            foodPool.Enqueue(newFood);
        }

        GameObject food = foodPool.Dequeue();

        Vector2 spawnPosition;
        bool positionFound = false;

        // Try to find a valid spawn position
        do
        {
            spawnPosition = new Vector2(Random.Range(-28, 28), Random.Range(-10, 20));

            // Use a CircleCast to check for food and agents within the minimum distance
            positionFound = !Physics2D.CircleCast(spawnPosition, minDistanceFromAgents, Vector2.zero, 0, LayerMask.GetMask("Agent")) &&
                            !Physics2D.CircleCast(spawnPosition, minDistanceFromFood, Vector2.zero, 0, LayerMask.GetMask("Food"));
        } while (!positionFound);

        food.transform.position = spawnPosition;
        food.transform.localScale = Vector2.zero;
        food.SetActive(true);
        activeFood.Add(food);
    }
}