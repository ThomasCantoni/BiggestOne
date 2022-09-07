using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainableAttackComponent : MonoBehaviour, IDamager
{
    private DamageStatContainer damageStats;
    public DamageStatContainer DamageStats 
    {
        get { return damageStats;  }
        set { damageStats = value; }
    }
}
