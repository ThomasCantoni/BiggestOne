
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : GenericPickUp
{
    public int CoinAmount = 1;
    public override void OnTriggerEnter(Collider other)
    {
        
        PlayerInvetory playerInvetory = other.GetComponent<PlayerInvetory>();
        if (playerInvetory != null)
        {
            playerInvetory.CoinCollected(CoinAmount);
            Destroy(this.gameObject);
        }
    }
    
}
