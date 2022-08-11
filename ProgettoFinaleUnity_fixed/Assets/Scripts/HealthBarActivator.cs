using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarActivator : MonoBehaviour
{
    public float Radius = 5f;
    public LayerMask LayersToHit;
    public RaycastHit infoSphere,infoRay;
    // Update is called once per frame
    void Update()
    {
        bool sphere, ray;
        sphere = Physics.SphereCast(this.transform.position, Radius, transform.forward, out infoSphere, 25, LayersToHit.value);
        ray = Physics.Raycast(this.transform.position, transform.forward, out infoRay, 25, LayersToHit.value);
        if (!sphere && !ray)
        {
            return;
        }
       

        HealthBarObject tryGetHBO= infoSphere.collider.GetComponentInChildren<HealthBarObject>();
        if (tryGetHBO == null)
        {
            if(ray)
            {
                tryGetHBO = infoRay.collider.GetComponentInChildren<HealthBarObject>();
                if (tryGetHBO != null)
                {
                    tryGetHBO.Show();
                }
                
            }
        }
        else
        {
            tryGetHBO.Show();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(infoSphere.point, Radius);
    }
}
