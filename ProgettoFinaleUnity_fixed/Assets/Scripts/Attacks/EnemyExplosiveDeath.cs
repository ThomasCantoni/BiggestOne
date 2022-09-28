using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeOnDeath", menuName = "ScriptableObjects/ExplodeOnDeath")]

public class EnemyExplosiveDeath : ChainableAttack
{
    public float Radius;
    public LayerMask LayersToHit;
    public override void Apply(EnemyClass recepient)
    {
        if (  ((  1<<recepient.gameObject.layer  ) & LayersToHit) == 0  ) //if the thing i hit is LayersToHit
            return;
        EnemyDeathExplosionComponent EDEC = recepient.gameObject.AddComponent<EnemyDeathExplosionComponent>();
        if (EDEC == null)
            return;
        EDEC.LayersToHit = LayersToHit;
        EDEC.Radius = Radius;
        EDEC.DamageStats = damageValues;
       
    }
   
}
