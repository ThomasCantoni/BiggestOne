using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class Repeater
{
    public float InitialDelayMilliseconds;
    public float Frequency
    {
        get { return 1000f / IntervalMilliseconds; }
        set
        {
            IntervalMilliseconds = 1000f / value;
        }
    }
    public float IntervalMilliseconds=1000;
    public bool IsActive
    {
        get { return t.Enabled; }
        set { t.Enabled = value; }
    }

    private Timer t;
    private Timer tResume;
    public delegate void RepeaterEvents();
    public event RepeaterEvents RepeaterStartEvent, RepeaterTickEvent, RepeaterStopEvent;

    public Repeater() 
    {

    }
    public Repeater(float IntervalMilliseconds,float initialDelayMilliseconds)
    {
        this.IntervalMilliseconds = IntervalMilliseconds ;
        InitialDelayMilliseconds = initialDelayMilliseconds;
    }
    
    public void StartRepeater()
    {
        t = new Timer(IntervalMilliseconds);
        t.Elapsed += new ElapsedEventHandler(Tick);
        
        t.Enabled = true;
       
        
        RepeaterStartEvent?.Invoke();
        
    }
    private void Tick(object sender,ElapsedEventArgs e)
    {
        try
        {
            ExampleMainThreadCall();
        }
        catch (Exception exc)
        {
            Debug.LogError(exc.Message);
            t.Close();
        }

    }
    
    public IEnumerator ThisWillBeExecutedOnTheMainThread()
    {
        //Debug.Log("This is executed from the main thread");
        RepeaterTickEvent.Invoke();
        yield return null;
    }

    public void ExampleMainThreadCall()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread());
    }
    public void ChangeTime(int TimeInMilliseconds)
    {
        t.Interval = TimeInMilliseconds;

    }
    public void PauseRepeater()
    {
        t.Enabled = false;
        
    }
    public void PauseRepeater(float delayResumeMilliseconds)
    {
        t.Enabled=false;
        tResume = new Timer(delayResumeMilliseconds);
        tResume.AutoReset = false;
        tResume.Elapsed+= (object o, ElapsedEventArgs e) => UnityMainThreadDispatcher.Instance().Enqueue(ResumeRepeater());
        tResume.Start();
        
        Debug.Log("Repeater paused with  " +tResume.Interval + "  ms of delay");
    }
    public IEnumerator ResumeRepeater()
    {
        try
        {
            t.Enabled = true;

           
            tResume.Dispose();
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
        yield return null;
    }
    
    public void StopRepeater()
    {
       
        RepeaterStopEvent?.Invoke();
        IsActive = false;
        t.AutoReset = false;
        t.Dispose();
        
    }
    
}
