using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackState : AiAgent, AiState
{
    public Transform player;


    public float distanceFromPlayer;
    public bool RunFromPlayer;
    public float runFromPlayerRange;
    public bool AlreadyAttack { get { return EC.HasAlreadyAttack; } set { EC.HasAlreadyAttack = value; } }
    public AiAttackState(EnemyClass Owner) : base(Owner)
    {
        
    }
    public void Enter(AiAgent agent)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        EC.anim.SetBool("Run", false);
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Update(AiAgent agent)
    {
        EC.PlayerInAttackRange = Physics.CheckSphere(EC.transform.position, EC.attackRange, 8);
        RunFromPlayer = Physics.CheckSphere(EC.transform.position, runFromPlayerRange, 8);
        if (EC.PlayerInAttackRange)
        {
            if (!RunFromPlayer)
            {
                AttackPlayer();
            }
            else
            {
                EC.agent.stateMachine.ChangeState(AiStateId.Escape);
            }
        }
        else
        {
            EC.agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
        
        //PlayerInAttackRange = true;
        //if (distanceFromPlayer <= attackRange)
        //{
        //    Quaternion look = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);
        //    agent.navMeshAgent.transform.rotation = look;
        //    agent.navMeshAgent.isStopped = true;
        //    if (!EC.alreadyAttacked)
        //    {
        //        EC.alreadyAttacked = true;
        //        GameObject.Instantiate(EC.bullet, EC.offSet.position, Quaternion.LookRotation(dir, Vector3.up));
        //        EC.Invoke(nameof(ResetAttack), EC.timeBetweenAttacks);
        //    }
        //}
    }
    public void AttackPlayer()
    {
        
        distanceFromPlayer = Vector3.Distance(EC.transform.position, player.transform.position);
        Vector3 dir = (player.transform.position - EC.offSet.position).normalized;

        if (distanceFromPlayer <= EC.attackRange && EC.PlayerIsVisible)
        {
            EC.PlayerInAttackRange = true;
            if (distanceFromPlayer <= EC.attackRange)
            {
                Quaternion look = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z), Vector3.up);
                EC.NavMeshAgent.transform.rotation = look;
                EC.NavMeshAgent.isStopped = true;
                if (!AlreadyAttack)
                {
                    EC.anim.SetTrigger("Attack");
                    AlreadyAttack = true;
                    GameObject.Instantiate(EC.bullet, EC.offSet.position, Quaternion.LookRotation(dir, Vector3.up));
                    //EC.Invoke(nameof(EC.ResetAttack), EC.timeBetweenAttacks);
                }
            }
        }
    }
}
