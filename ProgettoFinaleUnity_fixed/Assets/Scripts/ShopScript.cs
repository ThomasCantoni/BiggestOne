using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;


public class ShopScript : MonoBehaviour
{
    public InputCooker IC;
    public TextMeshProUGUI PressE;
    public PlayerInvetory PI;
    public LayerMask mask;
    public TriggerTutorialScript TutorilTrigger;
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
            if (result.distance < 10 && result.gameObject.GetComponent<Interactable>() != false || TutorilTrigger.TutorialShop)
            {
                onInteract = result.gameObject.GetComponent<Interactable>().onInteract;
                if (TutorilTrigger.TutorialShop)
                {
                    TutorilTrigger.Trigger.SetActive(true);
                }
                Debug.Log("Hit " + result.gameObject.name);
                //PressE.enabled = true;
                onInteract?.Invoke();
            }
        }
    }
    private void Update()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(IC.CameraHolder.position, IC.CameraHolder.forward, 10.3f,mask);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.point != null)
            {
                PressE.text = "";
            }
        }
        foreach (RaycastResult result in results)
        {
            if (result.distance < 10)
            {
                PressE.text = "Press E";
            }
        }
    }
}
