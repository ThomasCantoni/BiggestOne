using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Repeater
{
    public float InitialDelay;
    public float Frequency=0;
    public bool IsActive = false;
    private Timer t;
    
    public delegate void RepeaterEvents();
    public event RepeaterEvents RepeaterStartEvent, RepeaterTickEvent, RepeaterStopEvent;
    public Repeater() 
    {

    }
    public Repeater(float FrequencySeconds,float initialDelaySeconds)
    {
        this.Frequency = FrequencySeconds ;
        InitialDelay = initialDelaySeconds;

    }
    
    public void StartRepeater()
    {
        Debug.Log(Frequency);
        t = new Timer(Frequency);
        t.Elapsed += Tick;
       
        t.Start();
       // Debug.Log("Repeater started");
        IsActive = true;
        RepeaterStartEvent?.Invoke();
    }
    private void Tick(object sender,ElapsedEventArgs e)
    {
       Debug.Log("Repeater ticking");
        RepeaterTickEvent?.Invoke();
        t.Start();

    }
    public void ChangeTime(int TimeInMilliseconds)
    {
        t.Interval = TimeInMilliseconds;

    }
    public void StopRepeater()
    {
       
        RepeaterStopEvent?.Invoke();
        IsActive = false;
        t.AutoReset = false;
        t.Dispose();
        
    }
    
}
