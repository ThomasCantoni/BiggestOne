using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent 
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public EnemyClass EC;
    public void Start()
    {
        navMeshAgent = EC.NavMeshAgent;
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayer(EC));
        stateMachine.RegisterState(new AiAttackState(EC));
        stateMachine.RegisterState(new AiEscapeState(EC));
        stateMachine.RegisterState(new AiDeathState(EC));
        stateMachine.ChangeState(initialState);
    }

    public AiAgent(EnemyClass Owner)
    {
        this.EC = Owner;
    }

    public void Update()
    {
        stateMachine.Update();
    }
}
