using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;


public abstract class EnemyClass : MonoBehaviour,IKillable
{
    
    NavMeshAgent Enemy;
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
        Enemy = GetComponentInParent<NavMeshAgent>();
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
    public bool IsDead
    {
        get { return HP_Value <= 0; }
    }

    public IKillable.OnDeathEvent deathEvent { get { return OnEnemyDeath; } set { OnEnemyDeath = value; } }
    public virtual void DetuctHealth(HitInfo info)
    {
        if (!IsDead)
        {
            HP_Value -= info.DamageStats.Damage;
            if (IsDead)
                OnDeath();
        }
    }

    public virtual void OnHit(HitInfo info)
    {
        DetuctHealth(info);
    }

    public void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        Destroy(Enemy.gameObject);
    }
}


