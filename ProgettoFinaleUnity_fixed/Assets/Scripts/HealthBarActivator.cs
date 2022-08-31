using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthBarActivator : MonoBehaviour
{
    public float Radius = 1f;
    public LayerMask LayersToHit;
    // Update is called once per frame
    private Vector3 temp;
    void Update()
    {
        RaycastHit infoSphere, infoRay;
        bool sphere, ray, controlRay;
        controlRay = Physics.Raycast(this.transform.position,transform.forward, 2f,3);
        if (controlRay) // i hit something too close to the camrea
            return;
        sphere = Physics.SphereCast(this.transform.position, Radius, transform.forward, out infoSphere, 25, LayersToHit.value);
        ray = Physics.Raycast(this.transform.position, transform.forward, out infoRay, 25, LayersToHit.value);
        if (!sphere && !ray)
        {
            return;
        }


        HealthBarObject tryGetHBO;
        if (sphere && infoSphere.collider != null)
        {
            tryGetHBO = infoSphere.collider.GetComponentInChildren<HealthBarObject>();
            temp = infoSphere.point;

            if (tryGetHBO != null && tryGetHBO.CanShow())
            {
                tryGetHBO.Show();
                return;
                //Debug.Log(tryGetHBO.gameObject.name);
            }
        }
        if (ray && infoRay.collider != null)
        {
            tryGetHBO = infoRay.collider.GetComponentInChildren<HealthBarObject>();
            if (tryGetHBO != null && tryGetHBO.CanShow())
            {
                tryGetHBO.Show();
                Debug.Log(tryGetHBO.gameObject.name);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(temp, Radius);
    }
}
