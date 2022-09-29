using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;


public abstract class EnemyClass : MonoBehaviour,IKillable
{
    public NavMeshAgent NavMeshAgent;
    public AiAgent agent;
    public GameObject player;
    public FirstPersonController FPS_Controller;
    Slider HP_Slider;
    private float hp_Value = 100f;
    private float maxHp = 100f;
    public Transform offSet;
    //public bool alreadyAttacked;
    public float timeBetweenAttacks;
    public GameObject bullet;
    public float maxDistance = 1.0f;
    public float attackRange;
    public bool PlayerInAttackRange;
    public LayerMask layerBullet;
    public Animator anim;
    public bool HasAlreadyAttack = false;

    public bool PlayerIsVisible
    {
        get
        {
            RaycastHit hit;
            Vector3 dir = (player.transform.position - offSet.position).normalized;
            Ray enemyPosition = new Ray(offSet.position, dir);
            if (Physics.SphereCast(enemyPosition, 0.2f, out hit, attackRange, layerBullet.value))
            {
                return hit.transform.gameObject.layer == 3;
            }
            return false;
        }
    }

    public MonoBehaviour Mono
    {
        get { return this; }
    }
    public IKillable.OnDeathEvent OnEnemyDeath;
    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public virtual void Start()
    {
        HP_Slider = GetComponentInChildren<Slider>(true);
        NavMeshAgent = GetComponentInParent<NavMeshAgent>();
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
            {
                OnDeath(info);
                //AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
                //agent.stateMachine.ChangeState(AiStateId.Death);
            }
        }
    }
    public virtual void ResetAttack()
    {
        HasAlreadyAttack = false;
    }

    public virtual void OnHit(HitInfo info)
    {
        DetuctHealth(info);
    }
    public virtual void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        Destroy(NavMeshAgent.gameObject);
    }
    public virtual void OnDeath(HitInfo info)
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


