using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasePlayer : AiAgent, AiState
{
    public Transform player;
    //public float attackRange;
    public AiChasePlayer(EnemyClass Owner) : base(Owner)
    {

    }
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    public void Enter(AiAgent agent)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        EC.NavMeshAgent.isStopped = false;
        EC.anim.SetBool("Run", true);
    }
    public void Exit(AiAgent agent)
    {
    }
    public void Update(AiAgent agent)
    {
        if (!agent.EC.NavMeshAgent.hasPath)
        {
            agent.EC.NavMeshAgent.destination = player.transform.position;
        }
        Vector3 dir = (player.transform.position - agent.EC.NavMeshAgent.destination);
        dir.y = 0;
        //if (agent.EC.NavMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        //{
        //    agent.EC.NavMeshAgent.destination = player.transform.position;
        //}
        CheckForAttack();
    }
    public void CheckForAttack()
    {
        float distanceFromPlayer = Vector3.Distance(EC.transform.position, player.transform.position);
        //Vector3 dir = (player.transform.position - EC.offSet.position).normalized;
        if (distanceFromPlayer <= EC.attackRange && EC.PlayerIsVisible)
        {
            
            EC.agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }
}
