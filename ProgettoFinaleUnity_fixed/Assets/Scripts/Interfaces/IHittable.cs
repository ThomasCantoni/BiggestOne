using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HitInfo
{
    [Tooltip("The GameObject from which this information comes from. \r If not set it's automatically set to the owner of component.")]
    public DamageInstance SourceDamageInstance;
    public bool IsEnemy;
   // public GameObject sender;
    public GameObject GameObjectHit;
    public float Damage;
    public Vector3 collisionPoint;
    public Vector3 collisionNormal;

    public PlayerAttackEffects PlayerAttackEffects;
    public HitInfo(DamageInstance Source)
    {
        SourceDamageInstance = Source;
        PlayerAttackEffects = Source.SourceGun.Player.GetComponent<PlayerAttackEffects>();
        Damage = Source.SourceGun.Damage;
    }
    public HitInfo()
    {

    }
}
public interface IHittable
{
    public abstract void OnHit(HitInfo info);

}

