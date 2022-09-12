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
            if (atk == null)
            {
                continue;
            }
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
        if (atk == null)
        {
            return null;
        }
        LinkedListNode<ChainableAttack> newNode = new LinkedListNode<ChainableAttack>(atk);
        if (atk.ChainAttackApplicationMode == ChainAttackApplicationType.PerEnemy)
            PerEnemyAttacks.AddLast(newNode);
        else
            PerShotAttacks.AddLast(newNode);
        ChainableAttackList.Add(atk);
        return newNode;
    }
    public void Remove(ChainableAttack atk)
    {
        //LinkedListNode<ChainableAttack> toRemove = PerEnemyAttacks.Find(atk);

        //PerEnemyAttacks.Remove(toRemove);
        //PerShotAttacks.Remove(toRemove);
        GetCorrectList(atk).Remove(atk);
        ChainableAttackList.Remove(atk);
    }
    public LinkedList<ChainableAttack> GetCorrectList(ChainableAttack atk)
    {
        if (atk == null)
        {
            return null;
        }
        switch (atk.ChainAttackApplicationMode)
        {
            case ChainAttackApplicationType.PerEnemy:
                return PerEnemyAttacks;
            case ChainAttackApplicationType.PerShot:
                return PerShotAttacks;
            default:
                return null;
        }
    }
}
