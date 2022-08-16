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

    public Vector2 PointOnScreen;
 
    
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
        //Debug.Log("HIDDEN");

        this.transform.GetChild(0).gameObject.SetActive(false);

    }
    public void Show()
    {
        //Debug.Log("Shown");
        this.transform.GetChild(0).gameObject.SetActive(true);

    }
    public bool CanShow()
    {
        if (IsOutsideFrustum())
        {
            //Debug.Log("OutSide");
            
            return false;
        }
        //Debug.Log("Visible to camera");
        PointOnScreen = Camera.WorldToScreenPoint(this.transform.position, Camera.stereoActiveEye);
        if (PositionOutOfLimits(PointOnScreen))
        {
            //Debug.Log("Out of limits vector2");
            
            return false;
        }
        else
        {
            return true;
        }
    }
    public void Update()
    {
        if(!CanShow())
        {
            Hide();
            return;
        }
        
        
        UI_ElementToRender.GetComponent<RectTransform>().position = PointOnScreen;
        
        

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
