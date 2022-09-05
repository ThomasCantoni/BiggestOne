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
    public float Damage;
    public HitInfo(GenericGun SourceGun)
    {
        //SourceDamageInstance = SourceGun;
        //PlayerAttackEffects = SourceGun.Player.GetComponent<PlayerAttackEffects>();
        Damage = SourceGun.Damage;
    }
    
    public HitInfo()
    {

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

