using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

[Serializable]
public class Repeater
{
    
    public float Frequency
    {
        get { return (float)1000f / IntervalMilliseconds; }
        set
        {
            IntervalMilliseconds = 1000f / value;
            ChangeTime((int)IntervalMilliseconds);
        }
    }
    public float IntervalMilliseconds=1000;
    public bool IsActive;

    private Timer t;
    private Timer tResume;
    public delegate void RepeaterEvents();
    public delegate void RepeaterProgressEvent(float progress);

    public event RepeaterEvents RepeaterStartEvent, RepeaterTickEvent, RepeaterStopEvent, RepeaterPauseEvent,RepeaterResumeEvent;
    public event RepeaterProgressEvent RepeaterProgress;
    private float activationTime;
    public Repeater() 
    {

    }
    public Repeater(float Frequency)
    {
        this.IntervalMilliseconds = 1000/ IntervalMilliseconds;

    }
    public Repeater(int IntervalMilliseconds)
    {
        this.IntervalMilliseconds = IntervalMilliseconds ;
        
    }
    
    public void StartRepeater()
    {
        activationTime = Time.realtimeSinceStartup;
        t = new Timer(Tick,null,0,(int)IntervalMilliseconds);
        IsActive = true;
        //t.Elapsed += new ElapsedEventHandler(Tick);
        //t.AutoReset = true;
        //t.Enabled = true;
       
        
        RepeaterStartEvent?.Invoke();
        
    }
    private void Tick(object state)
    {
        try
        {
            ExampleMainThreadCall();
        }
        catch (Exception)
        {
            //Debug.LogError(exc.Message);
            t.Dispose();
        }

    }
    
    public IEnumerator ThisWillBeExecutedOnTheMainThread()
    {
        //Debug.Log("This is executed from the main thread");
        if(IsActive)
        {
            RepeaterTickEvent?.Invoke();
            RepeaterProgress?.Invoke(activationTime);
        }
        
        yield return null;
    }

    public void ExampleMainThreadCall()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread());
    }
    public void ChangeTime(int TimeInMilliseconds)
    {
        if(t != null)
            t.Change(0,TimeInMilliseconds);

    }
    public void PauseRepeater()
    {
        t.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(-1));
        IsActive = false;
    }
    public void StopRepeater(int delayResumeMilliseconds)
    {
        IsActive = false;
        t.Dispose();
        tResume = new Timer(ResumeRepeater, null, delayResumeMilliseconds, TimeSpan.FromMilliseconds(-1).Milliseconds);
        //Debug.Log("Repeater paused with  " +tResume.Interval + "  ms of delay");
    }
    private void ResumeRepeater(object o)
    {
        try
        {
            t = new Timer(Tick, null, 0, (int)IntervalMilliseconds);
            IsActive = true;
            //t.Change(0,(int) IntervalMilliseconds);
            //StartRepeater();

           
            tResume.Dispose();
            RepeaterResumeEvent?.Invoke();
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
   
    }
    
    public void StopRepeater()
    {
       
        RepeaterStopEvent?.Invoke();
        IsActive = false;
        //t.AutoReset = false;
        t.Dispose();
         
    }
    
}
