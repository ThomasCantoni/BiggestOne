using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class SimpleTimer
{
    public float  TimeMs=1000;
    public bool HasCompleted = false;
    Timer t;
    
    public delegate void TimerEvents();
    public event TimerEvents TimerCompleteEvent,TimerStartEvent;
    public SimpleTimer()
    {

    }
    public SimpleTimer(float millisecond)
    {
        this.TimeMs = millisecond;
    }
    
    public void StartTimer()
    {

        t = new Timer(TimeMs);
        t.Elapsed += stopTimer;
        //Debug.Log("Timer started");
        TimerStartEvent?.Invoke();
        HasCompleted = false;
        t.Start();
    }
    public void ChangeTime(int TimeInMilliseconds)
    {
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
