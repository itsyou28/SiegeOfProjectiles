using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalList<T>
{
    public static List<T> list = new List<T>();

    public static int Count { get { return list.Count; } }

    public static T GetRandomItem()
    {
        if (list.Count == 0)
            return default(T);

        return list[Random.Range(0, list.Count)];
    }

    public static void Add(T target)
    {
        list.Add(target);
    }

    public static void Remove(T target)
    {
        list.Remove(target);
    }
    
}
