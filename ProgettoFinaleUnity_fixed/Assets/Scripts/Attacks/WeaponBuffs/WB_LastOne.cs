using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LastOne", menuName = "ScriptableObjects/WeaponBuffs/LastOne")]
public class WB_LastOne : WeaponBuff
{
    public float DamageMultiplier;
    private float toAdd;
    public override void OnGunStart(GenericGun justEquipped)
    {
        if(justEquipped.WeaponType == WeaponType.Shotgun)
        {
            Apply(justEquipped);

        }
    }
    public override void OnGunStop(GenericGun justUnequipped)
    {
        if (justUnequipped.WeaponType == WeaponType.Shotgun)
        {
            Remove(justUnequipped);

        }
    }
    public override void Apply(GenericGun toBuff)
    {
        toBuff.BeforeShoot += BigShot;
        toBuff.AfterShoot += ResetBigShot;
    }
    public override void Remove(GenericGun toNerf)
    {
        toNerf.BeforeShoot -= BigShot;
        toNerf.AfterShoot -= ResetBigShot;

    }
    public void BigShot(GenericGun toBuff)
    {
        if(toBuff.currentAmmo == 1)
        {
            toAdd = toBuff.DamageContainer.Damage * (DamageMultiplier - 1);
            toBuff.DamageContainer.Damage += toAdd;
        }
    }
    public void ResetBigShot(GenericGun toNerf)
    {
        toNerf.DamageContainer.Damage -= toAdd;
        toAdd = 0;
    }
}
