using UnityEngine;
using System.Collections;

public class E_ProjectilePool : MonoBehaviour
{
    static private E_ProjectilePool instance = null;
    static public E_ProjectilePool Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<E_ProjectilePool>();

            return instance;
        }
    }

    public GameObject origin;
    ObjectPool<E_Projectile> pool;

    void Awake()
    {
        pool = new ObjectPool<E_Projectile>(10, 5, CreateProjectile);
    }

    static int id = 0;
    E_Projectile CreateProjectile()
    {
        GameObject obj = Instantiate(origin);
        id++;
        obj.name = id.ToString();
        obj.transform.parent = transform;
        return obj.GetComponent<E_Projectile>();
    }

    public E_Projectile Pop()
    {
        return pool.pop();
    }

    public void Push(E_Projectile target)
    {
        target.gameObject.transform.SetParent(transform);
        pool.push(target);
    }
}
