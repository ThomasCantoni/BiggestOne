using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Fracture2))]
[ExecuteInEditMode]
public class HitEventFracture : MonoBehaviour, IHittable
{
    public FractureType FractureType;
    public UnityEvent<FractureInfo> OnHitEventWithFracture;

    public MonoBehaviour Mono
    {
        get { return this; }
    }
    public void Start()
    {
        
        Rigidbody RB = GetComponent<Rigidbody>();
        RB.constraints = RigidbodyConstraints.FreezeAll;
    }
    public void OnHit(HitInfo hitInfo)
    {
        if(FractureType == hitInfo.FractureInfo.FractureType)
            OnHitEventWithFracture.Invoke(hitInfo.FractureInfo);
    }




}
