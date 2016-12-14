using UnityEngine;
using System.Collections;

public class OnShield : OnEnemyCollider
{
    public int shieldIdx = -1;

    [SerializeField]
    int shieldHP;

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("PlayerProjectile"))
        {
            iControl.OnKnuckback(10);
            shieldHP -= 1;
        }

        if(col.CompareTag("Meteo"))
        {
            shieldHP -= 3;
            iControl.OnMeteo();
        }

        if(shieldHP <= 0)
        {
            iControl.OnDestroyedShield(shieldIdx);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
