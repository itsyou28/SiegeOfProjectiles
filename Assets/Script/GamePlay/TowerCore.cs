using UnityEngine;
using System.Collections;

public class TowerCore : MonoBehaviour
{
    [SerializeField]
    Tower pTower;

    public void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("EnemyProjectile"))
        {
            pTower.Hit();
        }
    }
}
