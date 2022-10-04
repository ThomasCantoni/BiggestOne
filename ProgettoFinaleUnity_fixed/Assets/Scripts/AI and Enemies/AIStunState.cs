using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStunState : AiState
{
    EnemyClass Owner;
    public AIStunState(EnemyClass Owner) { }
    
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Stun;
    }

    public void Update()
    {
    }

}
