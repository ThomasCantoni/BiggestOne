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

    public  void Update2()
    {
       
        ////Debug.Log("HasShotOnce :" +hasShotOnce);
        ////Debug.Log("CanShoot :" + CanShoot);
        ////Debug.Log("Current shot cooldown : " + currentShootCD);
        //if (!CanShoot)
        //{
        //    currentShootCD -= Time.deltaTime;
        //    return;
        //}

        //if (IsAutomatic)
        //{
        //    if (InputCooker.IsShooting)
        //    {
        //        if (currentShootCD <= 0f)
        //        {
        //            Shoot();
        //        }
        //    }
        //    else
        //    {
        //        if (IsReloading != false)
        //        {
        //            anim.SetTrigger("Shooting");
        //        }
        //    }
        //}
        ////if (IsReloading)
        ////    return;

        ////if (currentAmmo <= 0)
        ////{
        ////    StartReload();
        ////    return;
        ////}
        ////shotgun = false;
    }

    public override void Shoot()
    {
       if(!HasShotOnce)
        base.Shoot();
            Debug.Log("BOOM");
           
            
        
    }

}
