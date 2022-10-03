using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainableAttackComponent : MonoBehaviour, IDamager
{
    private DamageStats damageStats, outputStats;
    public DamageStats BaseStats 
    {
        get { return damageStats;  }
        set { damageStats = value; }
    }
    public DamageStats OutputStats
    {
        get { return damageStats + outputStats; }
        set { outputStats = value; }
    }
    public MonoBehaviour Mono
    {
        get { return this; }
    }
}
