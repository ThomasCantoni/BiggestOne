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
    public SimpleTimer HealthRegenTime;
    public Repeater healthRegenRepeater;
    public float StartRegeneratingAfterSECONDS=0,GetToFullHPTimeSECONDS;
    private float hp_regenAmount=0;
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
        HealthRegenTime = new SimpleTimer(StartRegeneratingAfterSECONDS*1000);
        HealthRegenTime.TimerCompleteEvent += AssessRepeater;
        healthRegenRepeater = new Repeater();
        healthRegenRepeater.Frequency = 25;
        healthRegenRepeater.RepeaterTickEvent += () => HP_Value += hp_regenAmount;

    }
    
    private void AssessRepeater()
    {
        if(hp_Value < maxHp)
        {
            hp_regenAmount = (maxHp - hp_Value) / GetToFullHPTimeSECONDS/healthRegenRepeater.Frequency;
            healthRegenRepeater.StartRepeater();

        }
    }
    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }
    public void DetuctHealth(HitInfo info)
    {
        HP_Value -= info.DamageStats.Damage;
        healthRegenRepeater.StopRepeater();
        HealthRegenTime.StartTimer();
        if (hp_Value <= 0)
            PlayerDeath();
    }
}
