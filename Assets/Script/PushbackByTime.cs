using UnityEngine;
using System.Collections;

public class PushbackByTime : MonoBehaviour
{
    public GameObjectPool targetPool;
    public float pushbackTime;

    float accumeTime = 0;
    void OnEnable()
    {
        accumeTime = 0;
    }

    void Update()
    {
        accumeTime += Time.deltaTime;

        if(accumeTime > pushbackTime)
        {
            targetPool.Push(gameObject);
            gameObject.SetActive(false);
        }
    }
}
