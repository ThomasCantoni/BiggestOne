using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GenericGun> List = new List<GenericGun>();
    public int CurrentIndex;
    public InputCooker InputCooker;
    // Start is called before the first frame update
    public void Start()
    {
        InputCooker = GetComponentInParent<InputCooker>();
        InputCooker.NextWeapon += () =>
        {
            if (CurrentIndex + 1 >= List.Count)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex++;

            }
            Debug.Log("Index ++");

        };


        InputCooker.PreviousWeapon += () =>
        {
            if (CurrentIndex - 1 < 0)
            {
                CurrentIndex = List.Count - 1;
            }
            else
            {
                CurrentIndex--;

            }
            Debug.Log("Index --");
        };
    }



}
