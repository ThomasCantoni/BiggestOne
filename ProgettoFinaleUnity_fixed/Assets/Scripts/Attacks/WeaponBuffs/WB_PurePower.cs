using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurePower", menuName = "ScriptableObjects/WeaponBuffs/PurePower")]
public class WB_PurePower : WeaponBuff
{
    
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

        toBuff.WeaponBaseStats.EffectChance -= 300f;
        toBuff.WeaponBaseStats.Damage *= 4;
    }
    public override void Remove(GenericGun toNerf)
    {
        toNerf.WeaponBaseStats.EffectChance += 300;
        toNerf.WeaponBaseStats.Damage *= 0.25f;

    }


}
