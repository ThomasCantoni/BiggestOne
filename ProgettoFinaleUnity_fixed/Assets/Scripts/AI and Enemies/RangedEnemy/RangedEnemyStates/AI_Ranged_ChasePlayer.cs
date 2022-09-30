using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Ranged_ChasePlayer : AI_Ranged_BaseState
{
   
    //public float attackRange;
    public AI_Ranged_ChasePlayer(EnemyRanged Owner) : base(Owner)
    {
        
    }
    public override AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }
    public override void Enter()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Owner.NavMeshAgent.isStopped = false;
        Owner.anim.SetBool("Run", true);
    }
    public override void Exit()
    {
    }
    public override void Update()
    {
        base.Update();
        Owner.NavMeshAgent.destination = Player.transform.position;
        
        
        
        //if (agent.EC.NavMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
        //{
        //    agent.EC.NavMeshAgent.destination = player.transform.position;
        //}
        CheckForAttack();
    }
    public void CheckForAttack()
    {
       
        if (PlayerInAttackRange && Owner.PlayerIsVisible)
        {
            
            Owner.StateMachineManager.stateMachine.ChangeState(AiStateId.Attack);
        }
    }
}
