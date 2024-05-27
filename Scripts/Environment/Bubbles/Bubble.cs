using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private float minSpeed = 0.6f;
    [SerializeField] private float maxSpeed = 4f;
    [SerializeField] private float minSize = 0.5f;
    [SerializeField] private float maxSize = 4f;
    [SerializeField] private float minSizeAmplitude = 0.1f;
    [SerializeField] private float maxSizeAmplitude = 0.25f;
    [SerializeField] private float minSizeFrequency = 2f;
    [SerializeField] private float maxSizeFrequency = 3.5f;
    [SerializeField] private float minOscillationAmplitude = 0.5f;
    [SerializeField] private float maxOscillationAmplitude = 2f;
    [SerializeField] private float minOscillationFrequency = 1f;
    [SerializeField] private float maxOscillationFrequency = 1.5f;
    
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 startScale;
    private float riseSpeed = 2f;
    private float oscillationAmplitude = 0.5f;
    private float oscillationFrequency = 1f;
    private float sizeWobbleAmplitude = 0.1f;
    private float sizeWobbleFrequency = 2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    private void Update()
    {
        float time = Time.time;

        // Move upwards
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);

        // Apply horizontal oscillation
        float oscillation = Mathf.Sin(time * oscillationFrequency) * oscillationAmplitude;
        transform.position = new Vector3(startPosition.x + oscillation, transform.position.y, transform.position.z);

        // Apply size wobble
        float sizeWobbleX = Mathf.Sin(time * sizeWobbleFrequency) * sizeWobbleAmplitude;
        float sizeWobbleY = Mathf.Sin(time * sizeWobbleFrequency) * sizeWobbleAmplitude;
        float deformX = startScale.x + sizeWobbleX;
        float deformY = startScale.y - sizeWobbleY / 2;

        transform.localScale = new Vector2(deformX, deformY);
    }
    
    public void RandomizeFeatures(bool randomizeColor)
    {
        riseSpeed = Random.Range(minSpeed, maxSpeed);
        sizeWobbleAmplitude = Random.Range(minSizeAmplitude, maxSizeAmplitude);
        sizeWobbleFrequency = Random.Range(minSizeFrequency, maxSizeFrequency);
        oscillationAmplitude = Random.Range(minOscillationAmplitude, maxOscillationAmplitude);
        oscillationFrequency = Random.Range(minOscillationFrequency, maxOscillationFrequency);

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector2(randomSize, randomSize);

        if (randomizeColor && colors.Count > 0)
        {
            spriteRenderer.color = colors[Random.Range(0, colors.Count)];
        }
    }
}
