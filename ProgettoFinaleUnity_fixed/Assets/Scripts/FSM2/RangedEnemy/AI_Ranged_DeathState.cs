using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_DeathState : AiAgent, AiState
{
    public AI_Ranged_DeathState(EnemyClass Owner) : base(Owner)
    {

    }
    public void Enter(AiAgent agent)
    {
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Update(AiAgent agent)
    {
        
    }
    
}
