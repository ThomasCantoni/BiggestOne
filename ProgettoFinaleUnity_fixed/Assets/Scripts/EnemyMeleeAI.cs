using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMeleeAI : EnemyClass, IDamager
{
    public NavMeshAgent agent;
    //public Transform player;
    public LayerMask layer;
    public DamageStats damage;
    public Animator anim;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //States
    public float attackRange;
    public bool playerInAttackRange;

    public DamageStats DamageStats { get { return damage; } set { damage = value; } }

    private void Awake()
    {
        //player = GameObject.Find("Player 2.0").transform;
    }

    private void Update()
    {
        //Check for sight and attack range
        AttackPlayer();
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, layer);
    }

    private void AttackPlayer()
    {

        agent.SetDestination(player.transform.position);
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        agent.isStopped = distanceFromPlayer <= attackRange;
        if (distanceFromPlayer <= attackRange)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
        }
        if (!alreadyAttacked)
        {

            if (distanceFromPlayer <= attackRange)
            {
                HitInfo infoDamage = new HitInfo(this, player.GetComponent<HealthPlayer>());
                player.GetComponent<HealthPlayer>().OnHit(infoDamage);
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
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
    }
}
