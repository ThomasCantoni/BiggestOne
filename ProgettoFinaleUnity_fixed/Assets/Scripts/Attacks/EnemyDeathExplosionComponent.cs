using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathExplosionComponent : MonoBehaviour
{
    public float Radius,Damage;
    public HitInfo hit;
    public LayerMask LayersToHit;
    HealthBarEnemy HBE;
    void Start()
    {
        HBE = GetComponent<HealthBarEnemy>();
        HBE.OnEnemyDeath += OnEnemyDeath;
    }
    private void OnEnemyDeath()
    {
        Collider[] thingsHit = Physics.OverlapSphere(this.transform.position,Radius,LayersToHit.value);
        List<IHittable> thingsActuallyHittable = FilterArray.FilterOverlapArrayIntoList<IHittable,Collider>(thingsHit);
        thingsActuallyHittable.Remove(this.GetComponent<IHittable>());
        hit.Damage = Damage;
        foreach(IHittable x in thingsActuallyHittable)
        {
            
            x.OnHit(hit);
        }
        HBE.OnEnemyDeath -= OnEnemyDeath;
        Destroy(this);
    }

}
