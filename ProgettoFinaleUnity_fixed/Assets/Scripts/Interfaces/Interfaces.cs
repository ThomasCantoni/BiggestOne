using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public interface IDamager :IMono
{
    public abstract DamageStats BaseStats
    {
        get;
        set;
    }
    public abstract DamageStats OutputStats
    {
        get;
        set;
    }
}
public interface IMono
{
    //i need Mono to have access to the GameObject hit through the interface
    public abstract MonoBehaviour Mono
    {
        get;
    }
}
public interface IHittable:IMono
{
    
    public abstract void OnHit(HitInfo info);

}
public interface IInteractable
{
    public UnityEvent InteractUnityEvent { get; }
    public string InteractMessage { get; set; }
    public abstract void OnInteract();
}
public interface IKillable : IHittable
{
    public abstract void OnDeath();
    public delegate void OnDeathEvent();
    public delegate void OnDeathEventParameter(IKillable self);

    public OnDeathEvent deathEvent { get; set; }
}
public interface IStackableChainAttack
{
    public abstract void Stack(MonoBehaviour toStack);
}

