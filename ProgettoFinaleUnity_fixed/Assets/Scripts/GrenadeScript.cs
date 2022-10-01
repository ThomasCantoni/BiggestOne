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
    public Vector3 targetVector;
    public Rigidbody RB;
    private Vector3 vectorPos;
    public Vector3 targetForward;
    public float Speed = 10f;
    bool HasReachedDestination = false;
    public DamageStats DamageStats { get { return DamageValues; } set => DamageValues = value; }

    public MonoBehaviour Mono
    {
        get { return this; }
    }

    public void Start()
    {
        RB = GetComponent<Rigidbody>();
        vectorPos = this.transform.position;
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
    private void Update()
    {
        if (HasReachedDestination == false)
        {

            if (Vector3.Distance(this.transform.position, targetVector) > 0.1f)
            {
                vectorPos = targetVector - this.transform.position;
            }
            else
            {
                HasReachedDestination=true;
            }
        }
        else
        {
            vectorPos = targetForward;
        }
        
        RB.MovePosition(this.transform.position + (vectorPos.normalized * Speed * Time.deltaTime));

    }
}
