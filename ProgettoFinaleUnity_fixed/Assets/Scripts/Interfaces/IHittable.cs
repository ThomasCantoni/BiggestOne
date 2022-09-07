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
        get { return GameObjectHit.layer == 7; }
    }
    public bool IsChainableAttack = false;
    public GameObject GameObjectHit;
    //public PlayerAttackEffects PlayerAttackEffects;
    public Vector3 collisionPoint;
    public Vector3 collisionNormal;
    public DamageStatContainer DamageStats;
    
    public HitInfo(IDamager Source)
    {
        //SourceDamageInstance = SourceGun;
        //PlayerAttackEffects = SourceGun.Player.GetComponent<PlayerAttackEffects>();
        DamageStats = Source.DamageStats;
    }
    public HitInfo(Collider collider,Transform source)
    {
        collisionPoint = collider.transform.position;
        collisionNormal = (collisionPoint - source.position).normalized;
        GameObjectHit = collider.gameObject;
    }
    public HitInfo()
    {

    }
}
[Serializable]
public struct  DamageStatContainer
{
    public float Damage;
    public float CritChance;
    public float CritMultiplier;
    public float EffectChance;
}
public interface IDamager
{
    public abstract DamageStatContainer DamageStats
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

