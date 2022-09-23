using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDetector : MonoBehaviour
{
    public Material mat;
    private void Update()
    {
        string extracted = mat.GetFloat("_MATERIALTYPE").ToString();
        Debug.Log(extracted);
    }
}
