using UnityEngine;
using System.Collections;

public class OnDamage : OnEnemyCollider
{
    protected override void OnDeerStar()
    {
        iControl.OnDamage();
    }

    protected override void OnTowerAttack()
    {
        iControl.OnDamage();
        iControl.OnKnuckback(8);
    }
}
