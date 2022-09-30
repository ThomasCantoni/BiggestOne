using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_BaseState : AiState
{
    public Transform Player;
    public float distanceFromPlayer;
    public EnemyRanged Owner;
    public AI_Ranged_StateManager OwnerStateManager;
    public bool PlayerInAttackRange;
    public bool HasAlreadyAttack = false;
    public Vector3 TowardsPlayer;
    public Vector3 TowardsPlayerXZ;
    public bool PlayerIsVisible;
    public float RunFromPlayerRange
    {
        get { return Owner.RunFromPlayerRange; }
    }
    public bool PlayerInEscapeRange;
    public AI_Ranged_BaseState(EnemyRanged Owner)
    {
        this.Owner = Owner;
        OwnerStateManager = Owner.StateMachineManager;
        Player = Owner.player.transform;
    }
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual AiStateId GetId()
    {
        return AiStateId.BaseState;
    }

    public virtual void Update()
    {
        TowardsPlayer = (Player.transform.position - Owner.transform.position);
        TowardsPlayerXZ = TowardsPlayer;
        TowardsPlayerXZ.y = 0;
        distanceFromPlayer = Vector3.Distance(Owner.transform.position, Player.position);
        PlayerInAttackRange = distanceFromPlayer <= Owner.attackRange;
        PlayerInEscapeRange = (distanceFromPlayer <= RunFromPlayerRange);
        PlayerIsVisible = Owner.PlayerIsVisible;
    }
}
