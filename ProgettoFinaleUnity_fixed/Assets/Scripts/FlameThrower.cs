using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : GenericGun
{
    public ParticleSystem Ps;
    public float speed = 5f;
    public bool isShooting;
    public override void Start()
    {
        InputCooker.PlayerPressedShoot += () => Ps.Play(true);
        InputCooker.PlayerReleasedShoot += () => Ps.Stop(false);
    }


    public override void Shoot()
    {
        DeductAmmo();

    }
}
