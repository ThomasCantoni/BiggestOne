using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    //public Animator anim;
    //public GameObject Pipe;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player 2.0").transform;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

        //anim.SetBool("Run", false);
        agent.speed = 1.5f;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

        //anim.SetBool("Run", false);
        agent.speed = 1.5f;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = 2.2f;
        //anim.SetBool("Attack", false);
        //anim.SetBool("Run", true);
    }


    private void AttackPlayer()
    {
        agent.SetDestination(player.position);
        //anim.SetBool("Attack", true);
        //anim.SetBool("Run", false);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) DestroyEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {

        //if (collision.collider.CompareTag("Pipe"))
        //{
        //    TakeDamage(120);
        //    BoxCollider[] myColliders = this.GetComponents<BoxCollider>();
        //    foreach (BoxCollider bc in myColliders) bc.enabled = false;
        //    this.GetComponent<CharacterController>().enabled = false;
        //}

    }
    private void DestroyEnemy()
    {
        //anim.SetLayerWeight(1, 0);
        //anim.SetTrigger("DamageTaken");
        agent.isStopped = true;
        Destroy(this.gameObject, 4f);
        //agent.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
