using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanBullet : GenericBullet
{
    public override bool HasHitSomething
    {
        get { return Hits.Count > 0; } 
       
    }
    public HitscanBullet(GenericGun owner) : base(owner)
    {
        Hits = new List<HitInfo>();

    }

    public override void Deploy()
    {
        Vector3 dir = Owner.GetShootingDirection();
        RaycastHit[] thingsHit = Physics.RaycastAll(Owner.InputCooker.MainCamera.transform.position, 
            dir, 
            100f, 
            Owner.Mask.value);
        Debug.DrawRay(Owner.InputCooker.MainCamera.transform.position, dir*100f, Color.red,2f);
        Debug.Log("SHOOTING HITSCAN");
        if (thingsHit == null || thingsHit.Length == 0)
            return;


        //sort by distance closest to furthest
        for (int i = 0; i < thingsHit.Length; i++)
        {
            for (int j = 0; j < thingsHit.Length; j++)
            {
                if(thingsHit[i].distance > thingsHit[j].distance)
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
                hitInfo.FractureInfo.Force = Owner.FractureInformation.Force;
                //hitInfo.IsChainableAttack = false;

                hitinfoList.Add(hitInfo);
                Owner.HitInfoCreated?.Invoke(hitInfo);
            }
        }

        Hits = hitinfoList;
        Owner.BulletHitListPopulated?.Invoke(this);
        
    }
    

   
}
