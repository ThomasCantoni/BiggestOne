using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    Slider HP_Slider;
    private float hp_Value = 100f;
    private float maxHp = 100f;


    private void Start()
    {
        HP_Slider = GetComponentInChildren<Slider>();
    }
    public float HP_Value
    {
        get
        {
            return hp_Value;
        }
        set
        {
            hp_Value = Mathf.Clamp(value, -maxHp, maxHp);
            HP_Slider.GetComponent<Slider>().value = hp_Value;
        }
    }
    public void DetuctHealth(IHittableInformation info)
    {
        HP_Value -= info.Damage;
        if (hp_Value <= 0)
            EnemyDeath();
    }
    void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
