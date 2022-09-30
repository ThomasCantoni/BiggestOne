using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvetoryUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void UpdateCoinText(PlayerInvetory playerInvetory)
    {
        coinText.text = playerInvetory.NumberOfCoins.ToString();
    }
}
