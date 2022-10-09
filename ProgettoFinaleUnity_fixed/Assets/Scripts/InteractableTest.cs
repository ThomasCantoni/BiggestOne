using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class InteractableTest : MonoBehaviour, IInteractable
{
    public UnityEvent InteractUnityEvent { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public TextMeshPro Text;
    public string InteractMessage { get { return Text.text; } set { Text.text = value; } }

    public void OnInteract()
    {
        Debug.Log("AAAAAA");
    }
}
