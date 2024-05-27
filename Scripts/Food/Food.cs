using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private ParticleSystem foodEatenParticleSystem;
    [SerializeField] private float moveSpeed = 2.0f;       
    [SerializeField] private float areaSize = 5.0f;        
    [SerializeField] private float perlinScale = 1.0f;     
    [SerializeField] private Vector2 targetSize = new Vector2(0.2f, 0.2f);

    private Vector2 startPosition;       
    private float timeOffsetX;           
    private float timeOffsetY; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<FlockAgent>(out FlockAgent agent))
        {
            // Convert food x position to a value between -1 and 1
            float halfScreenWidth = Screen.width / 2f;
            float distanceFromCenter = Camera.main.WorldToScreenPoint(transform.position).x - halfScreenWidth;
            float panning = distanceFromCenter / halfScreenWidth;

            AudioManager.Instance.PlayRandomPluck(0f, panning);
            Instantiate(foodEatenParticleSystem, transform.position, Quaternion.identity);
            FoodSpawner.Instance.ReturnFoodToPool(gameObject);
        }
    }

    void OnEnable()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector2.zero;
        LeanTween.scale(gameObject, targetSize, 1.2f).setEaseOutBounce();

        startPosition = transform.position;
        timeOffsetX = Random.Range(0f, 100f);
        timeOffsetY = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Calculate Perlin noise values for smooth random movement
        float noiseX = Mathf.PerlinNoise(Time.time * perlinScale + timeOffsetX, 0.0f);
        float noiseY = Mathf.PerlinNoise(Time.time * perlinScale + timeOffsetY, 0.0f);

        // Map the noise values to the desired area size
        float posX = Mathf.Lerp(startPosition.x - areaSize, startPosition.x + areaSize, noiseX);
        float posY = Mathf.Lerp(startPosition.y - areaSize, startPosition.y + areaSize, noiseY);

        // Move around
        Vector2 targetPosition = new Vector2(posX, posY);
        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
