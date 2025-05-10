using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour
{

    static public AudioHandler instance;

    [SerializeField] List<AudioResource> buttonClicksResources;

    [SerializeField] List<AudioResource> selectSoundResources;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonSound()
    {
        audioSource.resource = buttonClicksResources[Random.Range(0, buttonClicksResources.Count)];
        audioSource.Play();
    }

    public void PlaySelectSound()
    {
        audioSource.resource = selectSoundResources[Random.Range(0, selectSoundResources.Count)];
        audioSource.Play();
    }
}
