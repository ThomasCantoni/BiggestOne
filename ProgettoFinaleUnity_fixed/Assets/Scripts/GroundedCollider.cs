using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class GroundedCollider : MonoBehaviour
{
    
    public SphereCollider Small, Big;
    public bool touching;
    public bool DoRaycast = true;
    public Vector3 RaycastDirection = Vector3.down;
    public LayerMask LayerToHit;
    public float Distance;
    [SerializeField]
    public RaycastHit Info;
    float groundAngle = 0f;
    public float AngleOfGround
    {
        get { return groundAngle; }
        private set { groundAngle = value; }
    }
    private void OnTriggerEnter(Collider other)
    {
        touching = true;
        ExecRaycast();
    }
    private void OnTriggerStay(Collider other)
    {

        touching = true;
        ExecRaycast();

    }
    private void OnTriggerExit(Collider other)
    {
        touching = false;
        AngleOfGround = 0f;

    }
    private void ExecRaycast()
    {
        if (!DoRaycast)
            return;

        bool raycastHitSomething = Physics.Raycast(transform.position, RaycastDirection, out Info, Distance, LayerToHit.value);

        if (touching && raycastHitSomething)
            AngleOfGround = (float)(Math.Acos(Vector3.Dot(Info.normal, -RaycastDirection)) * 180f / Math.PI);
        else
            AngleOfGround = 0f;
    }
    public void Disable()
    {
        SphereCollider[] c = GetComponents<SphereCollider>();
        foreach (SphereCollider x in c)
        {
            x.enabled = false;
        }
        touching = false;
        this.enabled = false;
    }
    public void Enable()
    {
       SphereCollider[] c = GetComponents<SphereCollider>();
       foreach(SphereCollider x in c)
        {
            x.enabled = true;
        }
        this.enabled = true;
    }
    public void SwitchToSmall()
    {
        Small.enabled = true;
        Big.enabled = false;
    }
    public void SwitchToBig()
    {
        Small.enabled = false;
        Big.enabled = true;
    }
   
}
