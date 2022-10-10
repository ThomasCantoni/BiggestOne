using TMPro;
using UnityEngine;

public class PlayerInteractableRaycast : MonoBehaviour
{
    public float range = 2f;
    public LayerMask mask;
    public InputCooker IC;
    public FirstPersonController FPS;
    public IInteractable objectDetected;
    public TextMeshProUGUI MessageText;

    private void Start()
    {
        IC.playerInteract += TryInteract;
    }
    private void Update()
    {
        Ray r = new Ray(this.transform.position,this.transform.forward);
        RaycastHit info;
        if (Physics.Raycast(this.transform.position, this.transform.forward,out info, range, mask))
        {
            objectDetected = info.collider.transform.gameObject.GetComponent<IInteractable>();
            if (objectDetected != null)
            {
                MessageText.text = objectDetected.InteractMessage;
                return;
            }
            else
            {
                MessageText.text = "";
                objectDetected = null;
            }
        }
        else
        {
            MessageText.text = "";
            objectDetected = null;
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
