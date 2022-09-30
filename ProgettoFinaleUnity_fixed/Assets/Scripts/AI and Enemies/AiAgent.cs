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
    public virtual void Start()
    {
        navMeshAgent = EC.NavMeshAgent;
        stateMachine = new AiStateMachine(this);
        
        stateMachine.ChangeState(initialState);
    }

    public AiAgent(EnemyClass Owner)
    {
        this.EC = Owner;
    }

    public virtual void Update()
    {
        stateMachine.Update();
    }
}
