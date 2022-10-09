using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class ShopBox : MonoBehaviour,IInteractable
{
    public string InteractMessage { get; set; }
    public InputCooker IC;
    public PlayerInvetory PI;
    public ChainableAttack chainableAttack;
    public PlayerAttackEffects attackEffects;
    public InvetoryUI InvetoryUI;
    public PlayerInvetory playerInvetor;
    public float duration = 5f;
    public int cost;
    public UnityEvent onInteract;
    public UnityEvent InteractUnityEvent { get { return onInteract; } set => throw new System.NotImplementedException(); }

    public void ChainableAtt()
    {
        if (attackEffects.ChainableAttackList.Contains(chainableAttack))
        {
            return;
        }
        if (playerInvetor.NumberOfCoins >= cost)
        {
            playerInvetor.NumberOfCoins -= cost;
            InvetoryUI.UpdateCoinText(playerInvetor);
            attackEffects.Add(chainableAttack);
            StartCoroutine(removePowerUps(attackEffects));
        }

    }
    public IEnumerator removePowerUps(PlayerAttackEffects PAE)
    {
        yield return new WaitForSeconds(duration);
        PAE.Remove(chainableAttack);
    }

    public void OnInteract()
    {
        ChainableAtt();
        onInteract?.Invoke();
    }
}
