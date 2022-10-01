using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerBullet : ProjectileBulletMonobehaviour
{
   
    
    public override void Start()
    {
        RB = GetComponent<Rigidbody>();
        AlreadyHit = new List<GameObject>();
        Destroy(this.gameObject, destroyTime);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!AlreadyHit.Contains(other.gameObject))
        {
            AlreadyHit.Add(other.gameObject);
        }
        timesHit++;
        IHittable tryget = other.gameObject.GetComponent<IHittable>();
        if (tryget != null)
        {
            HitInfo info = new HitInfo(this, tryget);

            DamageInstance DI = new DamageInstance(this);
            DI.PlayerAttackEffects = Source.PlayerAttackEffects;
            
            DI.AddHitInfo(info);
            DI.Deploy();
        }
        if (AlreadyHit.Count >= ProjectileBullet.maxPenetrations + 1)
            Destroy(this.gameObject);
    }
    public override void FixedUpdate()
    {

        RB.MovePosition(transform.position+ transform.forward * Speed*Time.fixedDeltaTime);
    }
}
