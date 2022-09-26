using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanBullet
{
    
    public GenericGun Owner;
    //public List<EnemyClass> EnemiesHit;
    public List<HitInfo> Hits;
    public int maxPenetrations;
    
    public HitscanBullet(GenericGun owner)
    {
        Owner = owner;
        
    }

    public bool ShootRay()
    {

        RaycastHit[] thingsHit = Physics.RaycastAll(Owner.InputCooker.MainCamera.transform.position, 
            Owner.GetShootingDirection(), 
            100f, 
            Owner.Mask.value);

        if (thingsHit == null || thingsHit.Length == 0)
            return false;


        //sort by distance closest to furthest
        for (int i = 0; i < thingsHit.Length; i++)
        {
            for (int j = 0; j < thingsHit.Length; j++)
            {

                float distance1 = Vector3.Distance(Owner.Player.transform.position, thingsHit[i].point);
                float distance2 = Vector3.Distance(Owner.Player.transform.position, thingsHit[j].point);
                if(distance1<distance2)
                {
                    RaycastHit swapper = thingsHit[i];
                    thingsHit[i] = thingsHit[j];
                    thingsHit[j] = swapper;
                }
            }
        }
        List<HitInfo> hitinfoList = new List<HitInfo>(1+maxPenetrations);
        for (int i = 0; i < thingsHit.Length; i++)
        {
            if (hitinfoList.Count >= maxPenetrations+1)
                break;
            RaycastHit hit = thingsHit[i];

            IHittable thingHit = hit.collider.GetComponent<IHittable>();
            if (thingHit != null)
            {
                HitInfo hitInfo = new HitInfo(Owner,thingHit);
                hitInfo.SetRaycastPositions(hit);
                
                hitInfo.IsChainableAttack = false;

                hitinfoList.Add(hitInfo);
                Owner.HitInfoCreated?.Invoke(hitInfo);
            }
        }

        Hits = hitinfoList;
        Owner.HitscanBulletPopulated?.Invoke(this);
        return true;
    }
    

   
}
