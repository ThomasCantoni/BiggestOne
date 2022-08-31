using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainableAttack : ScriptableObject
{
    public abstract void Apply(IHittableInformation info);
}
