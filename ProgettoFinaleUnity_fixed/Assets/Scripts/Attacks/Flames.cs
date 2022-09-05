using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Flames", menuName = "ScriptableObjects/Flames")]
public class Flames : ChainableAttack
{
    
    public float FlamesDamageFrequency;
    public float FlamesInitialDamage,FlamesBurnDamage,FlamesBurnDuration;
    public override void Apply(GameObject recepient)
    {
        FlamesEnemyComponent fec = recepient.AddComponent<FlamesEnemyComponent>();
        if (fec == null)
            return;
        fec.flamesHitInfo = new HitInfo();
        fec.flamesHitInfo.GameObjectHit = recepient;
        fec.flamesHitInfo.IsChainableAttack = true;
        
        fec.Duration = FlamesBurnDuration;
        fec.InitialDamage = FlamesInitialDamage;
        fec.Frequency = FlamesDamageFrequency;
        fec.BurnDamage = FlamesBurnDamage;
    }
}
