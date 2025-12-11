using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class AudioKey
{
    [SerializeField] public string Name;
    [SerializeField] public AudioResource Audio;
    [SerializeField] public bool isLooped;
}

public class AudioStorage : MonoBehaviour
{
    public AudioKey[] audioClips;
    [HideInInspector] public Dictionary<string, AudioSource> audioSources = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (AudioKey audioAsset in audioClips)
        {
            print(audioAsset);
            print(audioAsset.Name);
            audioSources[audioAsset.Name] = gameObject.AddComponent<AudioSource>();
            audioSources[audioAsset.Name].playOnAwake = false;
            audioSources[audioAsset.Name].loop = audioAsset.isLooped;
            audioSources[audioAsset.Name].resource = audioAsset.Audio;
        }
    }
}
