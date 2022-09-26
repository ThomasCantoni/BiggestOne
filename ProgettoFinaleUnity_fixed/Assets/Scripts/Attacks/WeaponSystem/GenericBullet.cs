using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBullet
{
    public GenericGun Owner;
    //public List<EnemyClass> EnemiesHit;
    public List<HitInfo> Hits;
    public int maxPenetrations;
    public virtual bool HasHitSomething { get; }
    public  GenericBullet(GenericGun owner)
    {
        Owner = owner;

    }
    public virtual void Deploy()
    {
        
    }
    
}
