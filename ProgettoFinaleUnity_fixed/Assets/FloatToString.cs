using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatToString : MonoBehaviour
{
    // Start is called before the first frame update
    public void FloatToStr(float n)
    {
       
       GetComponent<TMP_Text>().text = n.ToString();
        
    }
    public void VolumeToString(Slider volumeSl)
    {
        int  percentage =  Mathf.FloorToInt((volumeSl.value / volumeSl.maxValue) * 100f);
        //int text =(int) (((80f+volumeSl.value) / -80f) *- 100f);

        GetComponent<TMP_Text>().text = percentage.ToString();
    }
}
