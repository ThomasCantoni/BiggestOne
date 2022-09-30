using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Timers;
using System;


[Serializable]
public class SimpleTimer
{
    public float  TimeMs=1000;
    public bool HasCompleted = false;

    public float Progress
    {
        get { return estimatedCompletionTime / (Time.time*1000); }
    }
    public float RemainingTimeMs
    {
        get { return estimatedCompletionTime - (Time.time * 1000); }
    }
    public bool IsActive
    {
        get 
        {
            if (t == null)
                return false;
            
            return t.Enabled; 
        }
        set
        {
            if(t!= null)
                t.Enabled = value;
        }
    }

    Timer t;
    private float activationTime, estimatedCompletionTime;
    

    public delegate void TimerEvents();
    public event TimerEvents TimerCompleteEvent,TimerStartEvent;


    public SimpleTimer()
    {

    }
    public SimpleTimer(float millisecond)
    {
        this.TimeMs = millisecond;
    }

    [ButtonDrawer("UpdateTimer",ButtonWidth = 150)]
    public bool UpdateTimer;
    public void updateTimer()
    {
        // Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        StartTimer();
    }
    public void StartTimer()
    {
        t?.Dispose();
        activationTime = Time.time*1000;
        estimatedCompletionTime = activationTime + TimeMs;
        t = new Timer(TimeMs);
        t.Elapsed += stopTimer;
        //Debug.Log("Timer started");
        TimerStartEvent?.Invoke();
        HasCompleted = false;
        
        t.Start();
    }
    public void ChangeTime(int TimeInMilliseconds)
    {
        if(t != null)
            t.Interval = TimeInMilliseconds;
    }
    public void StopTimer(bool invoke = true)
    {
        try
        {
            HasCompleted = true;
            if (invoke)
                ExampleMainThreadCall();
            t?.Close();
        }
        catch (System.Exception a)
        {
            Debug.LogError("ERRORE PORCO" + a.Message);
            t.Close();
        }
        
    }
    public void StopTimer()
    {
        HasCompleted = true;

        ExampleMainThreadCall();
        t?.Close();
    }

    private void stopTimer(object sender,ElapsedEventArgs a)
    {
        //yield return new WaitForSeconds(Time);
        //Debug.Log("Timer Stopped");
        StopTimer(true);
        
    }
    public IEnumerator ThisWillBeExecutedOnTheMainThread()
    {
        //Debug.Log("This is executed from the main thread");
        TimerCompleteEvent?.Invoke();
        yield return null;
    }

    public void ExampleMainThreadCall()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(ThisWillBeExecutedOnTheMainThread());
    }
}
