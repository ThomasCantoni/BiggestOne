using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallWeapon : GenericGun
{
    public Transform spawnBullet;
    public ProjectileBulletMonobehaviour bullet2;
    //public GameObject bullet;
    public float speed = 5f;
    //ProjectileBullet newProjectile;
    public override void Start()
    {
    }

   
    public override void Shoot()
    {
        if (!CanShoot) return;
        ProjectileBullet newProjectile = new ProjectileBullet(this,bullet2,spawnBullet);
        newProjectile.Deploy();
        BulletCreated?.Invoke(newProjectile);
        //GameObject fireBall = Instantiate(bullet, spawnBullet.position, spawnBullet.transform.rotation);
        //ProjectileBulletMonobehaviour BS = fireBall.GetComponent<ProjectileBulletMonobehaviour>();
        //BS.Source = this;
        DeductAmmo();
        if (currentAmmo == 0)
        {
            StartReload();
        }
    }

}
