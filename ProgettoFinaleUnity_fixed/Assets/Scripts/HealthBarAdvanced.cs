using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAdvanced : MonoBehaviour
{
    public GameObject UI_ElementToRender;
    public Camera MainCamera;
    CanvasRenderer MR;
    public Canvas HealthElement;
    private RectTransform ui_toMove;
    private GameObject player;
    private bool update = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = Camera.main;
        if(HealthElement == null)
        {
            HealthElement = GetComponentInChildren<Canvas>();
        }
        MR = HealthElement.GetComponent<CanvasRenderer>();
        ui_toMove = HealthElement.GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>();


        //each time the player moves the camera i calculate the position
        InputCooker IC = player.GetComponent<InputCooker>(); 
        IC.PlayerRotatedCamera += CalculatePosition;
        IC.PlayerMoved += () => update = true;
        IC.PlayerStopped += () => update = false;
    }
    public void Update()
    {
       if(!update)
        {
            return;
        }
        CalculatePosition();
    }
    private void CalculatePosition()
    {
       
        float angle = VectorOps.AngleVec((MainCamera.transform.position - this.transform.position).normalized, MainCamera.transform.forward);
        
        if (angle <= 90f)
        {
            MR.cull = true;
            return;
        }
        MR.cull = false;
        Vector2 pointOnScreen = MainCamera.WorldToScreenPoint(this.transform.position,MainCamera.stereoActiveEye);
        //CanvasToRender.SetActive(true);
        pointOnScreen.x = Mathf.Clamp(pointOnScreen.x, -200, MainCamera.pixelWidth +200);
        pointOnScreen.y = Mathf.Clamp(pointOnScreen.y, -200, MainCamera.pixelHeight+200);


        ui_toMove.GetComponent<RectTransform>().position =  pointOnScreen;
    }
    
}
