using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMeshPrefragmented : MonoBehaviour
{
    public Transform Parent;
    public float ExplosionForce, Radius;
    public float FadeOutTime = 3f;
    public void DestroyMesh(IHittableInformation Info)
    {

        if (Parent == null)
        {
            Parent = this.gameObject.transform;

        }
        Debug.Log("CRACK!");
        for (int i = 0; i < Parent.childCount; i++)
        {
            Rigidbody rb = Parent.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(ExplosionForce, Info.raycastInfo.point, Radius);
        }
        Destroy(Parent.gameObject, FadeOutTime);

    }
}
