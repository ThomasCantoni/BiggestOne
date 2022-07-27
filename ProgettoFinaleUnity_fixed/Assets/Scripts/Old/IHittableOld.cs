using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HittableType
{
    Chomper,
    Spitter,
    Gunner,
    Boss,
    Other
}

public interface IHittableOld
{
    
    public abstract HittableType OnHit(Collider sender);
}
