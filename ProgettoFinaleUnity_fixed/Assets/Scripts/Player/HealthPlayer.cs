using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour, IHittable
{
    public Slider HP_Slider;
    private float hp_Value = 100f;
    public readonly float maxHp = 100f;
    public delegate void PlayerDeathEvent();
    public event PlayerDeathEvent OnPlayerDeath;
    public float HP_Value
    {
        get
        {
            return hp_Value;
        }
        set
        {
            hp_Value = Mathf.Clamp(value, -maxHp, maxHp);
            HP_Slider.value = hp_Value;
        }
    }
    public MonoBehaviour Mono { get { return this; } }

    public void OnHit(HitInfo info)
    {
        DetuctHealth(info);
    }
    


    // Start is called before the first frame update
    void Start()
    {
        HP_Slider = GetComponentInChildren<Slider>(true);
    }

    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }
    public void DetuctHealth(HitInfo info)
    {
        HP_Value -= info.DamageStats.Damage;
        if (hp_Value <= 0)
            PlayerDeath();
    }
}
