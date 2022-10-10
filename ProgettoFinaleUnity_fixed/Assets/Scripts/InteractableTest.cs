using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class InteractableTest : MonoBehaviour, IInteractable
{
    public UnityEvent OnInteractUnityEvent;
    public UnityEvent InteractUnityEvent { get { return OnInteractUnityEvent; } }
    public string Message;
    public string InteractMessage { get { return Message; } set { Message = value; } }
    
    public void OnInteract()
    {
        Debug.Log("AAAAAA");
    }
}
