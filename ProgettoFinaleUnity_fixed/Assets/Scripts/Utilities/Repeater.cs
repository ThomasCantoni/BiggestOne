using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Repeater
{
    public int InitialDelayMilliseconds;
    public int FrequencyMilliseconds = 1000;
    public bool IsActive = false;
    Timer t;
    
    public delegate void RepeaterEvents();
    public event RepeaterEvents RepeaterStartEvent, RepeaterTickEvent, RepeaterStopEvent;
    public Repeater() 
    {

    }
    public Repeater(float FrequencySeconds,float initialDelaySeconds)
    {
        this.FrequencyMilliseconds = (int)FrequencySeconds * 1000;
        InitialDelayMilliseconds = (int)initialDelaySeconds*1000;

    }
    public Repeater(int FrequencyMilliseconds,int initialDelayMilliseconds)
    {
        this.FrequencyMilliseconds = FrequencyMilliseconds;
        InitialDelayMilliseconds = initialDelayMilliseconds;

    }
    public void StartRepeater()
    {
        t = new Timer(tick, null, InitialDelayMilliseconds, FrequencyMilliseconds);
        Debug.Log("Repeater started");
        RepeaterStartEvent?.Invoke();
        IsActive = true;
    }
    private void tick(object data)
    {
        RepeaterTickEvent?.Invoke();
        Debug.Log("Repeater ticking");

    }
    public void ChangeTime(int InitialDelayMs,int TimeInMilliseconds)
    {
        t.Change(InitialDelayMs, TimeInMilliseconds);
    }
    public void StopRepeater(object data)
    {
       
        RepeaterStopEvent?.Invoke();
        IsActive = false;
        t.Dispose();
        
    }
    
}
