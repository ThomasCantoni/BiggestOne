using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BreakMeshPrefragmented : MonoBehaviour
{
    public Transform Parent;
    public float ExplosionForce, Radius;
    public float FadeOutTime = 3f;
    public bool FragmentsHaveRigidbody = false;
    private HitInfo Info;
    public UnityEvent OnMeshDestroy;
    public MonoBehaviour Mono => throw new System.NotImplementedException();

    //public IKillable.OnDeathEvent OnMeshDestroy ;
    //public IKillable.OnDeathEvent deathEvent { get { return OnMeshDestroy; }set { OnMeshDestroy = value; } }
    public delegate void BreakMeshEvent();
    public event BreakMeshEvent OnBreakMesh;
    public void DestroyMesh(HitInfo info)
    {
        this.Info = info;
        if (Parent == null)
        {
            Parent = this.gameObject.transform;
        }
        if (FragmentsHaveRigidbody)
        {
            RigidBodyFragment();
        }
        else
        {
            DefaultFragment();
        }
        OnDeath();
    }

    private void DefaultFragment()
    {
        for (int i = 0; i < Parent.childCount; i++)
        {
            Rigidbody rb = Parent.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(ExplosionForce, Info.FractureInfo.collisionPoint, Radius);
        }
    }
    private void RigidBodyFragment()
    {
        for (int i = 0; i < Parent.childCount; i++)
        {
            Rigidbody rb = Parent.GetChild(i).gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(ExplosionForce, Info.FractureInfo.collisionPoint, Radius);
        }
    }

    public void OnDeath()
    {
        OnMeshDestroy?.Invoke();
        Destroy(Parent.gameObject, FadeOutTime);
    }

    
}
