using UnityEngine;
using System.Collections;

public class GameObjectPool : MonoBehaviour
{
    public GameObject origin;
    ObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(10, 5, CreateProjectile);
    }

    static int id = 0;
    GameObject CreateProjectile()
    {
        GameObject obj = Instantiate(origin);
        id++;
        obj.name = id.ToString();
        obj.transform.parent = transform;
        return obj;
    }

    public GameObject Pop()
    {
        return pool.pop();
    }

    public void Push(GameObject target)
    {
        target.gameObject.transform.SetParent(transform);
        pool.push(target);
    }
}
