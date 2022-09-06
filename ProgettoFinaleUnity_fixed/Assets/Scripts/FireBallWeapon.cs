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

   
    public override void Shoot()
    {
        if (!CanShoot) return;
        GameObject fireBall = Instantiate(bullet, spawnBullet.position, spawnBullet.transform.rotation);
        BulletScript BS = fireBall.GetComponent<BulletScript>();
        BS.Source = this;
        DeductAmmo();
    }

}
