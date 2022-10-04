using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Ranged_EscapeState : AI_Ranged_BaseState
{
    Vector3 walkPoint;
    private bool hasReachedWalkPoint;
    public AI_Ranged_EscapeState(EnemyRanged Owner) : base(Owner)
    {

    }
    public override void Enter()
    {
        base.Enter();

        
            SearchWalkPoint();

    }

    public override void Exit()
    {

    }

    public override AiStateId GetId()
    {
        return AiStateId.Escape;

    }

    public override void Update()
    {
        base.Update();
        hasReachedWalkPoint = Vector3.Distance(Owner.transform.position, Owner.NavMeshAgent.destination) <= 1;
        if(hasReachedWalkPoint)
        {
            if(PlayerInAttackRange && PlayerIsVisible)
            {
                OwnerStateManager.stateMachine.ChangeState(AiStateId.Attack);
            }
            else
            {
                OwnerStateManager.stateMachine.ChangeState(AiStateId.ChasePlayer);
            }
        }
    }
    public NavMeshPath TryGo(Vector3 dir, float multi, bool local = false)
    {
        if (local)
        {
            Quaternion rot = Quaternion.LookRotation((Player.position - Owner.transform.position).normalized);
            dir = rot * dir;
        }
        Vector3 tryPos = Owner.transform.position + dir * multi;

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(Owner.transform.position, tryPos, 1, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.DrawRay(Owner.transform.position, dir, Color.white, 2f);
            return path;
        }
        return null;

    }
    private void SearchWalkPoint()
    {
        Owner.NavMeshAgent.isStopped = false;
        Owner.anim.SetBool("Run", true);
        NavMeshPath newPath;
        Vector3[] directions = { Vector3.back, Vector3.left, Vector3.right, Vector3.forward };
        bool local = true;
        int j = 0;
        for (int i = 0; i < directions.Length * 2f; i++)
        {

            newPath = TryGo(directions[j], 5f, local);
            if (newPath != null)
            {
                Owner.NavMeshAgent.SetPath(newPath);
                walkPoint = Owner.NavMeshAgent.destination;
                break;
            }
            else if (i == 3)
            {
                local = false;
                j = 0;
                continue;
            }
            j++;
        }
        Owner.NavMeshAgent.speed = 4f;
        //Owner.NavMeshAgent.SetDestination(walkPoint);
        //Vector3 fsmPosition = this.transform.position;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward); // this

        //if (NavMeshAnalytics.IsNearCorner(fsmPosition, 3))
        //{
        //    agent.SetDestination(walkPoint);
        //    Debug.DrawRay(transform.position, fwd * 10, Color.red);
        //}
        Debug.DrawLine(Owner.transform.position, Owner.NavMeshAgent.destination, Color.green, 3f);
    }
}
