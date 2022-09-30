using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInvetory : MonoBehaviour
{
    public int NumberOfCoins { get; set; }
    public UnityEvent<PlayerInvetory> OnCoinCollected;
    public void CoinCollected()
    {
        NumberOfCoins++;
        OnCoinCollected?.Invoke(this);
    }
}
