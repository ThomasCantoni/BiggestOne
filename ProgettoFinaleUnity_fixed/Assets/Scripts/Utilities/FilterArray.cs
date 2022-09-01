using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FilterArray 
{
    public static List<T> FilterOverlapArrayIntoList<T,TTwo>(IList<TTwo> coll) where TTwo:Collider
    {
        //Collider[] thingsHit = Physics.OverlapSphere(this.transform.position, Radius, 3);
        List<T> filteredList = new List<T>(coll.Count);
        for (int i = 0; i < coll.Count; i++)
        {
            T tryGetHittable = coll[i].GetComponent<T>();
            if(tryGetHittable != null)
            {
                filteredList.Add(tryGetHittable);

            }

            
        }
        filteredList.TrimExcess();
        return filteredList;
    }
}
