using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Flames", menuName = "ScriptableObjects/Flames")]
public class Flames : ChainableAttack
{
    
    public float FlamesDamageFrequency;
    public float FlamesInitialDamage,FlamesBurnDamage,FlamesBurnDurationMs;
    public override void Apply(IHittableInformation info)
    {
        
       FlamesEnemyComponent fec = info.EnemyHit.AddComponent<FlamesEnemyComponent>();
        fec.flamesHit = new IHittableInformation();
        fec.flamesHit.EnemyHit = info.EnemyHit;
        fec.flamesHit.sender = info.sender;
        fec.DurationMs = FlamesBurnDurationMs;
        fec.InitialDamage = FlamesInitialDamage;
        fec.Frequency = FlamesDamageFrequency;
        fec.BurnDamage = FlamesBurnDamage;
    }
}
