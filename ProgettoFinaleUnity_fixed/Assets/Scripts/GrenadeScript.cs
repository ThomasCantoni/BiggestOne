using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour,IDamager
{

    public float Fuse = 2f;
    public float Radius = 5f;
    public float FractureForce = 100f;
    public LayerMask ThingsHittable;
    public DamageStats DamageValues;
    public DamageStats DamageStats { get { return DamageValues; } set => DamageValues = value; }

    public MonoBehaviour Mono
    {
        get { return this; }
    }

    public void Start()
    {
        Destroy(this.gameObject,6f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Collider[] insideExplosionRadius = Physics.OverlapSphere(this.transform.position, Radius, ThingsHittable);

        foreach (Collider x in insideExplosionRadius)
        {
            IHittable toHit = x.gameObject.GetComponent<IHittable>();
            if (toHit != null)
            {
                HitInfo newHit = new HitInfo();
                newHit.FractureInfo.collisionPoint = this.transform.position;
                newHit.FractureInfo.FractureType = FractureType.Grenade;
                newHit.FractureInfo.Radius = this.Radius;
                newHit.FractureInfo.Force = FractureForce;
                toHit.OnHit(newHit);
            }
        }
        Destroy(this.gameObject);
    }
}
