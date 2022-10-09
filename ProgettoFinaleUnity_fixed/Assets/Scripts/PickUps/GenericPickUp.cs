using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GenericPickUp : MonoBehaviour
{
    public TextMeshPro TextCanvas;
    public UnityEvent OnPickUpUnityEvent;
    public delegate void OnPickUpCollectedDelegate(GenericPickUp pickedUp);
    public delegate void OnPlayerPickUpDelegate(FirstPersonController Player);
    public OnPickUpCollectedDelegate OnPickedUp;
    public OnPlayerPickUpDelegate OnPlayerPickedUp;
    public virtual void OnTriggerEnter(Collider other)
    {
        FirstPersonController FPS = other.GetComponent<FirstPersonController>();

        if(FPS != null)
        {
            OnPickedUp?.Invoke(this);
            OnPlayerPickedUp?.Invoke(FPS);
        }
    }
}
