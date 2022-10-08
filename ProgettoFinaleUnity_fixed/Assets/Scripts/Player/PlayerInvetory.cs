using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerInvetory : MonoBehaviour
{
    public int NumberOfCoins;
    public List<GrenadeScript> grenades;
    public float mulCoins;
    public UnityEvent<PlayerInvetory> OnCoinCollected;
    public bool HasSpecialGrenade;
    public TextMeshProUGUI UI_PurchaseCanvas;
    public InvetoryUI CoinNumber;
    private void Start()
    {
        HasSpecialGrenade = false;
        grenades = new List<GrenadeScript>();
    }
    public void CoinCollected()
    {
        OnCoinCollected?.Invoke(this);
    }
    public void CoinCollected(int number)
    {
        NumberOfCoins += number * (int)mulCoins;
        OnCoinCollected?.Invoke(this);
    }
}
