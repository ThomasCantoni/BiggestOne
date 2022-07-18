using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_MasterVolumeScript : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioMixer UI_Mixer;
    public void ChangeVolume(float value)
    {
        value = Mathf.Log10(value) * 20f;
        Mixer.SetFloat("Volume",value);
        UI_Mixer.SetFloat("UI Volume", value);
        
    }
}
