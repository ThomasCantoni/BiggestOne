using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;


public abstract class EnemyClass : MonoBehaviour,IKillable
{
    

    NavMeshAgent Enemy;
    public GameObject player;
    public FirstPersonController FPS_Controller;
    Slider HP_Slider;
    private float hp_Value = 100f;
    private float maxHp = 100f;

    public MonoBehaviour Mono
    {
        get { return this; }
    }
    public event IKillable.OnDeathEvent OnEnemyDeath;
    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Start()
    {
        HP_Slider = GetComponentInChildren<Slider>(true);
        Enemy = GetComponentInParent<NavMeshAgent>();
       
        FPS_Controller = player.GetComponent<FirstPersonController>();
        
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
                OnDeath(info);
        }
    }

    public virtual void OnHit(HitInfo info)
    {
        DetuctHealth(info);
    }
    public virtual void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        Destroy(Enemy.gameObject);
    }
    public void OnDeath(HitInfo info)
    {
        if(info.SourceDamageInstance != null)
        {

            if(info.IsChainableAttack)
            {
                FirstPersonController tryget =(FirstPersonController) info.SourceDamageInstance.DamageSource.Mono;
                tryget.PlayerKilledSomething?.Invoke(tryget, this);
            }
            else
            {
                GenericGun tryget = info.SourceDamageInstance.DamageSource.Mono.GetComponent<GenericGun>();
                if (tryget != null)
                    tryget.WeaponKilledSomething?.Invoke(tryget, this);

            }
        }
        OnDeath();
    }
}


