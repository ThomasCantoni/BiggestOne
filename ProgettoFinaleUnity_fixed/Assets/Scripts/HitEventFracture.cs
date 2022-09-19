using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HitEventFracture : MonoBehaviour, IHittable
{
    
    public UnityEvent<FractureInfo> OnHitEventWithFracture;

    public MonoBehaviour Mono
    {
        get { return this; }
    }

    public void OnHit(HitInfo hitInfo)
    {
        OnHitEventWithFracture.Invoke(hitInfo.FractureInfo);
    }




}
