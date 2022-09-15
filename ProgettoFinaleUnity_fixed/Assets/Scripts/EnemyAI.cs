using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour,IKillable,IDamager
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask layer;
    public DamageStats damage;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //States
    public float attackRange;
    public bool playerInAttackRange;

    public IKillable.OnDeathEvent deathEvent { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public MonoBehaviour Mono => throw new System.NotImplementedException();

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
    }

    private void AttackPlayer()
    {
        agent.SetDestination(player.position);
        
        if (!alreadyAttacked)
        {
            float distanceFromPlayer = Vector3.Distance(this.transform.position, player.position);
            if (distanceFromPlayer <= 2f)
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

    public void OnDeath()
    {
    }

    public void OnHit(HitInfo info)
    {
        
    }
}
