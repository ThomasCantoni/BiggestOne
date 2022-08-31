using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    public List<ChainableAttack> InspectorList;
    public LinkedList<ChainableAttack> Attacks;


    private void Start()
    {
        Attacks = new LinkedList<ChainableAttack>();
        foreach(ChainableAttack atk in InspectorList)
        {
            Attacks.AddLast(atk);
        }
    }
    public LinkedListNode<ChainableAttack> Add(ChainableAttack atk)
    {
        LinkedListNode<ChainableAttack> newNode = new LinkedListNode<ChainableAttack>(atk);
        Attacks.AddLast(newNode);
        return newNode;
    }
    public LinkedListNode<ChainableAttack> Remove(ChainableAttack atk)
    {
        LinkedListNode<ChainableAttack> toRemove = Attacks.Find(atk);
        
        Attacks.Remove(toRemove);
        return toRemove;
    }
}
