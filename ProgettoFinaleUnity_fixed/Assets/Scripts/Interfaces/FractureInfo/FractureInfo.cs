using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FractureType
{
    Shot_and_dash,
    Grenade,
    Slide
}
[System.Serializable]
public struct FractureInfo 
{
    [HideInInspector]
    public Vector3 collisionPoint;
    [HideInInspector]
    public Vector3 collisionNormal;
    public float Radius;
    public float Force;
    public FractureType FractureType;
    //public float Damage;
}
