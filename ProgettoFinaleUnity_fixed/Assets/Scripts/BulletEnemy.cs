using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour, IDamager
{
    public static List<EnemyClass> AlreadyHit = new List<EnemyClass>();
    public float Speed = 1f;
    public DamageStats damage;
    Rigidbody RB;
    public DamageStats DamageStats
    {
        get { return damage; }
        set { damage = value; }
    }

    public MonoBehaviour Mono => throw new System.NotImplementedException();

    private void OnTriggerEnter(Collider other)
    {
        IHittable tryget = other.gameObject.GetComponent<IHittable>();
        if (tryget != null)
        {
            HitInfo info = new HitInfo(this, tryget);
            other.GetComponent<HealthPlayer>().OnHit(info);
        }

        Destroy(this.gameObject);
    }
    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        Destroy(this.gameObject, 10f);
    }

    public void FixedUpdate()
    {
        RB.MovePosition(this.transform.position + transform.forward * Speed * Time.fixedDeltaTime);

    }

}
