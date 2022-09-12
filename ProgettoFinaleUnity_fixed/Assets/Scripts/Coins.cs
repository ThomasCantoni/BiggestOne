
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        PlayerInvetory playerInvetory = other.GetComponent<PlayerInvetory>();
        if (playerInvetory != null)
        {
            playerInvetory.CoinCollected();
            Destroy(this.gameObject);
        }
    }
    
}
