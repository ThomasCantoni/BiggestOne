using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjDeath : MonoBehaviour
{
    public List<GameObject> objToDrops;
    public HealthBarEnemy HBE;
    void Start()
    {
        HBE = GetComponent<HealthBarEnemy>();
        HBE.OnEnemyDeath += Drop;
    }
    public void Drop()
    {
        for (int i = 0; i < objToDrops.Count; i++)
        {
            Instantiate(objToDrops[i],this.transform.position,Quaternion.Euler(0,0,90));
        }
        Destroy(this);
    }
}
