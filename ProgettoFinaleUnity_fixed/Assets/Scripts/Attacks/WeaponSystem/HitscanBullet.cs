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
       
        RaycastHit[] thingsHit = Physics.RaycastAll(Owner.InputCooker.MainCamera.transform.position, 
            Owner.GetShootingDirection(), 
            100f, 
            Owner.Mask.value);

        if (thingsHit == null || thingsHit.Length == 0)
            return;


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
