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
        justEquipped.BeforeShoot += Apply;
    }
    public override void OnGunStop(GenericGun justUnequipped)
    {
        justUnequipped.BeforeShoot -= Apply;
    }
    public override void Apply(GenericGun toBuff)
    {
        
        toBuff.DamageContainer.EffectChance += 100f;
    }
    public override void Remove(GenericGun toNerf)
    {
        toNerf.DamageContainer.EffectChance -= 100;
    }
}
