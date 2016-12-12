using UnityEngine;
using System.Collections;

public class ProjectilePool : MonoBehaviour
{
    public GameObject origin;
    ObjectPool<Projectile> pool;

    void Awake()
    {
        pool = new ObjectPool<Projectile>(10, 5, CreateProjectile);
    }

    static int id = 0;
    Projectile CreateProjectile()
    {
        GameObject obj = Instantiate(origin);
        id++;
        obj.name = id.ToString();
        obj.transform.parent = transform;
        return obj.GetComponent<Projectile>();
    }

    public Projectile Pop()
    {
        return pool.pop();
    }

    public void Push(Projectile target)
    {
        target.gameObject.transform.SetParent(transform);
        pool.push(target);
    }
}
