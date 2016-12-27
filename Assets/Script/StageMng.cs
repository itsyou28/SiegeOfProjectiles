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

        if (curPhaseIdx >= listPhase.Count)
            return null;

        curPhaseIdx++;

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

public class StageMng : MonoBehaviour
{
    public GameObject[] enemyOrigin;

    Dictionary<int, StageSet> dicStage;

    int curStageIdx;

    StageSet curStage;
    PhaseSet curPhase;

    int[] possibleTypeList;

    Vector3 spawnPos = new Vector3(94, 0.11f, 0);

    float accumeTime = 0;
    float nextSpawnTime = 0.5f;

    bool IsAllCreated = false;

    void Awake()
    {
        dicStage = FileManager.ResourceLoad("stageData") as Dictionary<int, StageSet>;

        EnemyList.EnemyIsZero += OnEnemyIsZero; ;
    }

    private void OnEnemyIsZero()
    {
        if (EnemyList.Count == 0 && IsAllCreated)
        {
            //Stage Clear!!

        }
    }

    //play일 때만 작동한다. 
    //진행상황 저장을 위한 유저데이터 관리
    //진행상황은 자동으로 저장된다. 
    

    void OnChangeStage()
    {
        curStageIdx++;
        curStage = dicStage[curStageIdx];

        curPhase = curStage.StartPhase();
        possibleTypeList = curPhase.GetIncludeEnemyTypeList();

        IsAllCreated = false;
    }

    void OnChangePhase()
    {
        curPhase = curStage.NextPhase();

        if (curPhase == null)
        {
            IsAllCreated = true;
            return;
        }

        possibleTypeList = curPhase.GetIncludeEnemyTypeList();
    }
    
    void Update()
    {
        if(!IsAllCreated)
            EnemySpawn();
    }

    /// <summary>
    /// 랜덤한 시간 간격으로 적 생성함수를 호출한다. 
    /// </summary>
    void EnemySpawn()
    {
        accumeTime += Time.deltaTime;

        if (accumeTime > nextSpawnTime)
        {
            CreateEnemy();

            accumeTime -= nextSpawnTime;
            nextSpawnTime = Random.Range(0.5f, 1.8f);
        }
    }

    /// <summary>
    /// 스테이지-페이즈에 해당하는 타입을 랜덤하게 생성한다. 
    /// </summary>
    void CreateEnemy()
    {
        //현재 페이즈에서 생성할 수 있는 타입을 랜덤하게 선택해서 생성한다. 
        int randType = Random.Range(0, possibleTypeList.Length);

        GameObject obj = Instantiate(enemyOrigin[randType]);

        //생성지대의 z축을 랜덤지정해서 생성위치를 지정한다. 
        spawnPos.z = Random.Range(-50, 25);
        obj.transform.position = spawnPos;

        //생성하고 생성수량을 증가시킨다. 
        if(curPhase.Increase(randType))
        {
            //지정수량을 채웠다면 생성가능한 타입리스트에서 제거한다. 
            int targetIdx = 0;
            for(int idx=0; idx<possibleTypeList.Length; idx++)
            {
                if (possibleTypeList[idx] == randType)
                {
                    targetIdx = idx;
                    break;
                }
            }
            BK_Function.RemoveAtArr(targetIdx, ref possibleTypeList);

            if (possibleTypeList.Length == 0)
                OnChangePhase();
        }
    }
}
