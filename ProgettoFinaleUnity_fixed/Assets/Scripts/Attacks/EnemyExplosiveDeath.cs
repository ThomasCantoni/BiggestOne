using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeOnDeath", menuName = "ScriptableObjects/ExplodeOnDeath")]
public class EnemyExplosiveDeath : ChainableAttack
{
    public float Radius, Damage;
    public LayerMask LayersToHit;
    public override void Apply(HitInfo info)
    {
        if (((1<<info.GameObjectHit.layer) & LayersToHit) == 0) //if the thing i hit is LayersToHit
            return;
        EnemyDeathExplosionComponent EDEC = info.GameObjectHit.AddComponent<EnemyDeathExplosionComponent>();
        EDEC.LayersToHit = LayersToHit;
        EDEC.Radius = Radius;
        EDEC.Damage = Damage;
        EDEC.hit = info;
    }
}
