using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableRaycast : MonoBehaviour
{
    public float range = 2f;
    public LayerMask mask;
    public InputCooker IC;
    public FirstPersonController FPS;
    public IInteractable objectDetected;

    private void Start()
    {
        IC.playerInteract += TryInteract;
    }
    private void Update()
    {
        Ray r = new Ray(this.transform.position,this.transform.forward);
        RaycastHit[] info = Physics.RaycastAll(this.transform.position, this.transform.forward, range, mask);
        foreach (RaycastHit i in info)
        {
            objectDetected = i.transform.gameObject.GetComponent<IInteractable>();
            if (objectDetected != null)
            {
                return;
            }
        }
    }
        
    public void TryInteract()
    {
        if (objectDetected != null)
        {
            objectDetected.OnInteract();
        }
    }
}
