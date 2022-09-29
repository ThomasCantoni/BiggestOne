using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Ranged_ChasePlayer : AI_Ranged_StateAgent, AiState
{
   
    //public float attackRange;
    public AI_Ranged_ChasePlayer(EnemyRanged Owner) : base(Owner)
    {
        
    }
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    public void Enter(AiAgent agent)
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Owner.NavMeshAgent.isStopped = false;
        Owner.anim.SetBool("Run", true);
    }
    public void Exit(AiAgent agent)
    {
    }
    public void Update(AiAgent agent)
    {
        base.UpdateVariables();
        agent.EC.NavMeshAgent.destination = Player.transform.position;
        
        
        
        //if (agent.EC.NavMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        //{
        //    agent.EC.NavMeshAgent.destination = player.transform.position;
        //}
        CheckForAttack();
    }
    public void CheckForAttack()
    {
       
        if (PlayerInAttackRange && EC.PlayerIsVisible)
        {
            
            Owner.agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }
}
