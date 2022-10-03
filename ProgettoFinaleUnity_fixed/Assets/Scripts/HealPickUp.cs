using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickUp : GenericPickUp
{
    public float UpdateHealth;
    HealthPlayer HP;
    private void Start()
    {
        UpdateHealth = HP.HP_Value / 2f;
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (HP.HP_Value < 100)
        {
            HP.HP_Value += UpdateHealth;
        }
        Destroy(this.gameObject);
    }
}
