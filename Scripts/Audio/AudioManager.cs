using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<AudioClip> pianoSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> pluckSounds = new List<AudioClip>();
    [SerializeField] private AudioSource pianoAudioSource;
    [SerializeField] private AudioSource pluckAudioSource;

    private int lastRandomPianoIndex;
    private int lastRandomPluckIndex;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayRandomPiano(float delay = 0f, float panning = 0f)
    {
        int randomSoundIndex = Random.Range(0, pianoSounds.Count);
            
        // Make it less likely to repeat the same sample
        if (randomSoundIndex == lastRandomPianoIndex)
        {
            randomSoundIndex = Random.Range(0, pianoSounds.Count);
        }
        
        lastRandomPianoIndex = randomSoundIndex;

        pianoAudioSource.panStereo = panning;

        StartCoroutine(PlayClipWithDelay(pianoAudioSource, pianoSounds[randomSoundIndex], delay));
    }

    public void PlayRandomPluck(float delay = 0f, float panning = 0f)
    {
        int randomSoundIndex = Random.Range(0, pluckSounds.Count);
            
        // Make it less likely to repeat the same sample
        if (randomSoundIndex == lastRandomPluckIndex)
        {
            randomSoundIndex = Random.Range(0, pluckSounds.Count);
        }

        lastRandomPluckIndex = randomSoundIndex;

        pluckAudioSource.panStereo = panning;

        StartCoroutine(PlayClipWithDelay(pluckAudioSource, pluckSounds[randomSoundIndex], delay));
    }

    IEnumerator PlayClipWithDelay(AudioSource source, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip);
    }
}
