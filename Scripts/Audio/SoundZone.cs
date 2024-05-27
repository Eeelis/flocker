using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundZone : MonoBehaviour
{
    [SerializeField] private float volumeMultiplier;

    private AudioSource audioSource;
    private float nAgentsInZone;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        nAgentsInZone += 1;
        audioSource.volume = nAgentsInZone / 100 * volumeMultiplier;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        nAgentsInZone -= 1;
        audioSource.volume = nAgentsInZone / 100 * volumeMultiplier;
    }
}
