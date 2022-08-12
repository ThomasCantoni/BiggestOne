using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class HealthBarObject: MonoBehaviour
{
    [Tooltip("Pixel amount from the Camera's center out of which the health bar will deactivate")]
    public Vector2 CenterLimits;
    public GameObject UI_ElementToRender;

    private GameObject Player;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBrain Brain;
    private Camera Camera;
  
    private RectTransform ui_toMove;
 
    
    // Start is called before the first frame update
    
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Brain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
        Camera = Brain.OutputCamera;
        virtualCamera = GameObject.FindGameObjectWithTag("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
         //canvasRenderer = elementCanvas.GetComponentInChildren<CanvasRenderer>();
        if (UI_ElementToRender == null)
        {
            Debug.LogError("UI Element not set!");
            this.enabled = false;
            return;
        }
        
        IsOutsideFrustum();
    }
    
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);

    }
    public void Show()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);

    }
    public void Update()
    {
        if(IsOutsideFrustum())
        {
            Debug.Log("OutSide");
            Hide();
            return;
        }
        Debug.Log("Visible to camera");
        Vector2 pointOnScreen = Camera.WorldToScreenPoint(this.transform.position, Camera.stereoActiveEye);
        if (PositionOutOfLimits(pointOnScreen))
        {
            Debug.Log("Out of limits vector2");
            Hide();
            return;
        }
        Debug.Log("Visible and within limits");

        UI_ElementToRender.GetComponent<RectTransform>().position = pointOnScreen;
    }
    private bool IsOutsideFrustum()
    {
        float angle = VectorOps.AngleVec(virtualCamera.transform.forward,(this.transform.position- virtualCamera.transform.position).normalized );
        //Debug.Log("ANGLE iS " + angle);
        //Debug.DrawLine(virtualCamera.transform.position, this.transform.position);
        //Debug.DrawRay(virtualCamera.transform.position, virtualCamera.transform.forward * 20f, Color.green, 0.5f);
        return angle > (virtualCamera.m_Lens.FieldOfView * 0.68f) ;

    }
    
    private bool PositionOutOfLimits(Vector2 pos)
    {

        return (
            (pos.x >= Camera.pixelWidth     * 0.5f + CenterLimits.x)   ||
            (pos.x <  Camera.pixelWidth     * 0.5f - CenterLimits.x)   ||
            (pos.y >= Camera.pixelHeight    * 0.5f + CenterLimits.y)   ||
            (pos.y <  Camera.pixelHeight   * 0.5f - CenterLimits.y)
            );
    }
   
}
