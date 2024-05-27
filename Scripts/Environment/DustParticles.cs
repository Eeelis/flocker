using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticles : MonoBehaviour
{
    [SerializeField] private float amplitude = 2.0f;
    [SerializeField] private float frequency = 20.0f;
    [SerializeField] private float noiseSpeed = 10.0f;

    private ParticleSystem dustParticles;
    private float noiseOffset;
    private Vector3 initialPosition;

    void Start()
    {
        dustParticles = GetComponent<ParticleSystem>();

        initialPosition = transform.position;

        noiseOffset = Random.Range(0.0f, 1000.0f);
    }

    void Update()
    {
        float time = Time.time;
        float perlinValue = Mathf.PerlinNoise(noiseOffset, time * noiseSpeed);

        // Map the Perlin noise value to a range of -1 to 1
        perlinValue = perlinValue * 2.0f - 1.0f;

        // Calculate the new y position using a sine wave and Perlin noise
        float newY = initialPosition.y + Mathf.Sin(time * frequency) * amplitude * perlinValue;

        // Apply the new position
        var emission = dustParticles.emission;
        emission.rateOverTimeMultiplier = newY * 2;
    }
}
