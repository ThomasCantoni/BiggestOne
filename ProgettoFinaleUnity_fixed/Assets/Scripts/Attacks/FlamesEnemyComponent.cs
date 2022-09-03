using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamesEnemyComponent : MonoBehaviour
{
    public float InitialDamage, BurnDamage, Frequency, DurationMs;
    public HitInfo flamesHit;
    private Repeater Repeater;
    private SimpleTimer Timer;
    private IHittable host;

    private void Start()
    {

        Timer = new SimpleTimer(DurationMs);
        Repeater = new Repeater();
        Repeater.Frequency = Frequency;
        //Timer.TimerCompleteEvent += () => Debug.LogError("FLAME TIMER ENDED");
        Timer.TimerCompleteEvent += () => StopEffect();

        Repeater.RepeaterTickEvent += ApplyDamage;
        

        //Repeater.RepeaterTickEvent += () => Debug.Log("FLAMES REPEATER TICKING");
        host = GetComponent<IHittable>();


        //apply initial damage

        flamesHit.Damage = InitialDamage;
        host.OnHit(flamesHit);
        // set burn damage
        flamesHit.Damage = BurnDamage;
        //Timer.TimerStartEvent += () => Debug.Log("FLAMES TIMER STARTED");
        
        Timer.StartTimer();
        Repeater.StartRepeater();
    }
    private void ApplyDamage()
    {
        Debug.Log("Applying Damage");
        host.OnHit(flamesHit);
    }
    private void StopEffect()
    {
        //Timer.StopTimer(false);
        Repeater.StopRepeater();
        Debug.Log("FLAMES EFFECT STOPPED");
        
        Destroy(this.gameObject);
    }

}
