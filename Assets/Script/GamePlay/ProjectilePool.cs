using UnityEngine;
using System.Collections;

public class ProjectilePool : MonoBehaviour
{
    static private ProjectilePool instance = null;
    static public ProjectilePool Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ProjectilePool>();

            return instance;
        }
    }

    public GameObject origin;
    ObjectPool<Projectile> pool;

    void Awake()
    {
        pool = new ObjectPool<Projectile>(300, 100, CreateProjectile);
    }

    Projectile CreateProjectile()
    {
        GameObject obj = Instantiate(origin);
        obj.transform.parent = transform;
        return obj.GetComponent<Projectile>();
    }

    public Projectile Pop()
    {
        return pool.pop();
    }

    public void Push(Projectile target)
    {
        pool.push(target);
    }
}
