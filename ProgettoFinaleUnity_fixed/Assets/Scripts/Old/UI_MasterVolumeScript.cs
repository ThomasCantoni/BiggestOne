using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_MasterVolumeScript : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioMixer UI_Mixer;
    public float value;
    private void Start()
    {
        TimeManager.ResumeEvent += ChangeVolume;
    }
    public void ChangeDesiredVolume(float value)
    {
        this.value = Mathf.Log10(value) * 20f;
    }
    public void ChangeVolume()
    {
        
        Mixer.SetFloat("Volume",value);
        UI_Mixer.SetFloat("UI Volume", value);
        
    }
}
