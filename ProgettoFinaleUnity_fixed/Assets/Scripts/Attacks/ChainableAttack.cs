using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChainAttackApplicationType
{
    PerEnemy,PerShot,WeaponBuff
}
public abstract class ChainableAttack : ScriptableObject
{
    public ChainAttackApplicationType ChainAttackApplicationMode;
    [HideInInspector]
    public List<GameObject> EnemiesHit;
    [SerializeField]
    public DamageStats DamageStats;
    public abstract void Apply(EnemyClass recepient);
    
}

