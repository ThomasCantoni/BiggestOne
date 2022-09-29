using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : EnemyClass, IDamager
{


    //public float maxDistance = 1.0f;
    public AI_Ranged_StateAgent agent;
    public LayerMask LayerOfPlayer;
    public DamageStats damage;
    //public LayerMask layerBullet;
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool HasAlreadyAttack;
    public GameObject bullet;
    public LayerMask layerBullet;
    public float RunFromPlayerRange;
    //public bool PlayerIsVisible { get {
    //        RaycastHit hit;
    //        Vector3 dir = (player.transform.position - offSet.position).normalized;
    //        Ray enemyPosition = new Ray(offSet.position, dir);
    //        if (Physics.SphereCast(enemyPosition, 0.2f, out hit, attackRange, layerBullet.value))
    //        {
    //            return hit.transform.gameObject.layer == 3;
    //        }
    //        return false;
    //    } }
    public DamageStats DamageStats { get { return damage; } set { damage = value; } }

    public override void Start()
    {
        base.Start();
        agent = new AI_Ranged_StateAgent(this);
       
        agent.Start();
    }

    private void Update()
    {
        agent.Update();
        //Check for sight and attack range
        //AttackPlayer();
        //PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, layer);
        //RunFromPlayerRange = Physics.CheckSphere(transform.position, runFromPlayerRange, layer);
    }

    private void AttackPlayer()
    {
        agent.stateMachine.ChangeState(AiStateId.Attack);
        //distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        //Vector3 dir = (player.transform.position - offSet.position).normalized;
        //if (distanceFromPlayer <= attackRange && distanceFromPlayer > runFromPlayerRange && PlayerIsVisible)
        //{
        //    anim.SetTrigger("Attack");
        //    anim.SetBool("Run", false);
        //}

        //if (distanceFromPlayer <= attackRange && distanceFromPlayer > runFromPlayerRange && PlayerIsVisible)
        //{
        //    PlayerInAttackRange = true;
        //    if (distanceFromPlayer <= attackRange)
        //    {
        //        Quaternion look = Quaternion.LookRotation(new Vector3(dir.x,0,dir.z), Vector3.up);
        //        NavMeshAgent.transform.rotation = look;
        //        NavMeshAgent.isStopped = true;
        //        if (!alreadyAttacked)
        //        {
        //            alreadyAttacked = true;
        //            //transform.LookAt(player.transform.position);
        //            Instantiate(ToInstantiate, offSet.position, Quaternion.LookRotation(dir, Vector3.up));
        //            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        //        }
        //    }
        //}
        //else if (distanceFromPlayer >= attackRange && !PlayerIsVisible)
        //{
        //    NavMeshAgent.SetDestination(player.transform.position);
        //    PlayerInAttackRange = false;
        //    NavMeshAgent.isStopped = false;
        //    NavMeshAgent.speed = 2f;
        //    anim.SetBool("Run", true);
        //    //Vector3 newPosition = agent.nextPosition - agent.transform.position;
        //    //newPosition.y = 0;
        //    //agent.transform.rotation = Quaternion.LookRotation(newPosition.normalized, Vector3.up);
        //}

        //if (distanceFromPlayer <= runFromPlayerRange && RunFromPlayerRange && PlayerIsVisible)
        //{
        //    SearchWalkPoint();
        //    PlayerInAttackRange = false;
        //    anim.SetBool("Run", true);
        //}
    }
    public NavMeshPath TryGo(Vector3 dir,float multi,bool local = false)
    {
        if (local)
        {
            Quaternion rot = Quaternion.LookRotation((player.transform.position - this.transform.position).normalized);
            dir = rot * dir;
        }
        Vector3 tryPos = this.transform.position + dir * multi;
        
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(this.transform.position, tryPos, 1, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.DrawRay(this.transform.position, dir, Color.white, 2f);
            return path;
        }
        return null;
        
    }
    private void SearchWalkPoint()
    {
        NavMeshAgent.isStopped = false;
        anim.SetBool("Run", true);
        NavMeshPath newPath;
        Vector3[] directions = { Vector3.back, Vector3.left, Vector3.right, Vector3.forward };
        bool local = true;
        int j = 0;
        for (int i = 0; i < directions.Length * 2f; i++)
        {

            newPath = TryGo(directions[j], 5f,local);
            if (newPath != null)
            {
                NavMeshAgent.SetPath(newPath);
                walkPoint = NavMeshAgent.destination;
                break;
            }
            else if(i == 3)
            {
                local = false;
                j = 0;
                continue;
            }
            j++;
        }
        NavMeshAgent.speed = 4f;
        //agent.SetDestination(walkPoint);
        //Vector3 fsmPosition = this.transform.position;
        //Vector3 fwd = transform.TransformDirection(Vector3.forward); // this

        //if (NavMeshAnalytics.IsNearCorner(fsmPosition, 3))
        //{
        //    agent.SetDestination(walkPoint);
        //    Debug.DrawRay(transform.position, fwd * 10, Color.red);
        //}
        Debug.DrawLine(this.transform.position, NavMeshAgent.destination, Color.green, 3f);
    }



    public override void ResetAttack()
    {
        HasAlreadyAttack = false;
    }
    private void DestroyEnemy()
    {
        NavMeshAgent.isStopped = true;
        Destroy(this.gameObject, 4f);
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, runFromPlayerRange);
    //}
}