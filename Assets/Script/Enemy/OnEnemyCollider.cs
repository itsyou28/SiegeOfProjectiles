using UnityEngine;
using System.Collections;

public class OnEnemyCollider : MonoBehaviour
{
    public iEnemyControl iControl { get; set; }

    float accumeTime = 0;

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Meteo"))
        {
            accumeTime = 0;
            iControl.OnMeteo();
        }
    }    
}
