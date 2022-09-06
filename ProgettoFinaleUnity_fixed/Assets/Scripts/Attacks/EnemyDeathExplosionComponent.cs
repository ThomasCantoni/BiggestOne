using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyDeathExplosionComponent : MonoBehaviour
{
    public float Radius,Damage;
   
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

        
        HitInfo hit = new HitInfo();
        hit.IsChainableAttack = true;
        hit.Damage = Damage;
        for (int i = 0; i < thingsActuallyHittable.Count; i++)
        {
            IHittable x = thingsActuallyHittable[i];
            if(!thingsHitByExplosions.Contains(x))
            {
                thingsHitByExplosions.Add(x);
                x.OnHit(hit);
            }
            else
            {
                continue;
            }

        }
        thingsHitByExplosions.Clear();    
        
        EC.OnEnemyDeath -= OnEnemyDeath;
        Destroy(this);
    }

}
