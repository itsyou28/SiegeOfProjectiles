using UnityEngine;
using System.Collections;

public class OnEnemyCollider : MonoBehaviour
{
    public iEnemyControl iControl { get; set; }

    float accumeTime = 0;

    void OnTriggerEnter(Collider col)
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
        //else if(col.CompareTag("GlobalAttack"))
        //{
        //    OnGlobalAttack();
        //}
    }

    protected virtual void OnDeerStar()
    {
    }

    protected virtual void OnMeteo()
    {
        accumeTime = 0;
        iControl.OnMeteo();
    }

    protected virtual void OnTowerAttack()
    {
    }

    protected virtual void OnGlobalAttack()
    {
    }
}
