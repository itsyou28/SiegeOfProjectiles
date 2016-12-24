using UnityEngine;
using System.Collections;

public class MeteoPool : ProjectilePool
{
    static private MeteoPool instance = null;
    static public MeteoPool Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MeteoPool>();

            return instance;
        }
    }
}
