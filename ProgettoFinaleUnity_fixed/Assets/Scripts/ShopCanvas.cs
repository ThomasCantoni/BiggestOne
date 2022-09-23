using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopCanvas : MonoBehaviour,IInteractable
{
    public UnityEvent onInteract;
    public int[,] shopItems = new int[4, 4];
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public UnityEvent InteractUnityEvent { get { return onInteract; } set => throw new System.NotImplementedException(); }
    public void OnInteract()
    {
        
        
    }

    private void Start()
    {
        //playerInvetory.UpdateCoinText(playerInvetor);
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();

        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;

        shopItems[2, 1] = 1;
        shopItems[2, 2] = 2;
        shopItems[2, 3] = 3;

    }
    private void Update()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                IInteractable toInteract = result.gameObject.GetComponent<IInteractable>();
                if (toInteract != null && result.distance < 10)
                {
                    //toInteract.OnInteract();
                    Debug.Log("AO");
                    return;
                }
            }
        }
    }

}
