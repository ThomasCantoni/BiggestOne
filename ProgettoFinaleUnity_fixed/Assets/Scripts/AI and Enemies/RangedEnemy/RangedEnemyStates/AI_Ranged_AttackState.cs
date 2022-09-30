using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_AttackState : AI_Ranged_BaseState
{

    // public bool AlreadyAttack { get { return EC.HasAlreadyAttack; } set { EC.HasAlreadyAttack = value; } }
    public AI_Ranged_AttackState(EnemyRanged Owner) : base(Owner)
    {
        
    }
    public override void Enter()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Owner.anim.SetBool("Run", false);
    }

    public override void Exit()
    {
    }

    public override AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public override void Update()
    {
        base.Update();

        if (PlayerIsVisible && PlayerInAttackRange)
        {
           
                if (!PlayerInEscapeRange)
                {
                    AttackPlayer();
                }
                else
                {
                    OwnerStateManager.stateMachine.ChangeState(AiStateId.Escape);
                }

            
        }
        else
        {
            OwnerStateManager.stateMachine.ChangeState(AiStateId.ChasePlayer);
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
            PlayerInAttackRange = true;
            if (distanceFromPlayer <= Owner.attackRange)
            {
                Quaternion look = Quaternion.LookRotation(TowardsPlayerXZ.normalized, Vector3.up);
                Owner.NavMeshAgent.transform.rotation = look;
                Owner.NavMeshAgent.isStopped = true;
               
                    Owner.anim.SetTrigger("Attack");
                   
                    //EC.Invoke(nameof(EC.ResetAttack), EC.timeBetweenAttacks);
                
            }
        }
    }
}
