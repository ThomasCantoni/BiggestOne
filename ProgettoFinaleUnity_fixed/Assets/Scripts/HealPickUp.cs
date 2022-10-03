using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickUp : GenericPickUp
{
    public override void OnTriggerEnter(Collider other)
    {
        HealthPlayer HP = other.GetComponent<HealthPlayer>();
        if (HP.HP_Value <= 100)
        {
            HP.HP_Value += HP.maxHp * 0.25f;
            Destroy(this.gameObject);
        }
    }
}
