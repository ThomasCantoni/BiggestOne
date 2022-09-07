using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GenericGun Source;
    public static List<EnemyClass> AlreadyHit = new List<EnemyClass>();
    public float Speed=1f;
    Rigidbody RB;
    private void OnTriggerEnter(Collider other)
    {
        EnemyClass tryget = other.gameObject.GetComponent<EnemyClass>();
        if (tryget != null)
        {

            HitInfo info = new HitInfo(other, this.transform);
            info.DamageStats = Source.DamageStats;

            DamageInstance DI = new DamageInstance(Source);
            DI.PlayerAttackEffects = Source.PlayerAttackEffects;
            DI.AddHitInfo(info);
            DI.Deploy();
        }

        Destroy(this.gameObject);
    }
    
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 10f);
    }

    public void FixedUpdate()
    {
        RB.MovePosition(this.transform.position + transform.forward * Speed*Time.fixedDeltaTime);
        
    }

}
