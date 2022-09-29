using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiEscapeState : AiAgent, AiState
{
    public Transform player;
    public AiEscapeState(EnemyClass Owner) : base(Owner)
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
