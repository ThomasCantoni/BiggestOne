using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutoTestWeapon : GenericGun
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        InputCooker.PlayerReleasedShoot += () => hasShotOnce = false;
    }

    public override void Update()
    {
        //Debug.Log("HasShotOnce :" +hasShotOnce);
        //Debug.Log("CanShoot :" + CanShoot);
        //Debug.Log("Current shot cooldown : " + currentShootCD);
        if (!CanShoot)
        {
            currentShootCD -= Time.deltaTime;
            return;
        }

        if (IsAutomatic)
        {
            if (InputCooker.IsShooting)
            {
                if (currentShootCD <= 0f)
                {
                    ShootRay();
                }
            }
            else
            {
                if (isReloading == false)
                {
                    anim.SetTrigger("Shooting");
                }
            }
        }
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }
    }

    public override void Shoot()
    {
        if (!hasShotOnce && CanShoot && !isReloading)
        {
            // Debug.Log("BOOM");
            ShootRay();
            hasShotOnce = true;
            currentShootCD = shootCD;
            anim.SetTrigger("Shooting");
        }
    }

}
