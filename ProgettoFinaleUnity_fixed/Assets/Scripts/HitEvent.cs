using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HitEvent : MonoBehaviour,IHittable
{
    public UnityEvent<float> OnHitEvent;
    public void OnHit(Collider sender,float damage)
    {
        OnHitEvent.Invoke(damage);
    }

    

   
}
