using UnityEngine;
using System.Collections;

public class OnDamage : OnEnemyCollider
{
    protected override void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("PlayerProjectile"))
            iControl.OnDamage();

        if (col.CompareTag("TowerAttack"))
        {
            iControl.OnDamage();
            iControl.OnKnuckback(8);
        }

        base.OnTriggerEnter(col);
    }
}
