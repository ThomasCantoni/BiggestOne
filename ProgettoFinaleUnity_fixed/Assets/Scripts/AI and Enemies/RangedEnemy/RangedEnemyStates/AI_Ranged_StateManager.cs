using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Ranged_StateManager : AiAgent
{
    public EnemyRanged Owner;
    public Transform Player;


    public float RunFromPlayerRange
    {
        get { return Owner.RunFromPlayerRange; }
    }
    public bool PlayerInEscapeRange;
    public AI_Ranged_StateManager(EnemyRanged Owner) : base(Owner)
    {
        this.Owner = Owner;
        Player = Owner.player.transform;
      
    }
    public override void Start()
    {
        initialState = AiStateId.ChasePlayer;
        base.Start();
        stateMachine.RegisterState(new AI_Ranged_BaseState(Owner));
        stateMachine.RegisterState(new AI_Ranged_ChasePlayer(Owner));
        stateMachine.RegisterState(new AI_Ranged_AttackState(Owner));
        stateMachine.RegisterState(new AI_Ranged_EscapeState(Owner));
        stateMachine.RegisterState(new AI_Ranged_DeathState(Owner));
        
    }

   

}
