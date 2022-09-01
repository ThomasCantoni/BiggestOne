using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplodeOnDeath", menuName = "ScriptableObjects/ExplodeOnDeath")]
public class EnemyExplosiveDeath : ChainableAttack
{
    public float Radius, Damage;
    public LayerMask LayersToHit;
    public override void Apply(IHittableInformation info)
    {
        EnemyDeathExplosionComponent EDEC = info.EnemyHit.AddComponent<EnemyDeathExplosionComponent>();
        EDEC.LayersToHit = LayersToHit;
        EDEC.Radius = Radius;
        EDEC.Damage = Damage;
        EDEC.hit = info;
    }
}
