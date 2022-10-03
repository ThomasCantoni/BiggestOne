using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInvetory : MonoBehaviour
{
    public int NumberOfCoins;
    public List<GrenadeScript> grenades;
    public UnityEvent<PlayerInvetory> OnCoinCollected;
    public bool HasSpecialGrenade;
    private void Start()
    {
        HasSpecialGrenade = false;
        grenades = new List<GrenadeScript>();
    }
    public void CoinCollected()
    {
        NumberOfCoins++;
        OnCoinCollected?.Invoke(this);
    }
    public void CoinCollected(int number)
    {
        NumberOfCoins++;
        OnCoinCollected?.Invoke(this);
    }
}
