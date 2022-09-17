using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeAI : EnemyClass, IDamager
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform offSet;
    public GameObject ToInstantiate;
    public LayerMask layer;
    public LayerMask layerBullet;
    public DamageStats damage;
    public float distanceFromPlayer;
    public float walkPointRange;
    public Vector3 walkPoint;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //States
    public float attackRange;
    public float runFromPlayerRange;
    public bool playerInAttackRange;
    public bool RunFromPlayerRange;
    private bool walkPointSet;

    public bool PlayerIsVisible { get {
            RaycastHit hit;
            Vector3 dir = (player.position - offSet.position).normalized;
            Ray enemyPosition = new Ray(offSet.position, dir);
            if (Physics.SphereCast(enemyPosition, 0.2f, out hit, attackRange, layerBullet.value))
            {
                return hit.transform.gameObject.layer == 3;
            }
            return false;
        } }
    public DamageStats DamageStats { get { return damage; } set { damage = value; } }

    private void Awake()
    {
        player = GameObject.Find("Player 2.0").transform;
    }

    private void Update()
    {
        //Check for sight and attack range
        AttackPlayer();
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, layer);
        RunFromPlayerRange = Physics.CheckSphere(transform.position, runFromPlayerRange, layer);
    }

    private void AttackPlayer()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        Vector3 dir = (player.position - offSet.position).normalized;
        //Ray enemyPosition = new Ray(offSet.position, dir);
        //RaycastHit info;
        if (distanceFromPlayer <= attackRange && distanceFromPlayer > runFromPlayerRange && PlayerIsVisible)
        {
            playerInAttackRange = true;
            if (playerInAttackRange)
            {
                Debug.DrawLine(offSet.position, player.position, Color.red, 1f);
                agent.isStopped = true;
                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                    Instantiate(ToInstantiate, offSet.position, Quaternion.LookRotation(dir, Vector3.up));
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }
        else if (distanceFromPlayer >= attackRange && !PlayerIsVisible)
        {
            agent.SetDestination(player.position);
            playerInAttackRange = false;
            agent.isStopped = false;
            agent.speed = 2f;
        }

        if (distanceFromPlayer <= runFromPlayerRange && RunFromPlayerRange && PlayerIsVisible)
        {
            SearchWalkPoint();
            playerInAttackRange = false;
        }
    }
    private void SearchWalkPoint()
    {
        agent.isStopped = false;
        walkPoint = this.transform.position + (this.transform.position - player.position).normalized;
        NavMeshHit meshHit;
        if (NavMesh.SamplePosition(walkPoint, out meshHit, runFromPlayerRange, 1))
        {
            walkPoint = meshHit.position;
        }
        agent.speed = 4f;
        agent.SetDestination(walkPoint);
        Debug.DrawLine(this.transform.position, walkPoint, Color.green, 3f);
        if (Physics.Raycast(walkPoint, -transform.up, runFromPlayerRange, 1))
            walkPointSet = true;
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void DestroyEnemy()
    {
        agent.isStopped = true;
        Destroy(this.gameObject, 4f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runFromPlayerRange);
    }
}