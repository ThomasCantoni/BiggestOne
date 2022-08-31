using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "ScriptableObjects/ChainableAttacks")]

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
       List<GameObject> thingsActuallyHittable = new List<GameObject>();
       foreach(Collider c in thingsHit)
        {
            if (c.gameObject.GetComponent<IHittable>() != null)
                thingsActuallyHittable.Add(c.gameObject);
        }
        thingsActuallyHittable.TrimExcess();
        if (thingsActuallyHittable.Count == 0)
            return;

        int index = Random.Range(0, thingsActuallyHittable.Count - 1);
        GameObject selected1 = thingsActuallyHittable[index];
        if(index +1 < thingsActuallyHittable.Count)
        {
            index++;
        }
        else
        {
            index--;
        }
        GameObject selected2 = thingsActuallyHittable[index];
        CreateLightning(selected1, selected2);
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
