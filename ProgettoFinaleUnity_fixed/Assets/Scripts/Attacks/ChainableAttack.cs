using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChainAttackApplicationType
{
    PerEnemy,PerShot,WeaponBuff
}
public abstract class ChainableAttack : ScriptableObject,IDamager
{
    [HideInInspector]
    public FirstPersonController FPS;
    public ChainAttackApplicationType ChainAttackApplicationMode;
    [HideInInspector]
    public List<GameObject> EnemiesHit;
    [SerializeField]
    public DamageStats damageValues;
    public DamageStats BaseStats
    {
        get { return damageValues; }
        set { damageValues = value; }
    }
    public DamageStats OutputStats { get { return damageValues; } set { damageValues = value; } }

    public MonoBehaviour Mono
    {
        get { return FPS; }
    }

    public abstract void Apply(EnemyClass recepient);
    
}

