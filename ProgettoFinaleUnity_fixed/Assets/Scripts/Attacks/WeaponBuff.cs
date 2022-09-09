using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBuffGeneric", menuName = "ScriptableObjects/WeaponBuffGeneric")]

public class WeaponBuff : ScriptableObject
{
    public DamageStats StatsToAdd;
    

    
    public virtual void Apply(ref DamageStats toBuff)
    {
        toBuff += StatsToAdd;
    }
}
