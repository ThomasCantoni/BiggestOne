using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text ammoText;
    public WeaponSwitching WS;
    private void Start()
    {
        WS.ReloadDelegateEvent += UpdateAmmo;
        WS.ChangeWeaponEvent += UpdateAmmo;
    }
    public void UpdateAmmo(GenericGun gun)
    {
        UpdateAmmo(gun.currentAmmo);
    }
    public void UpdateAmmo(int count)
    {
        ammoText.text = "Ammo:" + count;
    }
}
