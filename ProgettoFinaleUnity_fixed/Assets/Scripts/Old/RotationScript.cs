using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float DegreesPerSecond = 90f;



    void Update()
    {
        this.transform.Rotate(DegreesPerSecond * Time.deltaTime,0,0);
    }
}
