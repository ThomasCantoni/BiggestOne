using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    public List<ChainableAttack> ChainableAttackList;
    public LinkedList<ChainableAttack> PerEnemyAttacks;
    public LinkedList<ChainableAttack> PerShotAttacks;
    public List<WeaponBuff> WeaponBuffsList;
    public LinkedList<WeaponBuff> WeaponBuffs;
    private void Start()
    {
        PerEnemyAttacks = new LinkedList<ChainableAttack>();
        PerShotAttacks = new LinkedList<ChainableAttack>();
        WeaponBuffs = new LinkedList<WeaponBuff>();
        //WeaponBuffsList = new List<WeaponBuff>();


        foreach (ChainableAttack atk in ChainableAttackList)
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
        foreach(WeaponBuff x in WeaponBuffsList)
        {
            WeaponBuffs.AddLast(x);
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
