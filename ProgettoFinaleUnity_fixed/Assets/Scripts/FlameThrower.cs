using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : GenericGun
{
    public ParticleSystem Ps;
    public float FlameDistanceReachTime=0.5f;
    public float FlameDistance = 10f;
    private float speed
    {
        get { return  FlameDistance / FlameDistanceReachTime; }
    }
    public bool CanActivatePS 
    { 
        get
        { 
            return currentAmmo > 0 && !IsReloading; 
        } 
    }
    
    public Transform BulletOffset;
    public GameObject BulletToInstantiate;
    //public ProjectileBulletMonobehaviour bullet2;
    
    public override void Start()
    {
        base.Start();
        InputCooker.PlayerPressedShoot += () =>
        {
            if (CanActivatePS)
                Ps.Play(true);
        };
        InputCooker.PlayerReleasedShoot += () => Ps.Stop(true);

    }
    
    protected override void Subscribe(bool subscribe)
    {
        base.Subscribe(subscribe);
        
    }

    public override void Shoot()
    {
        if (CanShoot)
        {
            
            BeforeShoot?.Invoke(this);
            ProjectileBullet newProjectile = new ProjectileBullet(this,BulletToInstantiate, BulletOffset);
            newProjectile.maxPenetrations = MaxPenetrations;
            newProjectile.ProjectileBulletGameObject.Speed = speed;
            newProjectile.ProjectileBulletGameObject.destroyTime = FlameDistanceReachTime;

            newProjectile.Deploy();
            BulletCreated?.Invoke(newProjectile);
            
            DeductAmmo();

        }
        if (currentAmmo == 0)
        {
            StartReload();
            
        }
        
        
    }

    public override void StartReload()
    {
        base.StartReload();
        if(CanStartReload)
            Ps.Stop(true);
    }
}
