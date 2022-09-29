using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_AttackState : AI_Ranged_StateAgent, AiState
{

    // public bool AlreadyAttack { get { return EC.HasAlreadyAttack; } set { EC.HasAlreadyAttack = value; } }
    public AI_Ranged_AttackState(EnemyRanged Owner) : base(Owner)
    {
        
    }
    public void Enter(AiAgent agent)
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Owner.anim.SetBool("Run", false);
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
        base.UpdateVariables();

        if (PlayerIsVisible && PlayerInAttackRange)
        {
           
                if (!PlayerInEscapeRange)
                {
                    AttackPlayer();
                }
                else
                {
                    //Owner.agent.stateMachine.ChangeState(AiStateId.Escape);
                }

            
        }
        else
        {
            Owner.agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
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


        if (PlayerIsVisible)
        {
            EC.PlayerInAttackRange = true;
            if (distanceFromPlayer <= EC.attackRange)
            {
                Quaternion look = Quaternion.LookRotation(TowardsPlayerXZ.normalized, Vector3.up);
                EC.NavMeshAgent.transform.rotation = look;
                EC.NavMeshAgent.isStopped = true;
               
                    EC.anim.SetTrigger("Attack");
                   
                    //EC.Invoke(nameof(EC.ResetAttack), EC.timeBetweenAttacks);
                
            }
        }
    }
}
