using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ShopScript : MonoBehaviour
{
    public InputCooker IC;
    public PlayerInvetory PI;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    UnityEvent onInteract;

    private void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
        IC.playerInteract += interactCanvas;
    }
    private void interactCanvas()
    {

        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.distance < 10 && result.gameObject.GetComponent<Interactable>() != false)
            {
                onInteract = result.gameObject.GetComponent<Interactable>().onInteract;
                Debug.Log("Hit " + result.gameObject.name);
                onInteract?.Invoke();
            }
        }
    }
    
}
