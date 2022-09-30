using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_DeathState : AI_Ranged_BaseState, AiState
{
    public AI_Ranged_DeathState(EnemyRanged Owner) : base(Owner)
    {

    }
    public override void Enter()
    {
        Owner.GetComponent<CapsuleCollider>().enabled = false;
        Owner.NavMeshAgent.isStopped = true;
        Owner.anim.SetBool("Run", false);
        Owner.anim.SetTrigger("Death");
    }

    public override void Exit()
    {
    }

    public override AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public override void Update()
    {
        
    }
    
}
