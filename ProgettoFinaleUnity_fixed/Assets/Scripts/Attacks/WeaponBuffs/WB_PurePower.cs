using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurePower", menuName = "ScriptableObjects/WeaponBuffs/PurePower")]
public class WB_PurePower : WeaponBuff
{
    public float Multiplier = 4f;
    public override void OnGunStart(GenericGun justEquipped)
    {
        hostGun = justEquipped;
        Apply(justEquipped);
    }
    public override void OnGunStop(GenericGun justUnequipped)
    {
        Remove(justUnequipped);
    }
    public override void Apply(GenericGun toBuff)
    {
        
        toBuff.weaponOutputStats.EffectChance -= 300f;
        toBuff.weaponOutputStats.Damage += toBuff.weaponOutputStats.Damage*(Multiplier -1);
        
    }
    public override void Remove(GenericGun toNerf)
    {
        toNerf.weaponOutputStats.EffectChance += 300;
        toNerf.weaponOutputStats.Damage -= toNerf.WeaponBaseStats.Damage * (Multiplier - 1);

    }


}
