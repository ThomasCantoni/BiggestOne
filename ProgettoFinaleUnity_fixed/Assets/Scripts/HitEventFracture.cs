using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HitEventFracture : MonoBehaviour, IHittable
{
    public FractureType FractureType;
    public UnityEvent<FractureInfo> OnHitEventWithFracture;

    public MonoBehaviour Mono
    {
        get { return this; }
    }

    public void OnHit(HitInfo hitInfo)
    {
        if(FractureType == hitInfo.FractureInfo.FractureType)
            OnHitEventWithFracture.Invoke(hitInfo.FractureInfo);
    }




}
