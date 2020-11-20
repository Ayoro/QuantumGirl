using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public AudioSource efxSource;
    public static AudioManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }


    public void AudioPlay(AudioClip clip,float pitch)
    {
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();
    }
}
