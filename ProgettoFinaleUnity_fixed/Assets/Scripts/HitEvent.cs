using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HitEvent : MonoBehaviour, IHittable
{
    public UnityEvent<HitInfo> OnHitEvent;
    public void OnHit(HitInfo hitInfo)
    {
        OnHitEvent.Invoke(hitInfo);
    }




}
