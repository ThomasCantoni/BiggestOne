using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "ScriptableObjects/Lightning")]

public class Lightning : ChainableAttack
{
    public float Radius = 5f;
    
    public LayerMask LayersToHit;

   
    public override void Apply(EnemyClass recepient)
    {
        
       Collider[] thingsHit =  Physics.OverlapSphere(recepient.transform.position,Radius,LayersToHit);
       EnemyClass firstEnemyHit = recepient;
       
       List<EnemyClass> thingsActuallyHittable = new List<EnemyClass>();
        int firstEnemyHitIndex = 0;
       for(int i= 0;i<thingsHit.Length; i++)
       {
            Collider c = thingsHit[i];
            if (c.gameObject.GetComponent<IHittable>() != null)
            {
                thingsActuallyHittable.Add(c.gameObject.GetComponent<EnemyClass>());
            }
       }

        thingsActuallyHittable.TrimExcess();
        for (int i = 0; i < thingsActuallyHittable.Count; i++)
        { 
               if(thingsActuallyHittable[i] == firstEnemyHit)
                {
                    firstEnemyHitIndex = i;
                }
        }
        
        if (thingsActuallyHittable.Count < 2)
            return;

        int index = firstEnemyHitIndex;
        if(index +1 < thingsActuallyHittable.Count-1)
        {
            index++;
        }
        else if( index -1 >= 0)
        {
            index--;
        }

        EnemyClass selected2 = thingsActuallyHittable[index];
        CreateLightning(firstEnemyHit, selected2);
    }
    public void CreateLightning(EnemyClass one,EnemyClass two)
    {
        HitInfo lightningHit = new HitInfo();
        lightningHit.DamageStats = DamageStats;
        //lightningHit.sender = playerInfo.sender;
        
        Debug.DrawLine(one.transform.position, two.transform.position,Color.blue,3);
        Debug.Log("Lightning!");
        one.GetComponent<IHittable>().OnHit(lightningHit);
        two.GetComponent<IHittable>().OnHit(lightningHit);






    }
}
