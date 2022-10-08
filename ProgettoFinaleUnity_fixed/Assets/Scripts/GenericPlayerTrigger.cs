using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GenericPlayerTrigger : MonoBehaviour
{
    public UnityEvent<FirstPersonController> PlayerEntered;
    public Transform TeleportDestination;
    private void OnTriggerEnter(Collider mamt)
    {
        FirstPersonController FPS = mamt.gameObject.GetComponent<FirstPersonController>();
        if(FPS != null)
        {
            PlayerEntered?.Invoke(FPS);
        }
    }
    
    public void Teleport(FirstPersonController FPS)
    {
        FPS.Teleport(TeleportDestination);
    }
}
