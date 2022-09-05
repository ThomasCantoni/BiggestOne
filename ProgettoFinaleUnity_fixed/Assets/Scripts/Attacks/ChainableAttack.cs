using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChainAttackApplicationType
{
    PerEnemy,PerShot
}
public abstract class ChainableAttack : ScriptableObject
{
    public ChainAttackApplicationType ChainAttackApplicationMode;
    [HideInInspector]
    public List<GameObject> EnemiesHit;
    public abstract void Apply(EnemyClass recepient);
    
}
public interface IStackableChainAttack
{
    public abstract void Stack(MonoBehaviour toStack);
}
