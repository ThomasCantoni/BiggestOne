using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class EnemyDroneAI : MonoBehaviour, IDamager
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform offSet;
    public Transform offSet2;
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
    public bool PlayerIsVisible
    {
        get
        {
            RaycastHit hit;
            Vector3 dir = (player.position - offSet2.position).normalized;
            Ray enemyPosition = new Ray(offSet2.position, dir);
            if (Physics.SphereCast(enemyPosition, 0.2f, out hit, attackRange, layerBullet.value))
            {
                return hit.transform.gameObject.layer == 3;
            }
            return false;
        }
    }
    public DamageStats DamageStats { get { return damage; } set { damage = value; } }

    private void Awake()
    {
        player = GameObject.Find("Player 2.0").transform;
    }

    private void Update()
    {
        AttackPlayer();
        playerInAttackRange = Physics.CheckSphere(offSet2.position, attackRange, layer);
        RunFromPlayerRange = Physics.CheckSphere(offSet2.position, runFromPlayerRange, layer);
    }

    private void AttackPlayer()
    {
        distanceFromPlayer = Vector3.Distance(offSet2.position, player.position);
        Vector3 dir = (player.position - offSet.position).normalized;
        transform.LookAt(player.position);

        //Ray enemyPosition = new Ray(offSet.position, dir);
        //RaycastHit info;
        if (distanceFromPlayer <= attackRange && distanceFromPlayer > runFromPlayerRange && PlayerIsVisible)
        {
            playerInAttackRange = true;
            if (playerInAttackRange)
            {
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
        //if (distanceFromPlayer <= attackRange && distanceFromPlayer <= runFromPlayerRange && !PlayerIsVisible)
        //{
        //    agent.SetDestination(player.position);
        //}

        if (distanceFromPlayer <= runFromPlayerRange && RunFromPlayerRange && PlayerIsVisible)
        {
            SearchWalkPoint();
            playerInAttackRange = false;
        }
    }
    public NavMeshPath TryGo(Vector3 dir, float multi, bool local = false)
    {
        if (local)
        {
            Quaternion rot = Quaternion.LookRotation((player.transform.position - offSet2.position).normalized);
            dir = rot * dir;
        }
        Vector3 tryPos = offSet2.position + dir * multi;

        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(offSet2.position, tryPos, 1, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            Debug.DrawRay(offSet2.position, dir, Color.white, 2f);
            return path;
        }
        return null;
        
    }
    private void SearchWalkPoint()
    {
        agent.isStopped = false;
        NavMeshPath newPath;
        Vector3[] directions = { Vector3.back, Vector3.left, Vector3.right, Vector3.forward };
        bool local = true;
        int j = 0;
        for (int i = 0; i < directions.Length * 2f; i++)
        {

            newPath = TryGo(directions[j], 5f, local);
            if (newPath != null)
            {
                agent.SetPath(newPath);
                walkPoint = agent.destination;
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
        agent.speed = 4f;
        Debug.DrawLine(this.transform.position, agent.destination, Color.green, 3f);
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
        Gizmos.DrawWireSphere(offSet2.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(offSet2.position, runFromPlayerRange);
    }
}







