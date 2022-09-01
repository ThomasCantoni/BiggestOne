using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "ScriptableObjects/Lightning")]

public class Lightning : ChainableAttack
{
    public float Radius = 5f;
    public float Damage;
    public LayerMask LayersToHit;

    private IHittableInformation playerInfo;
    public override void Apply(IHittableInformation info)
    {
        playerInfo = info;
       Collider[] thingsHit =  Physics.OverlapSphere(info.raycastInfo.point,Radius,LayersToHit);
       GameObject firstEnemyHit = info.raycastInfo.collider.gameObject;
       
       List<GameObject> thingsActuallyHittable = new List<GameObject>();
        int firstEnemyHitIndex = 0;
       for(int i= 0;i<thingsHit.Length; i++)
        {
            Collider c = thingsHit[i];
            if (c.gameObject.GetComponent<IHittable>() != null)
            {
                thingsActuallyHittable.Add(c.gameObject);
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

        GameObject selected2 = thingsActuallyHittable[index];
        CreateLightning(firstEnemyHit, selected2);
    }
    public void CreateLightning(GameObject one,GameObject two)
    {
        IHittableInformation lightningHit = new IHittableInformation();
        lightningHit.Damage = Damage;
        lightningHit.sender = playerInfo.sender;
        
        Debug.DrawLine(one.transform.position, two.transform.position,Color.blue,3);
        Debug.Log("Lightning!");
        one.GetComponent<IHittable>().OnHit(lightningHit);
        two.GetComponent<IHittable>().OnHit(lightningHit);



    }
}
