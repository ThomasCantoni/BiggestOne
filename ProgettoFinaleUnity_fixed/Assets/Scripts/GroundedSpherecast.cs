using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundedSpherecast : MonoBehaviour
{
    
    public bool Grounded
    {
        get { return grounded; }
        private set { grounded = value; }
    }
    bool grounded;
    public Vector3 Direction = Vector3.down;
    public LayerMask LayerToHit;
    public float CastRadius,Distance;
    [SerializeField]
    public RaycastHit Info;
    float groundAngle;
    public float AngleOfGround
    {
        get { return groundAngle; }
        private set { groundAngle = value; }
    }
    void Update()
    {

        Grounded = Physics.SphereCast(transform.position, CastRadius, Direction, out Info, Distance, LayerToHit.value);
        if (Grounded)
            AngleOfGround = (float)(Math.Acos(Vector3.Dot(Info.normal, -Direction)) * 180f / Math.PI);
        else
            AngleOfGround = 0f;
        //Debug.Log(AngleOfGround);
    }
    public void OnDisable()
    {
        grounded = false;
        AngleOfGround = 0f;
        this.enabled = false;
    }
    
}
