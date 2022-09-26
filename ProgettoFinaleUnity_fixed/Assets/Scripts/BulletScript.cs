using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour,IDamager
{
    public GenericGun Source;
    public static List<EnemyClass> AlreadyHit = new List<EnemyClass>();
    public float Speed=1f;
    Rigidbody RB;

    public DamageStats DamageStats
    { 
        get { return Source.DamageStats;    } 
        set { Source.DamageStats = value;   } 
    }

    private void OnTriggerEnter(Collider other)
    {
        IHittable tryget = other.gameObject.GetComponent<IHittable>();
        if (tryget != null)
        {
            HitInfo info = new HitInfo(this,tryget);
            
            DamageInstance DI = new DamageInstance(this);
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
        RB.MovePosition(this.transform.position + transform.forward * Speed * Time.fixedDeltaTime);
        
    }

}
