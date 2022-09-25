using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : GenericGun
{
    public ParticleSystem Ps;
    public float speed = 5f;
    public override void Start()
    {
    }


    public override void Shoot()
    {
        
                Ps.gameObject.SetActive(true);

        DeductAmmo();
    }
}
