using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lightning", menuName = "ScriptableObjects/Lightning")]

public class Lightning : ChainableAttack
{
    public float Radius = 5f;
    public LayerMask LayersToHit;
    public int MaxEnemiesHittable = 2;
    [Tooltip("The damage multiplier applied before damaging the next enemy. ")]
    [Range(0.1f,1)]
    public float DamageScalePerEnemy=0.6f;
    
    private List<EnemyClass> alreadyHit;
    private DamageStats multipliedDamageStats;
    public override void Apply(EnemyClass recepient)
    {
        alreadyHit = new List<EnemyClass>(MaxEnemiesHittable);

        List<EnemyClass> enemiesToHit = GetEnemiesDepth(recepient);

        if (enemiesToHit.Count < 2)
            return;
        HitEnemies(enemiesToHit);

       
        for (int i = 0; i < alreadyHit.Count; i ++)
        {
            EnemyClass selected1 = alreadyHit[i];
            if (i + 1 == enemiesToHit.Count)
                break;
            EnemyClass selected2 = alreadyHit[i + 1];
            CreateLightning(selected1, selected2);

        }


            //int index = firstEnemyHitIndex;
            //if(index +1 < enemiesHit.Count-1)
            //{
            //    index++;
            //}
            //else if( index -1 >= 0)
            //{
            //    index--;
            //}

    }
    public void HitEnemies(List<EnemyClass> enemiesToHit)
    {
        multipliedDamageStats = DamageStats;

        for (int i = 0; i < enemiesToHit.Count; i++)
        {
            HitInfo lightningHit = new HitInfo();
            lightningHit.DamageStats = multipliedDamageStats;
            enemiesToHit[i].OnHit(lightningHit);
            alreadyHit.Add(enemiesToHit[i]);
            multipliedDamageStats.Damage = multipliedDamageStats.Damage * DamageScalePerEnemy;
        }
    }
    public List<EnemyClass> GetEnemiesDepth(EnemyClass firstEnemy)
    {
        EnemyClass currentEnemy = firstEnemy;
        List<EnemyClass> toReturn = new List<EnemyClass>(MaxEnemiesHittable);
        toReturn.Add(currentEnemy);
        for (int steps = 1; steps < MaxEnemiesHittable; steps++)
        {
            Collider[] thingsHit = Physics.OverlapSphere(currentEnemy.transform.position, Radius, LayersToHit);
            for (int i = 0; i < thingsHit.Length; i++)
            {
                EnemyClass enemy = thingsHit[i].GetComponent<EnemyClass>();
                if (enemy != null && !toReturn.Contains(enemy))
                {
                    currentEnemy = enemy;
                    toReturn.Add(enemy);
                    break;

                }
            }
        }
        toReturn.TrimExcess();
        return toReturn;
    }
    public void CreateLightning(EnemyClass one,EnemyClass two)
    {
      
        Debug.DrawLine(one.transform.position, two.transform.position,Color.blue,3);
        Debug.Log("Lightning!");

    }
}
