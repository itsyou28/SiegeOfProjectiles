using UnityEngine;
using System.Collections;

public class OnEnemyCollider : MonoBehaviour
{
    public iEnemyControl iControl { get; set; }
    
    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            OnDeerStar();
        }
        else if (col.CompareTag("Meteo"))
        {
            OnMeteo();
        }
        else if (col.CompareTag("TowerAttack"))
        {
            OnTowerAttack();
        }
    }
    
    protected virtual void OnDeerStar()
    {
    }

    protected virtual void OnMeteo()
    {
        iControl.OnMeteo();
    }

    protected virtual void OnTowerAttack()
    {
    }
}
