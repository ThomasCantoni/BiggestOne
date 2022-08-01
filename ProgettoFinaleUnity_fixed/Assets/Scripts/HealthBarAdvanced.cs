using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAdvanced : MonoBehaviour
{
    public GameObject UI_ElementToRender;
    public Camera MainCamera;
    CanvasRenderer MR;
    public Graphic test;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        MR = test.canvasRenderer;
        //MR.cull = true;
    }

    private void Update()
    {
       
        float angle = AngleVec((MainCamera.transform.position - this.transform.position).normalized, MainCamera.transform.forward);
        
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
        
       
        test.GetComponent<RectTransform>().position =  pointOnScreen;
    }
    private float AngleVec(Vector3 one, Vector3 two)
    {
       return (float)(Mathf.Acos(Vector3.Dot(one, two)) * 180f / 3.14f);
    }
}
