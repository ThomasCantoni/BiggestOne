using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WB_Penetration", menuName = "ScriptableObjects/WB_Penetration")]

public class WB_Penetration : WeaponBuff
{
    public int maxPenetrations;
    public float damageMultiplierSubsequentPenetration;
    
    public override void OnGunStart(GenericGun justEquipped)
    {
        justEquipped.HitscanBulletCreated += ApplyPenetration;
        justEquipped.HitscanBulletPopulated += ApplyDamage;
    }
    public override void OnGunStop(GenericGun justUnequipped)
    {
        justUnequipped.HitscanBulletCreated -= ApplyPenetration;
        justUnequipped.HitscanBulletPopulated -= ApplyDamage;
    }
    
    public void ApplyPenetration(HitscanBullet created)
    {
        created.maxPenetrations += maxPenetrations;
        
    }
   
    public void ApplyDamage(HitscanBullet populated)
    {
        for (int i = 0; i < populated.Hits.Count; i++)
        {
            populated.Hits[i].DamageStats.Damage *=  Mathf.Pow( damageMultiplierSubsequentPenetration, i);
           
        }
    }

}
