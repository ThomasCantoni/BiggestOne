using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : EnemyClass, IDamager
{


    //public float maxDistance = 1.0f;
    public AI_Ranged_StateManager StateMachineManager;
    
    public DamageStats damage;
    //public LayerMask layerBullet;
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool HasAlreadyAttack;
    public GameObject bullet;
    public LayerMask layerBullet;
    public Transform BulletOffset;
    public float RunFromPlayerRange;
    //public bool PlayerIsVisible { get {
    //        RaycastHit hit;
    //        Vector3 dir = (player.transform.position - offSet.position).normalized;
    //        Ray enemyPosition = new Ray(offSet.position, dir);
    //        if (Physics.SphereCast(enemyPosition, 0.2f, out hit, attackRange, layerBullet.value))
    //        {
    //            return hit.transform.gameObject.layer == 3;
    //        }
    //        return false;
    //    } }
    public DamageStats DamageStats { get { return damage; } set { damage = value; } }

    public override void Start()
    {
        base.Start();
        StateMachineManager = new AI_Ranged_StateManager(this);
       
        StateMachineManager.Start();
        OnEnemyDeath += () => StateMachineManager.stateMachine.ChangeState(AiStateId.Death);
    }

    private void Update()
    {
        StateMachineManager.Update();
        
    }
    public void InstantiateBullet()
    {
        Instantiate(bullet, BulletOffset.position, Quaternion.LookRotation((player.transform.position - BulletOffset.position).normalized));
    }
    public override void ResetAttack()
    {
        HasAlreadyAttack = false;
    }
    public override void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        OnEnemyDeathParam?.Invoke(this);

        if (!IsInSpawnQueue)
            Destroy(NavMeshAgent.gameObject, 3f);
        else
        {
            disableGO_Timer = new SimpleTimer(3000);
            disableGO_Timer.TimerCompleteEvent += () => NavMeshAgent.gameObject.SetActive(false);
            disableGO_Timer.StartTimer();
        }
    }
    private void DestroyEnemy()
    {
        NavMeshAgent.isStopped = true;
        Destroy(this.gameObject, 4f);
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, runFromPlayerRange);
    //}
}