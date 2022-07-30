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
    }

    public override void Shoot()
    {
       if(!hasShotOnce && CanShoot)
        {
            Debug.Log("BOOM");
            ShootRay();
            hasShotOnce = true;
            currentShootCD = shootCD;
        }
    }
    
}
