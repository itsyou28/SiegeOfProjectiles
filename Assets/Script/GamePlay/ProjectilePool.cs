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
        pool.push(target);
    }
}
