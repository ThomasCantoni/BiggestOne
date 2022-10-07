using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public enum collisionType { head,body}
    public collisionType damageType;
    public EnemyClass EC;

    public void HIT(HitInfo info)
    {
        EC.DetuctHealth(info);
    }
}
