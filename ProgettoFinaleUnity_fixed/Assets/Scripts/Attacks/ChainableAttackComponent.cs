using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainableAttackComponent : MonoBehaviour, IDamager
{
    private DamageStats damageStats;
    public DamageStats DamageStats 
    {
        get { return damageStats;  }
        set { damageStats = value; }
    }
}
