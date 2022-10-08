using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public InputCooker InputCooker;
    public delegate void WeaponEvent();
    public delegate void GenericGunEvent(GenericGun gun);
    public event WeaponEvent ReloadEvent;
    public UIManager UIM;
    public event GenericGunEvent ReloadDelegateEvent, ChangeWeaponEvent;
    public GenericGunEvent GunEquippedEvent,GunUnequippedEvent;
    public GenericGun currentGun;
    public List<GenericGun> List = new List<GenericGun>();

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        InputCooker.NextWeapon += SelectWeaponIndex;
        InputCooker.PreviousWeapon += SelectWeaponIndex;
    }

   
    //void Update()
    //{
    //    Switch();
    //}
    public void SelectWeaponIndex(int additiveIndex)
    {
        if (selectedWeapon + additiveIndex >= List.Count)
            return;
        if(additiveIndex <0)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = List.Count - 1;
            else
                selectedWeapon--;
        }
        else
        {
            if (selectedWeapon >= List.Count - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        SelectWeapon();

    }
    public void SelectWeaponClass(GenericGun selected)
    {
        selectedWeapon = List.IndexOf(selected);
        SelectWeapon();
    }
    void SelectWeapon()
    {
        
        int i = 0;
        foreach (GenericGun weapon in List)
        {
            if (i == selectedWeapon)
            {
                currentGun = weapon;
                weapon.gameObject.SetActive(true);
                ChangeWeaponEvent?.Invoke(currentGun);
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    //animation event!
    public void LaunchReloadEvent()
    {
        ReloadEvent?.Invoke();
        ReloadDelegateEvent?.Invoke(currentGun);
    }
    public void AddGun(GenericGun gunToAdd,bool switchImmediately=false)
    {
        if (!List.Contains(gunToAdd))
        {
            List.Add(gunToAdd);
            if (switchImmediately || List.Count == 1)
            {
                SelectWeaponClass(gunToAdd);

            }
        }
    }
    public void RemoveGun(GenericGun gunToRemove)
    {
        if (List.Contains(gunToRemove))
        {
            int currentGunIndex = List.IndexOf(gunToRemove);
            List.Remove(gunToRemove);

            List<GenericGun> newList = new List<GenericGun>();
            for (int i = 0; i < List.Count; i++)
            {
                if(List[i] != null)
                {
                    newList.Add(List[i]);
                }
            }
            newList.TrimExcess();
            List = newList;
            if(currentGunIndex == selectedWeapon)
            {
                if(List.Count > 0)
                {
                    if(selectedWeapon-1 >= 0)
                    {
                        selectedWeapon--;
                        
                    }
                    else if(selectedWeapon +1 < List.Count-1)
                    {
                        selectedWeapon++;
                    }
                }
                else
                {
                    RemoveAllGuns();
                    return;
                }
            }
                SelectWeapon();
            
            
                

           
        }
    }
    public void RemoveAllGuns()
    {
        List.Clear();
        selectedWeapon = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject g = transform.GetChild(i).gameObject;
            g.SetActive(false);
        }
    }
    public void SwitchToGun(GenericGun GunInsideList)
    {

    }
    void Switch()
    {

        int prevWeapon = selectedWeapon;
        if (InputCooker.Controls.Player.WeaponScroll.ReadValue<float>() > 0f)
        {
            if (selectedWeapon >= List.Count - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        };

        if (InputCooker.Controls.Player.WeaponScroll.ReadValue<float>() < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = List.Count - 1;
            else
                selectedWeapon--;
        };

        if (prevWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
}
