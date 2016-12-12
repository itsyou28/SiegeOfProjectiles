using UnityEngine;
using System.Collections;

public class E_ProjectilePool : ProjectilePool
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
}
