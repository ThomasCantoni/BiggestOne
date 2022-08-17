using System.Collections;
using System.Collections.Generic;

using System.Threading;

public class SimpleTimer
{
    public int TimeInMilliseconds=1000;
    public bool HasCompleted = false;
    Timer t;
    TimerCallback tc;
    public delegate void TimerEvents();
    public event TimerEvents TimerCompleteEvent,TimerStartEvent;
    public SimpleTimer()
    {

    }
    public SimpleTimer(float seconds)
    {
        this.TimeInMilliseconds = (int)seconds*1000;
    }
    public SimpleTimer(int TimeInMilliseconds)
    {
        this.TimeInMilliseconds = TimeInMilliseconds;
    }
    public void StartTimer()
    {
        
        t = new Timer(StopTimer, null,TimeInMilliseconds, TimeInMilliseconds);
        
        TimerStartEvent?.Invoke();
        HasCompleted = false;
    }
    public void ChangeTime(int TimeInMilliseconds)
    {
        t.Change(TimeInMilliseconds, TimeInMilliseconds);
    }
    private void StopTimer(object data)
    {
        //yield return new WaitForSeconds(Time);
        TimerCompleteEvent?.Invoke();
        HasCompleted = true;
        t.Dispose();
    }
}
