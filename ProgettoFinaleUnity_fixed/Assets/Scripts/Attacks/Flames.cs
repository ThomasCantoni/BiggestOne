using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Flames", menuName = "ScriptableObjects/Flames")]
public class Flames : ChainableAttack
{
    
    public float FlamesDamageFrequency;
    public float FlamesInitialDamage,FlamesBurnDamage,FlamesBurnDurationMs;
    public override void Apply(HitInfo info)
    {
        
       FlamesEnemyComponent fec = info.GameObjectHit.AddComponent<FlamesEnemyComponent>();
        fec.flamesHit = new HitInfo();
        fec.flamesHit.GameObjectHit = info.GameObjectHit;
        //fec.flamesHit.sender = info.sender;
        fec.DurationMs = FlamesBurnDurationMs;
        fec.InitialDamage = FlamesInitialDamage;
        fec.Frequency = FlamesDamageFrequency;
        fec.BurnDamage = FlamesBurnDamage;
    }
}
