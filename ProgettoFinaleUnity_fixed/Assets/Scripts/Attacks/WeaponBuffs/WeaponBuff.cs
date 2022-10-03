using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBuffGeneric", menuName = "ScriptableObjects/WeaponBuffs/WeaponBuffGeneric")]

public class WeaponBuff : ScriptableObject
{
    public DamageStats StatsToAdd;
    public GenericGun hostGun;

    public virtual void OnGunStart(GenericGun justEquipped)
    {
        hostGun = justEquipped;
        Apply(ref justEquipped.WeaponBaseStats);
    }
    public virtual void OnGunStop(GenericGun justUnequipped)
    {
        
        Remove(ref justUnequipped.WeaponBaseStats);
    }
    public virtual void Apply(ref DamageStats toBuff)
    {
        toBuff += StatsToAdd;
        
    }
    public virtual void Remove(ref DamageStats toNerf)
    {
        toNerf -= StatsToAdd;
    }
    public virtual void Remove(GenericGun toNerf)
    {
        
    }
    public virtual void Apply(GenericGun toBuff)
    {
        
    }

    public virtual void Apply(DamageInstance toBuff)
    {

    }
}
