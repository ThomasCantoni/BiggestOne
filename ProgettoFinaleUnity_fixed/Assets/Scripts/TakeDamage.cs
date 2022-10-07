using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public enum collisionType { head,body}
    public collisionType damageType;
    public EnemyClass EC;
    public float multiplier = 2f;

    public void HIT(HitInfo info)
    {
        info.DamageStats.Damage *= multiplier;
        EC.DetuctHealth(info);
    }
}
