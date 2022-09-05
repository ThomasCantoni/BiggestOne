using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    public List<ChainableAttack> InspectorList;
    public LinkedList<ChainableAttack> PerEnemyAttacks;
    public LinkedList<ChainableAttack> PerShotAttacks;

    private void Start()
    {
        PerEnemyAttacks = new LinkedList<ChainableAttack>();
        foreach(ChainableAttack atk in InspectorList)
        {
            switch (atk.ChainAttackApplicationMode)
            {
                case ChainAttackApplicationType.PerEnemy:
                     PerEnemyAttacks.AddLast(atk);
                    break;
                case ChainAttackApplicationType.PerShot:
                    PerShotAttacks.AddLast(atk);
                    break;
                default:
                    break;
            }
        }
    }
    public LinkedListNode<ChainableAttack> Add(ChainableAttack atk)
    {
        LinkedListNode<ChainableAttack> newNode = new LinkedListNode<ChainableAttack>(atk);
        if (atk.ChainAttackApplicationMode == ChainAttackApplicationType.PerEnemy)
            PerEnemyAttacks.AddLast(newNode);
        else
            PerShotAttacks.AddLast(newNode);
        return newNode;
    }
    public LinkedListNode<ChainableAttack> Remove(ChainableAttack atk)
    {
        LinkedListNode<ChainableAttack> toRemove = PerEnemyAttacks.Find(atk);
        
        PerEnemyAttacks.Remove(toRemove);
        PerShotAttacks.Remove(toRemove);
        return toRemove;
    }
}
