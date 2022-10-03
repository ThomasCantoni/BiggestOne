using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FlamesEnemyComponent : ChainableAttackComponent
{
    public float InitialDamage, BurnDamage, Frequency, Duration;
    public HitInfo flamesHitInfo;
    private Repeater Repeater;
    private IHittable host;

    private void Start()
    {

       
        Repeater = new Repeater();
        Repeater.Frequency = Frequency;
       

        Repeater.RepeaterTickEvent += ApplyDamage;
        

        
        host = GetComponent<IHittable>();


        DamageInstance dmgInstance = new DamageInstance(this);

        flamesHitInfo = new HitInfo(this, host);
        //apply initial damage
        flamesHitInfo.DamageStats = BaseStats;
        flamesHitInfo.DamageStats.Damage = InitialDamage;
        dmgInstance.AddHitInfo(flamesHitInfo);
        dmgInstance.Deploy();
        //host.OnHit(flamesHitInfo);
        // set burn damage
        flamesHitInfo.DamageStats.Damage = BurnDamage;
        //Timer.TimerStartEvent += () => Debug.Log("FLAMES TIMER STARTED");

       
        Repeater.StartRepeater();
        Destroy(this, Duration);
    }
    private void ApplyDamage()
    {       
            Debug.Log("Applying Damage");
        DamageInstance dmgInstance = new DamageInstance(this);
        flamesHitInfo.DamageStats.Damage = BurnDamage;

        dmgInstance.AddHitInfo(flamesHitInfo);
        dmgInstance.Deploy();
        host.OnHit(flamesHitInfo); 
    }
    private void OnDestroy()
    {
        StopEffect();
    }
    public void StopEffect()
    {
        Repeater.StopRepeater();
        Debug.Log("FLAMES EFFECT STOPPED");
        
    }
}
