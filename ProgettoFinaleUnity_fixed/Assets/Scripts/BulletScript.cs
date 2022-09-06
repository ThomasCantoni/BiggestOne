using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject,5f);

    }
    public void Shoot()
    {
        DamageInstance newDamageInstance = new DamageInstance();

        //newDamageInstance.Hits = ShootRays();

        newDamageInstance.Deploy();
        currentAmmo--;
        currentShootCD = shootCD;
        if (currentAmmo <= 0)
        {
            StartReload();

        }
        else
        {
            anim.SetTrigger("Shooting");
        }
    }

}
