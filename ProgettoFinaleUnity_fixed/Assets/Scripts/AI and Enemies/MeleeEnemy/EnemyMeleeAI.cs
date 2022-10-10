using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMeleeAI : EnemyClass, IDamager
{
    //public NavMeshAgent agent;
    //public Transform player;
    public LayerMask layer;
    public DamageStats damage;
    //public Animator anim;

    //Attacking
    //public float timeBetweenAttacks;
    //bool alreadyAttacked;
    //States
     
    public bool HasAlreadyAttack;
    public float timeBetweenAttacks;
    public DamageStats BaseStats { get { return damage; } set { damage = value; } }
    public DamageStats OutputStats { get { return damage; } set { damage = value; } }



    private void Update()
    {
        //Check for sight and attack range
        
        if (!IsDead)
        {
            AttackPlayer();
            PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, layer);
        }
        
    }

    private void AttackPlayer()
    {

        NavMeshAgent.SetDestination(player.transform.position);
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        NavMeshAgent.isStopped = distanceFromPlayer <= attackRange;
        if (distanceFromPlayer <= attackRange)
        {
            anim.SetTrigger("Attack");
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
        }
        if (!HasAlreadyAttack)
        {

            if (distanceFromPlayer <= attackRange)
            {
                HitInfo infoDamage = new HitInfo(this, player.GetComponent<HealthPlayer>());
                player.GetComponent<HealthPlayer>().OnHit(infoDamage);

                
            }
            HasAlreadyAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public override void ResetAttack()
    {
        HasAlreadyAttack = false;
    }
    public override void OnDeath()
    {
        OnEnemyDeath?.Invoke();
        OnEnemyDeathParam?.Invoke(this);
        OnDeathUnityEvent?.Invoke();
        anim.SetBool("Run", false);
        anim.SetTrigger("Death");
        NavMeshAgent.isStopped = true;
        Destroy(NavMeshAgent.transform.gameObject, 3f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
