using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalList<T>
{
    static List<T> _list = new List<T>();

    public static T GetRandomItem()
    {
        if (_list.Count == 0)
            return default(T);

        return _list[Random.Range(0, _list.Count)];
    }

    public static void Add(T target)
    {
        _list.Add(target);
    }

    public static void Remove(T target)
    {
        _list.Remove(target);
        _list.TrimExcess();
    }
}
