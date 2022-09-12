using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjDeath : MonoBehaviour
{
    public List<GameObject> objToDrops;
    public IKillable IDA;
    public Transform OffSetTransform;
    void Start()
    {
        Initialize();
        
    }
    void Initialize()
    {
        IDA = GetComponent<IKillable>();
        if (IDA != null)
        {
            IDA.deathEvent += Drop;

        }
        
    }
    public void Drop()
    {
        if (OffSetTransform == null)
        {
            OffSetTransform = this.transform;
        }
        for (int i = 0; i < objToDrops.Count; i++)
        {
            Instantiate(objToDrops[i],OffSetTransform.position,Quaternion.Euler(0,0,90));
        }
        Destroy(this);
    }
}
