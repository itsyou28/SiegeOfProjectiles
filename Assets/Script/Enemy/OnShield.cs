using UnityEngine;
using System.Collections;

public class OnShield : OnEnemyCollider
{
    protected override void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            iControl.OnShield();
            iControl.OnKnuckback(10);
        }
    }
}
