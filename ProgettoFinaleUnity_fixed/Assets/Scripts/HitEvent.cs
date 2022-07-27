using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HitEvent : MonoBehaviour,IHittable
{
    public UnityEvent<IHittableInformation> OnHitEvent;
    public void OnHit(IHittableInformation hitInfo)
    {
        OnHitEvent.Invoke(hitInfo);
    }

    

   
}
