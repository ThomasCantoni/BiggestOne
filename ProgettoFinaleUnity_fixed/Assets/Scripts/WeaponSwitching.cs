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
        //InputCooker = GetComponentInParent<InputCooker>();
    }

    // Update is called once per frame
    void Update()
    {
        Switch();
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                currentGun = weapon.GetComponent<GenericGun>();
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
