using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_EscapeState : AiAgent, AiState
{
    public Transform player;
    public AI_Ranged_EscapeState(EnemyClass Owner) : base(Owner)
    {

    }
    public void Enter(AiAgent agent)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.Escape;

    }

    public void Update(AiAgent agent)
    {
    }
}
