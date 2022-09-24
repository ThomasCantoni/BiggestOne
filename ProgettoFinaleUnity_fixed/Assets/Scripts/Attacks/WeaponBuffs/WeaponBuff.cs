using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBuffGeneric", menuName = "ScriptableObjects/WeaponBuffGeneric")]

public class WeaponBuff : ScriptableObject
{
    public DamageStats StatsToAdd;
    

    public virtual void OnGunStart(GenericGun justEquipped)
    {
       
        Apply(ref justEquipped.DamageContainer);
    }
    public virtual void OnGunStop(GenericGun justUnequipped)
    {
        Remove(ref justUnequipped.DamageContainer);
    }
    public virtual void Apply(ref DamageStats toBuff)
    {
        toBuff += StatsToAdd;
        
    }
    public virtual void Remove(ref DamageStats toNerf)
    {
        toNerf -= StatsToAdd;
    }
    public virtual void Apply(GenericGun toBuff)
    {
        
    }

    public virtual void Apply(DamageInstance toBuff)
    {

    }
}
