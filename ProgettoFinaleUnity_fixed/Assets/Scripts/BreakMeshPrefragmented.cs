using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakMeshPrefragmented : MonoBehaviour
{
    public Transform Parent;
    public float ExplosionForce, Radius;
    public float FadeOutTime = 3f;
    public bool FragmentsHaveRigidbody = false;
    private HitInfo Info;
    public void DestroyMesh(HitInfo info)
    {
        this.Info = info;
        if (Parent == null)
        {
            Parent = this.gameObject.transform;

        }
        if(FragmentsHaveRigidbody)
        {
            RigidBodyFragment();
        }
        else
        {
            DefaultFragment();
        }

    }

    private void DefaultFragment()
    {
        Debug.Log("CRACK!");
        for (int i = 0; i < Parent.childCount; i++)
        {
            Rigidbody rb = Parent.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(ExplosionForce, Info.collisionPoint, Radius);
        }
        Destroy(Parent.gameObject, FadeOutTime);
    }
    private void RigidBodyFragment()
    {
        Debug.Log("CRACK!");
        for (int i = 0; i < Parent.childCount; i++)
        {
            Rigidbody rb = Parent.GetChild(i).gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(ExplosionForce, Info.collisionPoint, Radius);
        }


        Destroy(Parent.gameObject, FadeOutTime);
    }

}
