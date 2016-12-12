using UnityEngine;
using System.Collections;

public class DeerStarPool : ProjectilePool
{
    static private DeerStarPool instance = null;
    static public DeerStarPool Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DeerStarPool>();

            return instance;
        }
    }
}
