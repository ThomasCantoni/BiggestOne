using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiAgent, AiState
{
    public AiDeathState(EnemyClass Owner) : base(Owner)
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
