using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DamageInstance
{
    public IDamager DamageSource;
    public List<GenericBullet> BulletsHit;
    public GameObject InstanceOwner;
    public PlayerAttackEffects PlayerAttackEffects;
    
    private List<HitInfo> hits;

    
    public List<HitInfo> Hits
    {
        get { return hits; }
        set
        {
            if(value != null && value.Count >0)
            {
                hits = value;
                appendList(enemiesHit, FilterDistinct(hits));

            }


        }
    }
    private List<EnemyClass> enemiesHit;
    public List<EnemyClass> EnemiesHit
    {
        get { return enemiesHit; }
        private set
        {
            enemiesHit = value;
        }
    }


    public DamageInstance()
    {

        hits = new List<HitInfo>();
        enemiesHit = new List<EnemyClass>();
    }
    public DamageInstance(IDamager source)
    {
        DamageSource = source;
        InstanceOwner = source.Mono.gameObject;
        hits = new List<HitInfo>();
        enemiesHit = new List<EnemyClass>();
    }
    private void appendList(List<EnemyClass> target, List<EnemyClass> toAppend)
    {
        foreach (EnemyClass x in toAppend)
        {
            if (!target.Contains(x))
            {
                target.Add(x);
            }
        }
    }
    public void AddEnemy(EnemyClass toAdd)
    {
        if (!enemiesHit.Contains(toAdd))
        {
            enemiesHit.Add(toAdd);

        }

    }
    public List<HitInfo> AddHitInfo(HitInfo newToAdd)
    {
        if (hits != null)
        {
            hits.Add(newToAdd);
            
                if (newToAdd.HasHitEnemy && !EnemiesHit.Contains(newToAdd.EnemyHit))
                    EnemiesHit.Add(newToAdd.EnemyHit);
            
            return hits;
        }
        return null;
    }
    public List<EnemyClass> FilterDistinct(List<HitInfo> toFilter)
    {

        List<EnemyClass> toReturn = new List<EnemyClass>(toFilter.Count);
        foreach (HitInfo collided in toFilter)
        {
            EnemyClass maybe = collided.GameObjectHit.GetComponent<EnemyClass>();
            if (maybe != null && !toReturn.Contains(maybe))
            {
                toReturn.Add(maybe);
            }
        }
        toReturn.TrimExcess();
        return toReturn;
    }
    public void Deploy()
    {
        //apply every HitInfo
        for (int i = 0; i < Hits.Count; i++)
        {
            if (Hits[i] == null)
                continue;
            //applying the Player's weapon buffs before applying the damage
            Hits[i].SourceDamageInstance = this;
            //for (LinkedListNode<WeaponBuff> x = PlayerAttackEffects.WeaponBuffs.First;
            //    x != null;
            //    x = x.Next)
            //{
            //    x.Value.Apply(ref Hits[i].DamageStats);

            //}
            IHittable hitAnything = Hits[i]?.GameObjectHit.GetComponent<IHittable>();
            // apply the damage to whatever was hit 
            hitAnything.OnHit(Hits[i]);
            // attempt to cast whatever was hit to enemy
            try
            {
                //if succesfull, apply every PerShotEffect for every hit (x times for 1 hit)
                EnemyClass hitEnemy = (EnemyClass)hitAnything.Mono;
                if (PlayerAttackEffects.PerShotAttacks != null)
                {
                    foreach (ChainableAttack x in PlayerAttackEffects.PerShotAttacks)
                    {
                        x.Apply(hitEnemy);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Cast Failed! => " + e.Message);
                continue;
            }
        }
        // then apply every PerEnemyAttack to each enemy (x times for 1 enemy)
        if (PlayerAttackEffects.PerEnemyAttacks != null)
        {
            for (int i = 0; i < EnemiesHit.Count; i++)
            {
                foreach (ChainableAttack x in PlayerAttackEffects.PerEnemyAttacks)
                {
                    if (EnemiesHit[i] != null)
                        x.Apply(EnemiesHit[i]);
                }
            }
        }

    }
}

