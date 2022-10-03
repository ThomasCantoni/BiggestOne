using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceX2 : GenericPickUp
{
    public SimpleTimer St;
    public PlayerInvetory PI;
    public override void OnTriggerEnter(Collider other)
    {
        PI = other.GetComponent<PlayerInvetory>();

        if (PI != null)
        {
            OnPickUpUnityEvent?.Invoke();
            PI.mulCoins += 1;
            St.TimerCompleteEvent += RemoveAfter;
            St.StartTimer();
        }
    }
    public void RemoveAfter()
    {
        PI.mulCoins--;
        Destroy(this.gameObject);
    }
}
