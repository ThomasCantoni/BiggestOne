using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    BaseState,
    ChasePlayer,
    Attack,
    Escape,
    Stun,
    Death
}
public interface AiState
{
    public abstract AiStateId GetId();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
