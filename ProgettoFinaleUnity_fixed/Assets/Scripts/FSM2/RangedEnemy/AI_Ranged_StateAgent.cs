using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_StateAgent : AiAgent
{
    public EnemyRanged Owner;
    public Transform Player;
    public float distanceFromPlayer;

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
    public AI_Ranged_StateAgent(EnemyRanged Owner) : base(Owner)
    {
        this.Owner = Owner;
        Player = Owner.player.transform;
      
    }
    public override void Start()
    {
        initialState = AiStateId.ChasePlayer;
        base.Start();
        stateMachine.RegisterState(new AI_Ranged_ChasePlayer(Owner));
        stateMachine.RegisterState(new AI_Ranged_AttackState(Owner));
        stateMachine.RegisterState(new AI_Ranged_EscapeState(Owner));
        stateMachine.RegisterState(new AI_Ranged_DeathState(Owner));
        
    }

    public void UpdateVariables()
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
