using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorEnemyCounter : MonoBehaviour
{
    
    public Transform[] Enemies;
    public UnityAction OnEnemyDeath;
    public int AmountDead =0;
    void Start()
    {
        OnEnemyDeath = IncreaseDead;
        for(int i=0;i<Enemies.Length;i++)
        {

            Enemies[i].GetComponent<Enemy>().OnDeath.AddListener(OnEnemyDeath);
        }
        
        
        
    }
   
    void IncreaseDead()
    {
        AmountDead++;
        if(AmountDead >= Enemies.Length)
        {

            this.GetComponent<Animator>().SetBool("isDoorOpen",true);
            this.GetComponent<Animator>().SetTrigger("OpenDoor");
        }
    }
    
}
