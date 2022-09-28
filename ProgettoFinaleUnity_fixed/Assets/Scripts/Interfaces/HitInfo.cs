using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HitInfo
{
    //[Tooltip("The DamageInstance from which this information comes from")]
    public DamageInstance SourceDamageInstance;
    public bool HasHitEnemy
    {
        get { return EnemyHit != null; }
    }
    
    public bool IsChainableAttack
    {
        get
        {
            // if the damageSource is the player itself, then it's a chainableattack
            return SourceDamageInstance.DamageSource.Mono is FirstPersonController;
        }
    }

    public GameObject GameObjectHit;
    public EnemyClass EnemyHit;
    //public PlayerAttackEffects PlayerAttackEffects;
    
    public DamageStats DamageStats;
    public FractureInfo FractureInfo;
    public void SetColliderPositions(Vector3 bulletPosition, Collider colliderHit)
    {
        FractureInfo.collisionPoint = colliderHit.ClosestPoint(bulletPosition);
        FractureInfo.collisionNormal = (bulletPosition - FractureInfo.collisionPoint).normalized;
    }
    public void SetRaycastPositions(RaycastHit information)
    {
        FractureInfo.collisionPoint = information.point;
        FractureInfo.collisionNormal = information.normal;

    }
    public HitInfo(IDamager Source)
    {
        //SourceDamageInstance = SourceGun;
        //PlayerAttackEffects = SourceGun.Player.GetComponent<PlayerAttackEffects>();
        DamageStats = Source.DamageStats;
       
    }
    public HitInfo(IDamager Source, IHittable Target)
    {
        //SourceDamageInstance = SourceGun;
        //PlayerAttackEffects = SourceGun.Player.GetComponent<PlayerAttackEffects>();
        DamageStats = Source.DamageStats;
        GameObjectHit = Target.Mono.gameObject;
        //EnemyHit = new List<EnemyClass>();
        EnemyHit = GameObjectHit.GetComponent<EnemyClass>();
    }

    public HitInfo()
    {

    }
}
[Serializable]
public struct DamageStats
{
    public float Damage;
    public float CritChance;
    public float CritMultiplier;
    public float EffectChance;
    
    public static DamageStats operator +(DamageStats current, DamageStats toAdd)
    {
        DamageStats result = current;
        result.Damage += toAdd.Damage;
        result.CritChance += toAdd.CritChance;
        result.CritMultiplier += toAdd.CritMultiplier;
        result.EffectChance += toAdd.EffectChance;
        
        return result;
    }
    public static DamageStats operator -(DamageStats current, DamageStats toAdd)
    {
        DamageStats result = current;
        result.Damage -= toAdd.Damage;
        result.CritChance -= toAdd.CritChance;
        result.CritMultiplier -= toAdd.CritMultiplier;
        result.EffectChance -= toAdd.EffectChance;

        return result;
    }


}
