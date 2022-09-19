using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
public interface IKillable : IHittable
{
    public abstract void OnDeath();
    public delegate void OnDeathEvent();
    public OnDeathEvent deathEvent { get; set; }
}
public interface IStackableChainAttack
{
    public abstract void Stack(MonoBehaviour toStack);
}

