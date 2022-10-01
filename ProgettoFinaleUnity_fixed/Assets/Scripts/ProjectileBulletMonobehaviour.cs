using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBulletMonobehaviour : MonoBehaviour,IDamager
{
    public ProjectileBullet ProjectileBullet;
    public GenericGun Source;
    public static List<GameObject> AlreadyHit = new List<GameObject>();
    public float Speed=1f;
    protected Rigidbody RB;
    public int timesHit;
    public float destroyTime=10f;
    public DamageStats DamageStats
    { 
        get { return Source.DamageStats;    } 
        set { Source.DamageStats = value;   } 
    }
    public MonoBehaviour Mono
    {
        get
        {
            return this;
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(!AlreadyHit.Contains(other.gameObject))
        {
            AlreadyHit.Add(other.gameObject);
        }
            timesHit++;
        IHittable tryget = other.gameObject.GetComponent<IHittable>();
        if (tryget != null)
        {
            HitInfo info = new HitInfo(this,tryget);
            
            DamageInstance DI = new DamageInstance(this);
            DI.PlayerAttackEffects = Source.PlayerAttackEffects;
            DI.AddHitInfo(info);
            DI.Deploy();
        }
        if (AlreadyHit.Count>=  ProjectileBullet.maxPenetrations+1 )
            Destroy(this.gameObject);
    }

    public virtual void Start()
    {
        
        Destroy(this.gameObject, destroyTime);
    }

    public virtual void FixedUpdate()
    {
        RB.MovePosition(this.transform.position + transform.forward * Speed *Time.fixedDeltaTime);
        
    }
    
}
