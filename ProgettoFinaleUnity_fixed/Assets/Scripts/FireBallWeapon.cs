using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallWeapon : GenericGun
{
    public Transform spawnBullet;
    public GameObject bullet;
    public float speed = 5f;
    public override void Start()
    {
    }

    public override void Update()
    {
        if (!CanShoot)
        {
            Shoot();
        }
    }
    public override void Shoot()
    {
        GameObject fireBall = Instantiate(bullet, spawnBullet.position, bullet.transform.rotation);
        Rigidbody rig = fireBall.GetComponent<Rigidbody>();

        rig.AddForce(spawnBullet.forward * speed, ForceMode.Impulse);
        
    }

}
