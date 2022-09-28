using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyDeathExplosionComponent : ChainableAttackComponent
{
    public float Radius;
    
    
    public LayerMask LayersToHit;
    EnemyClass EC;
    static List<IHittable> thingsHitByExplosions = new List<IHittable>();
    void Start()
    {
        EC = GetComponent<EnemyClass>();
        EC.OnEnemyDeath += OnEnemyDeath;
     
    }
    private void OnEnemyDeath()
    {
        Collider[] thingsHit = Physics.OverlapSphere(this.transform.position,Radius,LayersToHit.value);
        List<IHittable> thingsActuallyHittable = FilterArray.FilterOverlapArrayIntoList<IHittable,Collider>(thingsHit);
        thingsActuallyHittable.Remove(this.GetComponent<IHittable>());

        DamageInstance newDamage = new DamageInstance(this);
        for (int i = 0; i < thingsActuallyHittable.Count; i++)
        {
                IHittable x = thingsActuallyHittable[i];
            if(!thingsHitByExplosions.Contains(x))
            {
                HitInfo hit = new HitInfo(this,x);
                newDamage.AddHitInfo(hit);
                thingsHitByExplosions.Add(x);
            }
            else
            {
                continue;
            }

        }
        thingsHitByExplosions.Clear();
        newDamage.Deploy();
        EC.OnEnemyDeath -= OnEnemyDeath;
        Destroy(this);
    }

}
