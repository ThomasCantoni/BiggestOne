using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WB_Penetration", menuName = "ScriptableObjects/WeaponBuffs/WB_Penetration")]

public class WB_Penetration : WeaponBuff
{
    public int maxPenetrations;
    public float damageMultiplierSubsequentPenetration;
    
    public override void OnGunStart(GenericGun justEquipped)
    {
        justEquipped.BulletCreated += ApplyPenetration;
        justEquipped.BulletHitListPopulated += ApplyDamage;
    }
    public override void OnGunStop(GenericGun justUnequipped)
    {
        justUnequipped.BulletCreated -= ApplyPenetration;
        justUnequipped.BulletHitListPopulated -= ApplyDamage;
    }
    
    public void ApplyPenetration(GenericBullet created)
    {
        created.maxPenetrations += maxPenetrations;
        
    }
   
    public void ApplyDamage(GenericBullet populated)
    {
        for (int i = 0; i < populated.Hits.Count; i++)
        {
            populated.Hits[i].DamageStats.Damage *=  Mathf.Pow( damageMultiplierSubsequentPenetration, i);
           
        }
    }

}
