using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IHittableInformation
{
    [Tooltip("The GameObject from which this information comes from. \r If not set it's automatically set to the owner of component.")]
    public GameObject sender;
    public RaycastHit raycastInfo;
    public float Damage;
}
public interface IHittable
{
    public abstract void OnHit(IHittableInformation info);

}
