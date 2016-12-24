using UnityEngine;
using System.Collections;

public class GlobalAttackPool : ProjectilePool
{
    static private GlobalAttackPool instance = null;
    static public GlobalAttackPool Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GlobalAttackPool>();

            return instance;
        }
    }
}
