using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FourLeafClover", menuName = "ScriptableObjects/WeaponBuffs/FourLeafClover")]
public class WB_FourLeafClover : WeaponBuff
{
    float originalEffectChance;
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
        originalEffectChance = toBuff.WeaponBaseStats.EffectChance;
        toBuff.weaponOutputStats.EffectChance = 100f;
    }
    public override void Remove(GenericGun toNerf)
    {
        toNerf.weaponOutputStats.EffectChance = originalEffectChance;
    }
}
