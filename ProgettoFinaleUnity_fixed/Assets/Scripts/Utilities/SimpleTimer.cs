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
    public void StopTimer(bool invoke)
    {
        HasCompleted = true;
        if(invoke)
            TimerCompleteEvent?.Invoke();
        t.Close();
    }
    
    private void stopTimer(object sender,ElapsedEventArgs a)
    {
        //yield return new WaitForSeconds(Time);
        //Debug.Log("Timer Stopped");
        StopTimer(true);
        
    }
}
