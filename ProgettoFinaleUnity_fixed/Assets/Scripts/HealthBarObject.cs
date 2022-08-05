using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class HealthBarObject: MonoBehaviour
{
    public GameObject UI_ElementToRender;
    public GameObject Player;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBrain Brain;
    private Camera Camera;
    private CanvasRenderer canvasRenderer;
    private Canvas elementCanvas;
    private RectTransform ui_toMove;
    //private GameObject player;
    private bool update = false;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Brain = Player.GetComponentInChildren<CinemachineBrain>();
        virtualCamera = Player.GetComponentInChildren<CinemachineVirtualCamera>();
        canvasRenderer = elementCanvas.GetComponentInChildren<CanvasRenderer>();
        if (UI_ElementToRender == null)
        {
            Debug.LogError("UI Element not set!");
            this.enabled = false;
            return;
        }
        else
        {
            elementCanvas = GetComponentInChildren<Canvas>();
        }
        ui_toMove = elementCanvas.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>();
        CalculatePosition();
    }
    public void Update()
    {
        CalculatePosition();
    }
    private void CalculatePosition()
    {
        float angle = VectorOps.AngleVec((Brain.transform.position - this.transform.position).normalized, Brain.transform.forward);
        if (angle <= virtualCamera.m_Lens.FieldOfView)
        {
            this.enabled = false;
            return;
        }
        canvasRenderer.cull = false;
        Vector2 pointOnScreen = Brain.OutputCamera.WorldToScreenPoint(this.transform.position,Camera.stereoActiveEye);
        //CanvasToRender.SetActive(true);
        pointOnScreen.x = Mathf.Clamp(pointOnScreen.x, -200, Camera.pixelWidth +200);
        pointOnScreen.y = Mathf.Clamp(pointOnScreen.y, -200, Camera.pixelHeight+200);


        ui_toMove.GetComponent<RectTransform>().position =  pointOnScreen;
    }
    private void OnDisable()
    {
        canvasRenderer.cull = true;
    }
}
