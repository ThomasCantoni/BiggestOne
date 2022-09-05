using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChainableAttack : ScriptableObject
{
   
    public List<GameObject> EnemiesHit;
    public abstract void Apply(GameObject recepient);
    
}
public interface IStackableChainAttack
{
    public abstract void Stack(MonoBehaviour toStack);
}
