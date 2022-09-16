using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeAI : EnemyClass, IDamager
{
    public NavMeshAgent agent;
    public Transform player;
    public GameObject ToInstantiate;
    public LayerMask layer;
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
        playerInAttackRange = Physics.CheckSphere(transform.position, runFromPlayerRange, layer);
    }

    private void AttackPlayer()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
        //if (!walkPointSet) SearchWalkPoint();

        //if (walkPointSet)
        //    agent.SetDestination(walkPoint);

        if (distanceFromPlayer <= attackRange)
        {
            Debug.DrawLine(transform.position, player.position, Color.red, 1f);
            if (!alreadyAttacked)
            {
                
                alreadyAttacked = true;
                HitInfo infoDamage = new HitInfo(this, player.GetComponent<HealthPlayer>());
                Vector3 dir = (player.position - this.transform.position).normalized;
                Ray enemyPosition = new Ray(transform.position, dir);
                RaycastHit info;
                Debug.DrawRay(transform.position, dir, Color.red, 5f);
                if (!Physics.Raycast(enemyPosition, out info, attackRange, 1))
                {
                    if (!Physics.SphereCast(enemyPosition, 0.5f, out info, attackRange, 1))
                    {
                        Instantiate(ToInstantiate, this.transform.position, Quaternion.LookRotation(dir, Vector3.up));
                    }
                }
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
            agent.isStopped = true;
        }
        else
        {
            agent.SetDestination(player.position);
        }
        if (distanceFromPlayer <= runFromPlayerRange)
        {
            SearchWalkPoint();
        }
    }
    private void SearchWalkPoint()
    {
        agent.isStopped = false;
        walkPoint = new Vector3(player.forward.x  ,player.forward.y , player.forward.z );
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