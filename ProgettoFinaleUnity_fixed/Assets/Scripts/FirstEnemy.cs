using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemy : EnemyClass
{
    public override void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
