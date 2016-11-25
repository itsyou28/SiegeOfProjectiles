using UnityEngine;
using System.Collections;

public class OnDamage : MonoBehaviour
{
    public Enemy iControl;

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("PlayerProjectile"))
            iControl.OnDamage();

        if (col.CompareTag("TowerAttack"))
            iControl.OnKnuckBackDamage();
    }
}
