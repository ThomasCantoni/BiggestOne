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
        if (tryget == null)
            return;
        
        HitInfo info = new HitInfo(other,this.transform);
        info.Damage = Source.Damage;
        DamageInstance DI = new DamageInstance(Source);
        DI.AddHitInfo(info);
        DI.Deploy();
        Destroy(this.gameObject);
    }
    
    private void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        RB.MovePosition(this.transform.position + transform.forward * Speed*Time.fixedDeltaTime);
    }

}
