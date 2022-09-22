using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    public ChainableAttack chainableAttack;
    public PlayerAttackEffects attackEffects;
    public InvetoryUI playerInvetory;
    public PlayerInvetory playerInvetor;
    public GameObject IDs;
    public float duration = 5f;
    public int[,] shopItems = new int[4, 4];
    private void Start()
    {
        playerInvetory.UpdateCoinText(playerInvetor);

        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;

        shopItems[2, 1] = 1;
        shopItems[2, 2] = 2;
        shopItems[2, 3] = 3;

    }
    public void ChainableAtt()
    {
        if (attackEffects.ChainableAttackList.Contains(chainableAttack))
        {
            return;
        }
        if (playerInvetor.NumberOfCoins >= shopItems[2, IDs.GetComponent<ImageInfo>().ItemID])
        {
            playerInvetor.NumberOfCoins -= shopItems[2, IDs.GetComponent<ImageInfo>().ItemID];
            playerInvetory.UpdateCoinText(playerInvetor);
            attackEffects.Add(chainableAttack);
            StartCoroutine(removePowerUps(attackEffects));
        }
        
    }
    public IEnumerator removePowerUps(PlayerAttackEffects PAE)
    {
        yield return new WaitForSeconds(duration);
        PAE.Remove(chainableAttack);
    }
}
