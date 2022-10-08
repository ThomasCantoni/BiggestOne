using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    //public float DegreesPerSecond = 90f;
    public Vector3 RotationVector;


    void Update()
    {
        Vector3 vec = RotationVector *Time.deltaTime;
        this.transform.Rotate(vec.x,vec.y,vec.z);
    }
}
