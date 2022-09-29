using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour
{
    FMOD.Studio.Bus bus;
    [SerializeField][Range(-80f,10)]
    private float Volume;
    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
    }
    public float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10f, dB / 20f);
        return linear;
    }
    // Update is called once per frame
    void Update()
    {
        bus.setVolume(DecibelToLinear(Volume));
    }
}
