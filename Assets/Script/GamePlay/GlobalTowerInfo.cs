using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalTowerInfo
{
    static List<Tower> listTower = new List<Tower>();
    
    public static Tower GetTower()
    {
        if (listTower.Count == 0)
            return null;
        
        return listTower[Random.Range(0, listTower.Count)];
    }

    public static void AddTower(Tower target)
    {
        listTower.Add(target);
    }

    public static void RemoveTower(Tower target)
    {
        listTower.Remove(target);
        listTower.TrimExcess();
    }
}
