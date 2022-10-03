using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWeaponBuffPickUp : GenericPickUp
{
    public PlayerAttackEffects PAE;
    public WeaponBuff ToApply;
    public bool Temporary;
    public SimpleTimer RemoveTimer;
    public void Start()
    {
        OnPlayerPickedUp += ApplyEffect;
    }
    public void ApplyEffect(FirstPersonController FPS)
    {
        if (FPS != null)
        {
            PAE = FPS.GetComponent<PlayerAttackEffects>();
            PAE.Add(ToApply);
            OnPickUpUnityEvent?.Invoke();
            if(Temporary)
            {
                RemoveTimer.TimerCompleteEvent += RemoveAfter;
                RemoveTimer.StartTimer();

            }
        }

    }

    public void RemoveAfter()
    {
        PAE.Remove(ToApply);
        Destroy(this.gameObject);
    }
}
