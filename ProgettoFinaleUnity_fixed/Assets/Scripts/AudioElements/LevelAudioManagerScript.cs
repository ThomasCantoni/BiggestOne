using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LevelAudioManagerScript : MonoBehaviour
{
    public AudioClip LevelMusic;
    public AudioSource Source;
    AudioMixer Mixer;
    // Start is called before the first frame update
    void Start()
    {

        Mixer = Source.outputAudioMixerGroup.audioMixer;
        //TimeManager.BulletTimeActivatedEvent += StopSources;
        //TimeManager.BulletTimeDeactivatedEvent += StartSource;
        TimeManager.PauseEvent += StopSources;
        TimeManager.ResumeEvent += StartSource;
        Source.clip = LevelMusic;
        StartSource();
    }

    public void StartSource()
    {
        if (Source.clip != null)
            Source.Play();
    }
    public void StopSources()
    {
        Source.Pause();

    }


}
