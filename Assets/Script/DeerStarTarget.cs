using UnityEngine;
using System.Collections;

public class DeerStarTarget : MonoBehaviour
{
    [SerializeField]
    GameObjectPool pool;

    float fireRemainTime;
    float arriveRemainTime;

    public void OnEnd()
    {
        pool.Push(gameObject);
        gameObject.SetActive(false);
    }
}
