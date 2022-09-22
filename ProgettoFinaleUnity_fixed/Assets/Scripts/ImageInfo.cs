using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImageInfo : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI PriceTxt;
    //public TextMeshProUGUI QuantityTxt;
    public GameObject shopMNG;

    private void Update()
    {
        PriceTxt.text = "Price: $" + shopMNG.GetComponent<Interactable>().shopItems[2, ItemID].ToString();
    }
}
