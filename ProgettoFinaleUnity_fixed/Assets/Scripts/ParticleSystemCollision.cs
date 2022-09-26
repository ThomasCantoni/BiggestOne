using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCollision : MonoBehaviour, IDamager
{
    public GameObject enemy;

    public DamageStats damage;
    public DamageStats DamageStats
    {
        get { return damage; }
        set { damage = value; }
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("nagg crist");
        HitInfo info = new HitInfo(this, enemy.GetComponent<EnemyClass>());
        enemy.GetComponent<EnemyClass>().OnHit(info);
    }
}
