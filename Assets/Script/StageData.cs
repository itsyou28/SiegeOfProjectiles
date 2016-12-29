using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StageSet
{
    List<PhaseSet> listPhase;
    int curPhaseIdx = 0;

    public void AddPhase(PhaseSet data)
    {
        if (listPhase == null)
            listPhase = new List<PhaseSet>();

        listPhase.Add(data);
    }

    public PhaseSet StartPhase()
    {
        curPhaseIdx = 0;
        return listPhase[0];
    }

    public PhaseSet NextPhase()
    {
        listPhase[curPhaseIdx].Reset();

        curPhaseIdx++;

        if (curPhaseIdx >= listPhase.Count)
            return null;


        return listPhase[curPhaseIdx];
    }
}

[System.Serializable]
public class PhaseSet
{
    Dictionary<int, EnemyPhaseData> list;

    public void AddData(EnemyPhaseData data)
    {
        if (list == null)
            list = new Dictionary<int, EnemyPhaseData>();

        list.Add(data.EnemyType, data);
    }

    public int[] GetIncludeEnemyTypeList()
    {
        int[] returnArr = new int[list.Count];
        list.Keys.CopyTo(returnArr, 0);

        return returnArr;
    }

    public void Reset()
    {
        foreach (EnemyPhaseData data in list.Values)
            data.Reset();
    }

    public bool Increase(int type)
    {
        return list[type].IncreaseCount();
    }
}

[System.Serializable]
public class EnemyPhaseData
{
    int m_iEnemyType;
    int m_iEquantity;

    [System.NonSerialized]
    int m_iCreatedCound;

    public int EnemyType { get { return m_iEnemyType; } }

    public EnemyPhaseData(int type, int quantity)
    {
        m_iEnemyType = type;
        m_iEquantity = quantity;
    }

    public void Reset()
    {
        m_iCreatedCound = 0;
    }

    public bool IncreaseCount()
    {
        m_iCreatedCound++;

        if (m_iEquantity == m_iCreatedCound)
            return true;

        return false;
    }
}
