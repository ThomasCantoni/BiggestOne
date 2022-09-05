using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeOnDeath", menuName = "ScriptableObjects/ExplodeOnDeath")]

public class EnemyExplosiveDeath : ChainableAttack
{
    public float Radius, Damage;
    public LayerMask LayersToHit;
    public override void Apply(GameObject recepient)
    {
        if (((1<<recepient.layer) & LayersToHit) == 0) //if the thing i hit is LayersToHit
            return;
        EnemyDeathExplosionComponent EDEC = recepient.AddComponent<EnemyDeathExplosionComponent>();
        if (EDEC == null)
            return;
        EDEC.LayersToHit = LayersToHit;
        EDEC.Radius = Radius;
        EDEC.Damage = Damage;
       
    }
   
}
