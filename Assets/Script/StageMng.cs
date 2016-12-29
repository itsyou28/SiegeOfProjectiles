using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageMng : MonoBehaviour
{
    public GameObject[] enemyOrigin;

    Dictionary<int, StageSet> dicStage;

    int curStageIdx = 0;

    StageSet curStage;
    PhaseSet curPhase;

    int[] possibleTypeList;

    Vector3 spawnPos = new Vector3(94, 0.11f, 0);

    float accumeTime = 0;
    float nextSpawnTime = 0.5f;

    bool IsAllCreated = false;
    bool isPlaying = false;

    void Awake()
    {
        dicStage = FileManager.ResourceLoad("stageData") as Dictionary<int, StageSet>;

        EnemyList.EnemyIsZero += OnEnemyIsZero; ;
    }

    void Start()
    {
        State tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_Play);
        tstate.EventStart += OnStartPlay;
        tstate.EventEnd += OnEndPlay;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_StageGuide);
        tstate.EventStart += OnStarStageGuide;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_MainMenu);
        tstate.EventStart += OnStartMainMenu;
    }

    private void OnStartMainMenu(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        curStageIdx = 0;
    }

    private void OnStarStageGuide(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        OnChangeStage();
    }

    private void OnEndPlay(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        isPlaying = false;
    }

    private void OnStartPlay(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        isPlaying = true;
    }

    private void OnEnemyIsZero()
    {
        if (EnemyList.Count == 0 && IsAllCreated)
        {
            if (curStageIdx >= dicStage.Count - 1)
                FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_ALL_CLEAR);
            else
            {
                //Stage Clear!!
                FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_CLEAR);
            }
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
        if(isPlaying && !IsAllCreated)
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
        randType = possibleTypeList[randType];

        GameObject obj = Instantiate(enemyOrigin[randType-1]);

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

            if (possibleTypeList == null)
                OnChangePhase();
        }
    }
}
