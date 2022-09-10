using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HitInfo
{
    [Tooltip("The DamageInstance from which this information comes from")]
    public DamageInstance SourceDamageInstance;
    public bool HasHitEnemy
    {
        get { return EnemyHit != null; }
    }
    public bool IsChainableAttack = false;
    public GameObject GameObjectHit;
    public EnemyClass EnemyHit;
    //public PlayerAttackEffects PlayerAttackEffects;
    public Vector3 collisionPoint;
    public Vector3 collisionNormal;
    public DamageStats DamageStats;
    public void SetColliderPositions(Vector3 bulletPosition,Collider colliderHit)
    {
        collisionPoint = colliderHit.ClosestPoint(bulletPosition);
        collisionNormal = (bulletPosition - collisionPoint).normalized;
    }
    public void SetRaycastPositions(RaycastHit information)
    {
        collisionPoint = information.point;
        collisionNormal = information.normal;
    
    }
    public HitInfo(IDamager Source)
    {
        //SourceDamageInstance = SourceGun;
        //PlayerAttackEffects = SourceGun.Player.GetComponent<PlayerAttackEffects>();
        DamageStats = Source.DamageStats;
    }
    public HitInfo(IDamager Source,IHittable Target)
    {
        //SourceDamageInstance = SourceGun;
        //PlayerAttackEffects = SourceGun.Player.GetComponent<PlayerAttackEffects>();
        DamageStats = Source.DamageStats;
        GameObjectHit = Target.Mono.gameObject;
        EnemyHit = GameObjectHit.GetComponent<EnemyClass>();
    }
   
    public HitInfo()
    {

    }
}
[Serializable]
public struct  DamageStats
{
    public float Damage;
    public float CritChance;
    public float CritMultiplier;
    public float EffectChance;
    public static DamageStats operator +(DamageStats current ,DamageStats toAdd)
    {
        DamageStats result=current;
        result.Damage += toAdd.Damage;
        result.CritChance += toAdd.CritChance;
        result.CritMultiplier += toAdd.CritMultiplier;
        result.EffectChance += toAdd.EffectChance;
            return result;
    }
        

}
public interface IDamager
{
    public abstract DamageStats DamageStats
    {
        get;
        set;
    }
}
public interface IHittable
{
    public abstract MonoBehaviour Mono
    {
        get;
    }
    public abstract void OnHit(HitInfo info);

}
public interface IStackableChainAttack
{
    public abstract void Stack(MonoBehaviour toStack);
}

