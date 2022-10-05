using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : GenericPickUp
{
    public SimpleTimer St;
    public EnemyClass EC;
    
    public override void OnTriggerEnter(Collider other)
    {
        //EC = other.GetComponent<EnemyClass>();
        if (EC != null)
        {
            OnPickUpUnityEvent?.Invoke();
            EC.Emp();
            St.TimerCompleteEvent += RemoveAfter;
            St.StartTimer();
        }
    }
    public void RemoveAfter()
    {
        EC.anim.SetBool("EMP", false);
        EC.EMPActive = false;
        EC.NavMeshAgent.speed = 2;
        Destroy(this.gameObject);
    }
}
