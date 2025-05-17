using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour
{
    static public AudioHandler instance;
    public AudioSource generalAudioSource;
    public AudioSource musicAudioSource;

    [SerializeField] List<AudioResource> buttonClicksResources;
    [SerializeField] List<AudioResource> selectSoundResources;
    [SerializeField] AudioResource menuAudioResourceMusic;
    [SerializeField] AudioResource matchingGameAudioResourceMusic;
    [SerializeField] AudioResource sortingGameAudioResourceMusic;
    [SerializeField] AudioResource patternGameAudioResourceMusic;
    [SerializeField] AudioResource confettiAudioResource;
    [SerializeField] AudioResource ladybugMovementAudioResource;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayButtonSound()
    {
        generalAudioSource.pitch = 1f;
        generalAudioSource.volume = 1f;
        generalAudioSource.loop = false;
        generalAudioSource.resource = buttonClicksResources[Random.Range(0, buttonClicksResources.Count)];
        generalAudioSource.Play();
    }

    public void PlaySelectSound()
    {
        generalAudioSource.pitch = 1f;
        generalAudioSource.volume = 1f;
        generalAudioSource.loop = false;
        generalAudioSource.resource = selectSoundResources[Random.Range(0, selectSoundResources.Count)];
        generalAudioSource.Play();
    }

    public void PlayLadybugMovementSound()
    {
        generalAudioSource.pitch = 0.8f;
        generalAudioSource.volume = 0.2f;
        generalAudioSource.loop = true;
        generalAudioSource.resource = ladybugMovementAudioResource;
        generalAudioSource.Play();
    }

    public void StopLadybugMovementSound()
    {
        generalAudioSource.pitch = 1f;
        generalAudioSource.volume = 1f;
        generalAudioSource.loop = false;
        generalAudioSource.Pause();
    }

    public void PlayConfettiSound()
    {
        generalAudioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
        generalAudioSource.volume = 0.5f;
        generalAudioSource.loop = false;
        generalAudioSource.resource = confettiAudioResource;
        generalAudioSource.Play();
    }

    public void PlayMatchingGameMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.resource = matchingGameAudioResourceMusic;
        musicAudioSource.Play();
    }

    public void PlaySortingGameMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.resource = sortingGameAudioResourceMusic;
        musicAudioSource.Play();
    }

    public void PlayPatternGameMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.resource = patternGameAudioResourceMusic;
        musicAudioSource.Play();
    }

    public void PlayMenuMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.resource = menuAudioResourceMusic;
        musicAudioSource.Play();
    }
}
