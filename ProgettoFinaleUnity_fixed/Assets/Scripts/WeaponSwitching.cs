using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public InputCooker InputCooker;
    public delegate void ReloadDelegate();
    public event ReloadDelegate ReloadEvent;

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
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
    public void LaunchReloadEvent()
    {
        ReloadEvent.Invoke();
    }
    void Switch()
    {
        int prevWeapon = selectedWeapon;

        if (InputCooker.Controls.Player.WeaponScroll.ReadValue<float>() > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        };

        if (InputCooker.Controls.Player.WeaponScroll.ReadValue<float>() < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        };

        if (prevWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
}
