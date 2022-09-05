using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HitEvent : MonoBehaviour, IHittable
{
    
    public UnityEvent<HitInfo> OnHitEvent;

    public MonoBehaviour Mono
    {
        get { return this; }
    }

    public void OnHit(HitInfo hitInfo)
    {
        OnHitEvent.Invoke(hitInfo);
    }




}
