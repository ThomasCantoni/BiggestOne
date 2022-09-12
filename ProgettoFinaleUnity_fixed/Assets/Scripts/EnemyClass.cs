using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class EnemyClass : MonoBehaviour,IKillable
{
    
    [SerializeField] public int speed;

    Slider HP_Slider;
    private float hp_Value = 100f;
    private float maxHp = 100f;

    public MonoBehaviour Mono
    {
        get { return this; }
    }
    public event IKillable.OnDeathEvent OnEnemyDeath;
    
    public void Start()
    {
        HP_Slider = GetComponentInChildren<Slider>(true);
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
            HP_Slider.value = hp_Value;
        }
    }

    public IKillable.OnDeathEvent deathEvent { get { return OnEnemyDeath; } set { OnEnemyDeath = value; } }

    public virtual void DetuctHealth(HitInfo info)
    {
        HP_Value -= info.DamageStats.Damage;
        if (hp_Value <= 0)
            OnDeath();
    }

    public virtual void OnHit(HitInfo info)
    {
        DetuctHealth(info);
    }

    public void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}


