using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;


public abstract class EnemyClass : MonoBehaviour,IKillable
{
    public NavMeshAgent NavMeshAgent;
    //public AiAgent agent;
    public GameObject player;
    public FirstPersonController FPS_Controller;
    Slider HP_Slider;
    private float hp_Value = 100f;
    private float maxHp = 100f;
    public bool IsInSpawnQueue = false;
    protected  SimpleTimer disableGO_Timer;

    public float MaxHP { 
        get
        {
            return maxHp;
        }
    }
    public Animator anim;
    public Transform Eyes;
    public float PlayerDetectDistance;
    public LayerMask PlayerDetectMask;
    public float attackRange;
    public bool PlayerInAttackRange;
    public bool PlayerIsVisible
    {
        get
        {
            RaycastHit hit;
            Vector3 dir = (player.transform.position - Eyes.position).normalized;
            Ray enemyPosition = new Ray(Eyes.position, dir);
            if (Physics.SphereCast(enemyPosition, 0.2f, out hit, PlayerDetectDistance, PlayerDetectMask))
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
    public IKillable.OnDeathEventParameter OnEnemyDeathParam;

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
       
    }

    public virtual void OnHit(HitInfo info)
    {
        DetuctHealth(info);
    }
    public virtual void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        OnEnemyDeathParam?.Invoke(this);
        if (!IsInSpawnQueue)
            Destroy(NavMeshAgent.gameObject);
        else
        {
            disableGO_Timer = new SimpleTimer(3000);
            disableGO_Timer.TimerCompleteEvent += () => NavMeshAgent.gameObject.SetActive(false);
            disableGO_Timer.StartTimer();
        }
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


