using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_FourLeafClover : GenericPickUp
{
    public PlayerAttackEffects PAE;
    public WeaponBuff ToApply;
    public SimpleTimer RemoveTimer;
    private void Start()
    {
        OnPlayerPickedUp += ApplyEffect;
    }
    public void ApplyEffect(FirstPersonController FPS)
    {
        if (FPS != null)
        {
            PAE = FPS.GetComponent<PlayerAttackEffects>();
            PAE.Add(ToApply);
            RemoveTimer.TimerCompleteEvent += RemoveAfter;
            RemoveTimer.StartTimer();
            OnPickUpUnityEvent?.Invoke();
        }

    }
    
    public void RemoveAfter()
    {
        PAE.Remove(ToApply);
        Destroy(this.gameObject);
    }
}
