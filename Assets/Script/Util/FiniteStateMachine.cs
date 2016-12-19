using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void deleStateTransEvent(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID);
public delegate void deleStatePauseResume();
public delegate void deleLayerPauseResume(FSM_LAYER layerNum);

public enum FSM_LAYER
{
    //레이어는 일반 배열을 사용하므로 Enum 변수는 반드시 0부터 순열로 정의되어야 한다. 
    USERSTORY = 0
}

public enum FSM_ID
{
    NONE = 0,
    USERSTORY,
}

public enum TRANS_PARAM_ID
{
    TRIGGER_NONE,
    TRIGGER_RESET,
    TRIGGER_ESCAPE,
    TRIGGER_BACKBTN,
    TRIGGER_CLOSE,
    TRIGGER_NEXT,
    BOOL_IAP_READY,
    INT_HP,
    TRIGGER_HIT,
    TRIGGER_TARGET_DESTROYED,
    BOOL_HAVE_TARGET,
    BOOL_IS_ARRIVE_TARGET,
    INT_DEFENDER_BEHAVIOR_MODE,
    TRIGGER_REINFORCE,
    TRIGGER_CLEAR,
    TRIGGER_ALL_CLEAR,
    TRIGGER_GAMEOVER
}

public enum STATE_ID
{
    None,
    AnyState,
    HistoryBack,
    Exit,
    Enemy_SearchTarget,
    Enemy_Move,
    Enemy_Attack,
    Enemy_Damage,
    Enemy_Idle,
    Enemy_Dead,
    Enemy_Protection,
    Enemy_DestroySelf,
    US_Start,
    US_MainMenu,
    US_ExitConfirm,
    US_Exit,
    US_Play,
    US_StageClear,
    US_Reinforce,
    US_Ending,
    US_GameOver,
}


public enum TRANS_ID
{
    NONE,
    TIME,
    HISTORY_BACK,
    RESET,
    ESCAPE_TO_MAIN,
    BACK_TO_MAIN,
    GAMEOVER,
    ESCAPE
}

public enum TransitionType
{
    INT, FLOAT, BOOL, TRIGGER
}
public enum TransitionComparisonOperator
{
    EQUALS, NOTEQUAL, GREATER, LESS
}

public static class FSM_Manager
{
    const int iMaxLayer = 3;

    static Dictionary<FSM_ID, FSM>[] dicFSM_EachLayer = new Dictionary<FSM_ID, FSM>[iMaxLayer];

    static FSM[] curFSM_EachLayer = new FSM[iMaxLayer];

    static event deleLayerPauseResume EventLayerPause;
    static event deleLayerPauseResume EventLayerResume;

    static Dictionary<FSM_LAYER, List<deleStateTransEvent>> dicLayerChangeState = new Dictionary<FSM_LAYER, List<deleStateTransEvent>>();

    static FSM_ID[] layerFSM_Buffer = new FSM_ID[iMaxLayer];

    static int layerNum;

    public static void ReleaseAll()
    {
        curFSM_EachLayer = new FSM[iMaxLayer];

        for (int i = 0; i < iMaxLayer; i++)
        {
            if (dicFSM_EachLayer[i] != null)
                dicFSM_EachLayer[i].Clear();
        }

        dicFSM_EachLayer = new Dictionary<FSM_ID, FSM>[iMaxLayer];

        EventLayerPause = null;
        EventLayerResume = null;

        dicLayerChangeState = new Dictionary<FSM_LAYER, List<deleStateTransEvent>>();
    }

    public static void AddFSM(FSM_LAYER eLayer, FSM newFSM, FSM_ID id =FSM_ID.NONE)
    {
        layerNum = (int)eLayer;
        if(dicFSM_EachLayer.Length <= layerNum)
        {
            Debug.LogError("할당되지 않은 레이어를 호출하고 있습니다 Layer 및 iMaxLayer를 확인해주세요");
            return;
        }

        if(dicFSM_EachLayer[layerNum] == null)
            dicFSM_EachLayer[layerNum] = new Dictionary<FSM_ID, FSM>();

        if (id != FSM_ID.NONE)
        {
            if (dicFSM_EachLayer[layerNum].ContainsKey(id))
                Debug.Log("동일 레이어에 중복된 FSM ID 를 등록하려함!");
            else
                newFSM.SetFSMID(id);
        }

        dicFSM_EachLayer[layerNum].Add(newFSM.fsmID, newFSM);

        if (curFSM_EachLayer[layerNum] == null)
        {
            curFSM_EachLayer[layerNum] = newFSM;

            RegisterToFSM_ChangeLayerState(eLayer);

            curFSM_EachLayer[layerNum].Resume();
        }
    }

    public static void RemoveFSM(FSM_LAYER eLayer, FSM_ID id)
    {
        layerNum = (int)eLayer;
        if (dicFSM_EachLayer[layerNum].ContainsKey(id))
        {
            dicFSM_EachLayer[layerNum].Remove(id);
        }
        else
            Debug.LogWarning("가지고 있지 않은 FSM을 삭제하려 함");

        if (curFSM_EachLayer[layerNum].fsmID == id)
            curFSM_EachLayer[layerNum] = null;
    }

    public static void SetInt_NoCondChk(FSM_LAYER eLayer, TRANS_PARAM_ID paramID, int value)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetInt_NoCondChk(paramID, value);
    }

    public static void SetFloat_NoCondChk(FSM_LAYER eLayer, TRANS_PARAM_ID paramID, float value)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetFloat_NoCondChk(paramID, value);
    }

    public static void SetBool_NoCondChk(FSM_LAYER eLayer, TRANS_PARAM_ID paramID, bool value)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetBool_NoCondChk(paramID, value);
    }



    public static void SetInt(FSM_LAYER eLayer, TRANS_PARAM_ID paramID, int value)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetInt(paramID, value);
    }

    public static void SetFloat(FSM_LAYER eLayer, TRANS_PARAM_ID paramID, float value)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetFloat(paramID, value);
    }

    public static void SetBool(FSM_LAYER eLayer, TRANS_PARAM_ID paramID, bool value)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetBool(paramID, value);
    }

    public static void SetTrigger(FSM_LAYER eLayer, TRANS_PARAM_ID paramID)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].SetTrigger(paramID);
    }



    public static int GetInt(FSM_LAYER eLayer, TRANS_PARAM_ID paramID)
    {
        if (!CurLayerCheck(eLayer))
            return 0;
        return curFSM_EachLayer[(int)eLayer].GetParamInt(paramID);
    }

    public static float GetFloat(FSM_LAYER eLayer, TRANS_PARAM_ID paramID)
    {
        if (!CurLayerCheck(eLayer))
            return 0;
        return curFSM_EachLayer[(int)eLayer].GetParamFloat(paramID);
    }

    public static bool GetBool(FSM_LAYER eLayer, TRANS_PARAM_ID paramID)
    {
        if (!CurLayerCheck(eLayer))
            return false;
        return curFSM_EachLayer[(int)eLayer].GetParamBool(paramID);
    }

    static bool CurLayerCheck(FSM_LAYER eLayer)
    {
        if(curFSM_EachLayer[(int)eLayer] == null)
        {
            Debug.LogWarning("지정한 레이어에 FSM이 지정되 있지 않음");
            return false;
        }
        else
            return true;
    }


    public static FSM GetCurFSM(FSM_LAYER eLayer)
    {
        if (!CurLayerCheck(eLayer))
            return null;

        return curFSM_EachLayer[(int)eLayer];
    }

    public static FSM GetFSM(FSM_LAYER eLayer, FSM_ID fsmID)
    {
        layerNum = (int)eLayer;
        if (dicFSM_EachLayer == null)
            return null;

        if (dicFSM_EachLayer[layerNum] == null)
        {
            Debug.LogError("지정한 레이어에 FSM이 지정되 있지 않음 ");
            return null;
        }

        if (!dicFSM_EachLayer[layerNum].ContainsKey(fsmID))
        {
            Debug.LogError(fsmID.ToString() + " FSM 이 등록되어 있지 않음");
            return null;
        }

        return dicFSM_EachLayer[layerNum][fsmID];
    }

    public static State GetState(FSM_LAYER eLayer, FSM_ID fsmID, STATE_ID stateID)
    {
        layerNum = (int)eLayer;
        if (dicFSM_EachLayer == null)
            return null;

        if (dicFSM_EachLayer[layerNum] == null)
        {
            Debug.LogError("지정한 레이어에 FSM이 지정되 있지 않음 ");
            return null;
        }

        if (!dicFSM_EachLayer[layerNum].ContainsKey(fsmID))
        {
            Debug.LogError(fsmID.ToString() + " FSM 이 등록되어 있지 않음");
            return null;
        }

        return dicFSM_EachLayer[layerNum][fsmID].GetState(stateID);
    }

    public static State GetCurState(FSM_LAYER eLayer)
    {
        layerNum = (int)eLayer;
        if (curFSM_EachLayer[layerNum] == null)
            return null;

        return curFSM_EachLayer[layerNum].GetCurState();
    }

    public static STATE_ID GetCurStateID(FSM_LAYER eLayer)
    {
        layerNum = (int)eLayer;
        if (curFSM_EachLayer[layerNum] == null)
            return STATE_ID.None;

        return curFSM_EachLayer[layerNum].GetCurStateID();
    }

    public static void HistoryBack(FSM_LAYER eLayer)
    {
        if (CurLayerCheck(eLayer))
            curFSM_EachLayer[(int)eLayer].HistoryBack();
    }

    public static void ChangeFSM(FSM_LAYER eLayer, FSM_ID fsmID)
    {
        layerNum = (int)eLayer;
        if (CurLayerCheck(eLayer))
        {
            layerFSM_Buffer[layerNum] = curFSM_EachLayer[layerNum].fsmID;

            curFSM_EachLayer[layerNum].Pause();
            
            UnRegisterToFSM_ChangeLayerState(eLayer);

            curFSM_EachLayer[layerNum] = dicFSM_EachLayer[layerNum][fsmID];

            RegisterToFSM_ChangeLayerState(eLayer);
            
            curFSM_EachLayer[layerNum].Resume();
        }
    }

    public static void ChangeFSM_Manual(FSM_LAYER eLayer, FSM_ID fsmID)
    {
        layerNum = (int)eLayer;
        if (CurLayerCheck(eLayer))
        {
            layerFSM_Buffer[layerNum] = curFSM_EachLayer[layerNum].fsmID;

            UnRegisterToFSM_ChangeLayerState(eLayer);

            curFSM_EachLayer[layerNum] = dicFSM_EachLayer[layerNum][fsmID];

            RegisterToFSM_ChangeLayerState(eLayer);
        }
    }

    public static void ChangeFSM_BufferBack(FSM_LAYER eLayer)
    {
        ChangeFSM(eLayer, layerFSM_Buffer[layerNum]);
    }

    static string[] errMsgAbout_Register_ChangeLayerState = 
    {
        "Success",
        "해당 레이어에 FSM이 등록되어 있지 않습니다. ",
        "해당 레이어의 이벤트 버퍼 리스트가 존재하지 않습니다"
    };

    private static int RegisterToFSM_ChangeLayerState(FSM_LAYER eLayer)
    {
        layerNum = (int)eLayer;
        if (!CurLayerCheck(eLayer))
            return 1;

        UnRegisterToFSM_ChangeLayerState(eLayer);

        if (!dicLayerChangeState.ContainsKey(eLayer))
            return 2;

        for (int idx = 0; idx < dicLayerChangeState[eLayer].Count; idx++)
            curFSM_EachLayer[layerNum].EventStateChange += dicLayerChangeState[eLayer][idx];

        return 0;
    }

    private static int UnRegisterToFSM_ChangeLayerState(FSM_LAYER eLayer)
    {
        if (!CurLayerCheck(eLayer))
            return 1;

        if (!dicLayerChangeState.ContainsKey(eLayer))
            return 2;

        for (int idx = 0; idx < dicLayerChangeState[eLayer].Count; idx++)
            curFSM_EachLayer[layerNum].EventStateChange -= dicLayerChangeState[eLayer][idx];

        return 0;
    }

    public static void RegisterEventChangeLayerState(FSM_LAYER eLayer, deleStateTransEvent _deleFunc)
    {
        layerNum = (int)eLayer;
        if (layerNum >= iMaxLayer)
        {
            Debug.LogWarning("할당 되지 않은 레이어를 호출했습니다");
            return;
        }

        if (!dicLayerChangeState.ContainsKey(eLayer))
            dicLayerChangeState.Add(eLayer, new List<deleStateTransEvent>());

        dicLayerChangeState[eLayer].Add(_deleFunc);

        int result = RegisterToFSM_ChangeLayerState(eLayer);

        if(result == 1)
            Debug.LogWarning(errMsgAbout_Register_ChangeLayerState[result] + " // 이 후 해당 레이어에 ChangeFSM이 호출 될 때 반영될 수 있습니다. ");
    }

    public static void UnRegisterChangeLayerState(FSM_LAYER eLayer, deleStateTransEvent _deleFunc)
    {
        if (!dicLayerChangeState.ContainsKey(eLayer))
            return;

        dicLayerChangeState[eLayer].Remove(_deleFunc);

        RegisterToFSM_ChangeLayerState(eLayer);
    }

    public static void RegisterPauseEvent(deleLayerPauseResume _deleFunc)
    {
        EventLayerPause += _deleFunc;
    }

    public static void UnRegisterPauseEvent(deleLayerPauseResume _deleFunc)
    {
        EventLayerPause -= _deleFunc;
    }

    public static void RegisterResumeEvent(deleLayerPauseResume _deleFunc)
    {
        EventLayerResume += _deleFunc;
    }

    public static void UnRegisterResumeEvent(deleLayerPauseResume _deleFunc)
    {
        EventLayerResume -= _deleFunc;
    }

    public static void Pause(FSM_LAYER eLayer)
    {
        if (!CurLayerCheck(eLayer))
            return;

        curFSM_EachLayer[layerNum].Pause();

        if (EventLayerPause != null)
            EventLayerPause(eLayer);
    }

    public static void Resume(FSM_LAYER eLayer)
    {
        if (!CurLayerCheck(eLayer))
            return;

        curFSM_EachLayer[layerNum].Resume();

        if (EventLayerResume != null)
            EventLayerResume(eLayer);
    }

    public static void Update()
    {
        for (int i = 0; i < iMaxLayer; i++)
        {
            if (curFSM_EachLayer[i] != null)
                curFSM_EachLayer[i].TimeCheck();
        }
    }

    public static void ReleaseLayer(FSM_LAYER eLayer)
    {
        dicFSM_EachLayer[layerNum].Clear();
        curFSM_EachLayer[layerNum] = null;
    }
}

public class FSM
{
    public event deleStatePauseResume EventPause;
    public event deleStatePauseResume EventResume;
    public event deleStateTransEvent EventStateChange;

    public FSM_ID fsmID { get; private set; }
    
    public void SetFSMID(FSM_ID id)
    {
        fsmID = id;
    }

    State anyState;

    public Dictionary<STATE_ID, State> dicStateList = new Dictionary<STATE_ID, State>();
    
    public Dictionary<TRANS_PARAM_ID, int> dicIntParam = new Dictionary<TRANS_PARAM_ID, int>();
    public Dictionary<TRANS_PARAM_ID, float> dicFloatParam = new Dictionary<TRANS_PARAM_ID, float>();
    public Dictionary<TRANS_PARAM_ID, bool> dicBoolParam = new Dictionary<TRANS_PARAM_ID, bool>();
    public TRANS_PARAM_ID triggerID;


    Stack<STATE_ID> history = new Stack<STATE_ID>();
    State curState;

    bool IsActive = false;
    int calldepth = 0;

    public FSM(FSM_ID id)
    {
        fsmID = id;

        anyState = new State(STATE_ID.AnyState);
        anyState.name = "AnyState";

        curState = anyState;

        dicStateList.Add(STATE_ID.AnyState, anyState);
    }

    public void InitNonSerializedField()
    {
        history = new Stack<STATE_ID>();
        curState = anyState;

        foreach (State t in dicStateList.Values)
            t.InitNonSerializedField();
    }


    public State MakeStateFactory(STATE_ID stateID, params TransitionCondition[] trans)
    {
        State tState = new State(stateID);


        if (trans != null)
        {
            if (trans.Length > 0)
            {
                for (int i = 0; i < trans.Length; i++)
                {
                    tState.AddTransition(trans[i]);
                    trans[i].SetOwner(tState);
                }
            }
        }

        AddState(tState);

        return tState;
    }

    public void AddState(State newState)
    {
        if (newState.eID == STATE_ID.AnyState)
            Debug.LogWarning("AnyState(Code_AnyState) 는 이미 생성되어 있습니다. ");

        if(dicStateList.ContainsKey(newState.eID))
        {
            Debug.LogError("ID가 겹침 " + newState.eID.ToString());
            return;
        }

        dicStateList.Add(newState.eID, newState);
    }

    public State GetState(STATE_ID stateID)
    {
        //Debug.Log("GetState : " + stateID.ToString() + " / dicStateList.Count : " + dicStateList.Count.ToString());

        if(dicStateList.ContainsKey(stateID))
            return dicStateList[stateID];

        Debug.LogError("등록되지 않은 상태를 호출했음");
        return null;
    }

    public State GetState(string stateName)
    {
        var itor = dicStateList.GetEnumerator();
        while(itor.MoveNext())
        {
            if (itor.Current.Value.name == stateName)
                return itor.Current.Value;
        }

        return null;
    }

    public STATE_ID GetCurStateID()
    {
        return curState.eID;
    }

    public State GetCurState()
    {
        return curState;
    }

    public State GetAnyState()
    {
        return anyState;
    }
    
    public void AddParamInt(TRANS_PARAM_ID param_id, int value = 0)
    {
        if(!dicIntParam.ContainsKey(param_id))
            dicIntParam.Add(param_id, value);
    }

    public void AddParamFloat(TRANS_PARAM_ID param_id, float value = 0.0f)
    {
        if(!dicFloatParam.ContainsKey(param_id))
            dicFloatParam.Add(param_id, value);
    }

    public void AddParamBool(TRANS_PARAM_ID param_id, bool value = false)
    {
        if(!dicBoolParam.ContainsKey(param_id))
            dicBoolParam.Add(param_id, value);
    }

    public int GetParamInt(TRANS_PARAM_ID param_id)
    {
        if (!dicIntParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        return dicIntParam[param_id];
    }
    public float GetParamFloat(TRANS_PARAM_ID param_id)
    {
        if (!dicFloatParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        return dicFloatParam[param_id];
    }
    public bool GetParamBool(TRANS_PARAM_ID param_id)
    {
        if (!dicBoolParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        return dicBoolParam[param_id];
    }


    public void SetInt(TRANS_PARAM_ID param_id, int value)
    {
        if (!dicIntParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        dicIntParam[param_id] = value;

        RequestTransitionChk();
    }

    public void SetFloat(TRANS_PARAM_ID param_id, float value)
    {
        if (!dicFloatParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        dicFloatParam[param_id] = value;

        RequestTransitionChk();
    }

    public void SetBool(TRANS_PARAM_ID param_id, bool value)
    {
        if (!dicBoolParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        dicBoolParam[param_id] = value;

        RequestTransitionChk();
    }

    public void SetInt_NoCondChk(TRANS_PARAM_ID param_id, int value)
    {
        if (!dicIntParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        dicIntParam[param_id] = value;
    }

    public void SetFloat_NoCondChk(TRANS_PARAM_ID param_id, float value)
    {
        if (!dicFloatParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        dicFloatParam[param_id] = value;
    }

    public void SetBool_NoCondChk(TRANS_PARAM_ID param_id, bool value)
    {
        if (!dicBoolParam.ContainsKey(param_id))
            throw new System.Exception(((FSM_ID)fsmID).ToString() + " not have given Transition parameter id. ");

        dicBoolParam[param_id] = value;
    }

    public void SetTrigger(TRANS_PARAM_ID param_id)
    {
        triggerID = param_id;

        RequestTransitionChk();

        triggerID = TRANS_PARAM_ID.TRIGGER_NONE;
    }

    void RequestTransitionChk()
    {
        if (!CurStateTransitionChk())
            AnyStateTransitionChk();
    }

    bool CurStateTransitionChk()
    {
        //Debug.Log("CurStateTransitionChk" + eType.ToString());
        if (curState.arrTransitionList != null)
        {
            for(int idx=0; idx<curState.arrTransitionList.Length; idx++)
            {
                if(curState.arrTransitionList[idx].CondtionCheck(this))
                {
                    //Debug.Log("ConditionCheck Pass : " + curState.arrTransitionList.Length.ToString() + "/ " + curState.name);
                    TransitionStart(curState.arrTransitionList[idx].eTransID, curState.arrTransitionList[idx].nextStateID);
                    return true;
                }
            }
        }
        
        return false;
    }

    void AnyStateTransitionChk()
    {
        //Debug.Log("AnyStateTransitionChk" + eType.ToString());
        for(int idx=0; idx<anyState.arrTransitionList.Length; idx++)
        {
            if (anyState.arrTransitionList[idx].CondtionCheck(this))
            {
                TransitionStart(anyState.arrTransitionList[idx].eTransID, anyState.arrTransitionList[idx].nextStateID);
                break;
            }
        }
    }

    void TransitionStart(TRANS_ID transParamID, STATE_ID nextStateID)
    {
        triggerID = TRANS_PARAM_ID.TRIGGER_NONE;

        if (!IsActive)
            return;
        
        if (nextStateID == STATE_ID.HistoryBack)
        {
            HistoryBack();
            return;
        }

        if (!dicStateList.ContainsKey(nextStateID))
        {
#if UNITY_EDITOR
            Debug.Log(nextStateID + " 등록된 씬이 아님!");
#else
            Debug.Log(nextStateID.ToString() + " 등록된 씬이 아님!");
#endif
            return;
        }

        calldepth++;

        if (calldepth > 1)
            Debug.LogWarning("FSM Call Depth is : " + calldepth
                + " // 재귀호출구조가 되면서 EvnetStateChange callback이 현재 상태만을 매개변수로 역순으로 반복호출됩니다. ");

#if _UNITY_EDITOR
        Debug.Log(fsmID + " Transition Start// " + curState.eID + " -> " 
            + dicStateList[nextStateID].eID + " // " + transParamID);
#endif

        STATE_ID preStateID = curState.eID;

        curState.End(transParamID, nextStateID);
        
        history.Push(curState.eID);

        curState = dicStateList[nextStateID];

        curState.Start(transParamID, preStateID);

        if (EventStateChange != null)
            EventStateChange(transParamID, curState.eID, preStateID);

        calldepth--;
    }

    public void TimeCheck()
    {
        if (curState.arrTransitionList == null || curState.arrTransitionList.Length == 0)
            return;

        for (int idx = 0; idx < curState.arrTransitionList.Length; idx++)
        {
            if (curState.arrTransitionList[idx].TimeConditionCheck(this))
            {
                TransitionStart(curState.arrTransitionList[idx].eTransID, curState.arrTransitionList[idx].nextStateID);
                return;
            }
        }
    }

    public void HistoryBack()
    {
        STATE_ID preStateID = curState.eID;
        STATE_ID nextStateID = history.Pop();

        curState.End(TRANS_ID.HISTORY_BACK, nextStateID);
        
        curState = dicStateList[nextStateID];

        curState.Start(TRANS_ID.HISTORY_BACK, preStateID);

        if (EventStateChange != null)
            EventStateChange(TRANS_ID.HISTORY_BACK, curState.eID, preStateID);
    }

    public void Pause()
    {
        if (!IsActive)
            return;
        IsActive = false;
        curState.Pause();

        if (EventPause != null)
            EventPause();
    }

    public void Resume()
    {
        if (IsActive)
            return;

        IsActive = true;
        curState.Resume();

        if (EventResume != null)
            EventResume();
    }
}

public class State
{
    public STATE_ID eID { get; private set;}
    public string name = null;

    public TransitionCondition[] arrTransitionList = null;

    public event deleStateTransEvent EventStart;
    public event deleStateTransEvent EventStart_Before;
    public event deleStateTransEvent EventStart_After1;
    public event deleStateTransEvent EventStart_After2;
    public event deleStateTransEvent EventEnd;
    public event deleStateTransEvent EventEnd_Before;
    public event deleStateTransEvent EventEnd_After;
    public event deleStatePauseResume EventPause;
    public event deleStatePauseResume EventResume;
    
    public State(STATE_ID id)
    {
        eID = id;
    }

    public void InitNonSerializedField()
    {
        if(arrTransitionList != null)
        {
            foreach (TransitionCondition t in arrTransitionList)
                t.SetOwner(this);
        }
    }

    public void AddTransition(TransitionCondition value)
    {
        TransitionCondition[] tempArr;

        if(arrTransitionList == null)
        {
            tempArr = new TransitionCondition[1];
            tempArr[0] = value;
        }
        else
        {
            for(int idx=0; idx<arrTransitionList.Length; idx++)
            {
                if(value.eTransID != 0 && arrTransitionList[idx].eTransID == value.eTransID)
                    Debug.LogWarning("동일한 전이 ID를 추가합니다");
            }

            tempArr = new TransitionCondition[arrTransitionList.Length + 1];

            for (int i = 0; i < arrTransitionList.Length; i++)
                tempArr[i] = arrTransitionList[i];

            tempArr[arrTransitionList.Length] = value;
        }
        
        arrTransitionList = tempArr;
        
        value.SetOwner(this);
    }

    public void ResetTime()
    {
        foreach(TransitionCondition t in arrTransitionList)
        {
            if (t.transitionWithTime != null)
                t.transitionWithTime.ResetStartTime();
        }
    }
    
    public void Start(TRANS_ID transID, STATE_ID preStateID)
    {
        if (EventStart_Before != null)
            EventStart_Before(transID, eID, preStateID);
        if (EventStart != null)
            EventStart(transID, eID, preStateID);
        if (EventStart_After1 != null)
            EventStart_After1(transID, eID, preStateID);
        if (EventStart_After2 != null)
            EventStart_After2(transID, eID, preStateID);
    }

    public void End(TRANS_ID transID, STATE_ID nextStateID)
    {
        if (EventEnd_Before != null)
            EventEnd_Before(transID, nextStateID, eID);
        if (EventEnd != null)
            EventEnd(transID, nextStateID, eID);
        if (EventEnd_After != null)
            EventEnd_After(transID, nextStateID, eID);
    }

    public void Pause()
    {
        if (EventPause != null)
            EventPause();
    }

    public void Resume()
    {
        if (EventResume != null)
            EventResume();
    }
}

[System.Serializable]
public class TransitionCondition
{
    public TRANS_ID eTransID { get; private set; }
    public ArrayList arrTransParam = new ArrayList();
    public TransCondWithTime transitionWithTime = null;

    public STATE_ID nextStateID { get; private set; }

    bool bCheckCondResult = false;

    State ownerState = null;
    /// <summary>
    /// 특정 상태로 넘어가기 위한 조건을 1개 이상 설정할 수 있다. 
    /// 가지고 있는 모든 조건을 만족했을 때만 다음 조건으로 전이한다. 
    /// </summary>
    /// <param name="uiID">특정코드번호를 정의하고 입력해두면 상태가 전이 됐을 때 어떤 전이조건으로 전이됐는지 체크할 때 사용할 수 있다. 사용할 일이 없다면 0으로 입력</param>
    /// <param name="transTime">초단위의 시간을 입력. 상태가 시작되고 입력된 시간이 지나면 조건이 만족된다. 시간조건을 사용하지 않는다면 0으로 입력</param>
    public TransitionCondition(STATE_ID _nextStateID, TRANS_ID uiID, float transTime, params TransCondWithParam[] _arrTransParam)
    {
        eTransID = uiID;
        nextStateID = _nextStateID;

        if (transTime != 0)
        {
            transitionWithTime = new TransCondWithTime(transTime);
        }

        if (_arrTransParam != null)
        {
            if (_arrTransParam.Length > 0)
            {
                for (int i = 0; i < _arrTransParam.Length; i++)
                {
                    arrTransParam.Add(_arrTransParam[i]);
                }
            }
        }
    }

    public void SetTransID(TRANS_ID id)
    {
        eTransID = id;
        Debug.LogWarning("Set trans ID : " + eTransID);
    }

    public void SetOwner(State _ownerState)
    {
        ownerState = _ownerState;
        if (transitionWithTime != null)
            transitionWithTime.SetOwnerState(_ownerState);
    }

    public bool CondtionCheck(FSM pFSM)
    {
        if(transitionWithTime == null)
        {
            if (arrTransParam.Count == 0)
            {
#if UNITY_EDITOR
                Debug.LogWarning("전이 조건이 없습니다. // FSM ID : " + pFSM.fsmID.ToString() + " / " + "CurrentState : " 
                    + pFSM.GetCurState().name + " / OwnerState : " + ownerState.name
                    + "/ TransID : " + eTransID.ToString());
#endif
                return false;
            }
        }

        bCheckCondResult = true;

        // &연산 true&true=true / true&false=false / false&false = false

        if (transitionWithTime != null)
        {
            bCheckCondResult &= transitionWithTime.TimeConditionChk();
        }
        
        foreach (TransCondWithParam t in arrTransParam)
        {
            bCheckCondResult &= t.ConditionCheck(pFSM);
        }


        return bCheckCondResult;
    }

    public bool TimeConditionCheck(FSM pFSM)
    {
        if (transitionWithTime != null)
        {
            if (transitionWithTime.TimeConditionChk())
                return CondtionCheck(pFSM);
        }

        return false;
    }

    public void SetTransTime(float transTime)
    {
        if (transitionWithTime == null)
        {
            transitionWithTime = new TransCondWithTime(transTime);

            if(ownerState != null)
                transitionWithTime.SetOwnerState(ownerState);
        }
        else
        {
            transitionWithTime.m_fConditionTimeValue = transTime;
        }
    }

    public void RemoveTransTime()
    {
        transitionWithTime = null;
    }

    public void SetNextStateID(STATE_ID nextID)
    {
        nextStateID = nextID;
    }
}

[System.Serializable]
public class TransCondWithTime
{
    public float m_fConditionTimeValue = 0;
    float fStartTime = 0, fPauseTIme = 0, fPauseInterval = 0;

    public TransCondWithTime(float condTime)
    {
        m_fConditionTimeValue = condTime;
    }

    public void SetOwnerState(State OwnerState)
    {
        OwnerState.EventStart += OwnerStart;
        OwnerState.EventEnd += OwnerEnd;
        OwnerState.EventPause += OwnerPause;
        OwnerState.EventResume += OwnerResume;
    }

    void OwnerStart(TRANS_ID transID, STATE_ID stateid, STATE_ID preStateID)
    {
        fStartTime = UnityEngine.Time.realtimeSinceStartup;
        fPauseTIme = 0;
        fPauseInterval = 0;
#if UNITY_EDITOR
        //Debug.Log("TransCondWithTime Start : " + stateid.ToString() + " / " + fStartTime.ToString());
#endif
    }

    void OwnerEnd(TRANS_ID transID, STATE_ID stateid, STATE_ID preStateID)
    {
    }

    void OwnerPause()
    {
        fPauseTIme = UnityEngine.Time.realtimeSinceStartup;
    }

    void OwnerResume()
    {
        if(fPauseTIme != 0)
            fPauseInterval = UnityEngine.Time.realtimeSinceStartup - fPauseTIme;
    }

    public void ResetStartTime()
    {
        fStartTime = UnityEngine.Time.realtimeSinceStartup;
        fPauseTIme = 0;
        fPauseInterval = 0;
    }

    public bool TimeConditionChk()
    {
        if (UnityEngine.Time.realtimeSinceStartup - fStartTime - fPauseInterval >= m_fConditionTimeValue)
            return true;

        return false;
    }
}

public class TransCondWithParam
{
    public TRANS_PARAM_ID m_uiParamID {get; private set;}

    public TransitionType m_eTransType { get; private set; }
    public TransitionComparisonOperator m_eCompOperator { get; private set; }

    const string warningmsg = "사용하지 않는 전이조건 변수를 설정했습니다.";

    int m_iConditionValue = 0;  
    public int M_iConditionValue
    {
        get 
        { 
            return m_iConditionValue; 
        }
        set
        {
            if (m_eTransType != TransitionType.INT) 
                Debug.LogWarning(warningmsg);

            m_iConditionValue = value;
        }
    }

    float m_fConditionValue = 0;
    public float M_fConditionValue
    {
        get { return m_fConditionValue; }
        set
        {
            if (m_eTransType != TransitionType.FLOAT) 
                Debug.LogWarning(warningmsg);

            m_fConditionValue = value;
        }
    }

    bool m_bConditionValue = false;
    public bool M_bConditionValue
    {
        get { return m_bConditionValue; }
        set
        {
            if (m_eTransType != TransitionType.BOOL)
                Debug.LogWarning(warningmsg);

            m_bConditionValue = value;
        }
    }

    TRANS_PARAM_ID m_TriggerID = 0;
    public TRANS_PARAM_ID M_TriggerID
    {
        get { return m_TriggerID; }
        set
        {
            if (m_eTransType != TransitionType.TRIGGER)
                Debug.LogWarning(warningmsg);

            m_TriggerID = value;
        }
    }
    
    /// <summary>
    /// 해당 FSM이 가지고 있는 파라메터의 값들과 설정된 값을 비교해서 조건을 만족하는지 검사할 수 있다. 
    /// </summary>
    /// <param name="_paramID">FSM이 가지고 있는 파라메터 ID를 입력한다. </param>
    public TransCondWithParam(TransitionType eT, TRANS_PARAM_ID _paramID=0, object conditionValue=null, TransitionComparisonOperator eCompOp=TransitionComparisonOperator.EQUALS)
    {
        m_eTransType = eT;

        if (eT != TransitionType.TRIGGER && _paramID == 0)
            Debug.LogError("파라메터 아이디를 설정해야 합니다!");

        m_uiParamID = _paramID;

        if (eT != TransitionType.TRIGGER && conditionValue == null)
            Debug.LogError("조건 변수를 설정해야 합니다. ");

        switch (eT)
        {
            case TransitionType.BOOL:
                M_bConditionValue = (bool)conditionValue;
                break;
            case TransitionType.FLOAT:
                M_fConditionValue = (float)conditionValue;
                break;
            case TransitionType.INT:
                M_iConditionValue = (int)conditionValue;
                break;
            case TransitionType.TRIGGER:
                M_TriggerID = _paramID;
                break;
        }

        m_eCompOperator = eCompOp;
    }

    public void SetTransitionType(TransitionType e)
    {
        m_eTransType = e;
    }

    public void SetParamID(TRANS_PARAM_ID _paramID)
    {
        m_uiParamID = _paramID;

        if (m_eTransType == TransitionType.TRIGGER)
            M_TriggerID = _paramID;
    }

    public void SetConditionValue(object conditionValue)
    {
        switch (m_eTransType)
        {
            case TransitionType.BOOL:
                M_bConditionValue = (bool)conditionValue;
                break;
            case TransitionType.FLOAT:
                M_fConditionValue = (float)conditionValue;
                break;
            case TransitionType.INT:
                M_iConditionValue = (int)conditionValue;
                break;
        }
    }

    public void SetCompOperator(TransitionComparisonOperator eOp)
    {
        m_eCompOperator = eOp;
    }


    public bool ConditionCheck(FSM pFSM)
    {
        switch (m_eTransType)
        {
            case TransitionType.INT:
                return IntTypeCondtionChk(m_iConditionValue, pFSM);
            case TransitionType.FLOAT:
                return FloatTypeCondtionChk(m_fConditionValue, pFSM);
            case TransitionType.BOOL:
                return BoolTypeConditionChk(m_bConditionValue, pFSM);
            case TransitionType.TRIGGER:
                return TriggerTypeConditionChk(m_TriggerID, pFSM);
        }

        return false;
    }

    bool IntTypeCondtionChk(int value, FSM pFSM)
    {
        switch (m_eCompOperator)
        {
            case TransitionComparisonOperator.EQUALS:
                if (pFSM.dicIntParam[m_uiParamID] == value)
                    return true;
                break;
            case TransitionComparisonOperator.NOTEQUAL:
                if (pFSM.dicIntParam[m_uiParamID] != value)
                    return true;
                break;
            case TransitionComparisonOperator.GREATER:
                if (pFSM.dicIntParam[m_uiParamID] > value)
                    return true;
                break;
            case TransitionComparisonOperator.LESS:
                if (pFSM.dicIntParam[m_uiParamID] < value)
                    return true;
                break;
        }

        return false;
    }

    bool FloatTypeCondtionChk(float value, FSM pFSM)
    {
        switch (m_eCompOperator)
        {
            case TransitionComparisonOperator.EQUALS:
                //Debug.LogWarning("float 변수는 Equal 조건을 만족시키지 못 할 위험이 큽니다. Float에는 이 조건을 쓰지마세요");
                if (pFSM.dicFloatParam[m_uiParamID] == value)
                    return true;
                break;
            case TransitionComparisonOperator.NOTEQUAL:
                //Debug.LogWarning("float 변수는 NotEqual 조건을 사용하면 항상 만족할 가능성이 큽니다. Float에는 이 조건을 쓰지마세요");
                if (pFSM.dicFloatParam[m_uiParamID] != value)
                    return true;
                break;
            case TransitionComparisonOperator.GREATER:
                if (pFSM.dicFloatParam[m_uiParamID] >= value)
                    return true;
                break;
            case TransitionComparisonOperator.LESS:
                if (pFSM.dicFloatParam[m_uiParamID] <= value)
                    return true;
                break;
        }

        return false;
    }

    bool BoolTypeConditionChk(bool value, FSM pFSM)
    {
        switch (m_eCompOperator)
        {
            case TransitionComparisonOperator.LESS:
            case TransitionComparisonOperator.GREATER:
                Debug.LogWarning("bool 변수에 잘못된 조건연산자를 지정했음");
                return false;
            case TransitionComparisonOperator.EQUALS:
                if (pFSM.dicBoolParam[m_uiParamID] == value)
                    return true;
                break;
            case TransitionComparisonOperator.NOTEQUAL:
                if (pFSM.dicBoolParam[m_uiParamID] != value)
                    return true;
                break;
        }

        return false;        
    }

    bool TriggerTypeConditionChk(TRANS_PARAM_ID triggerID, FSM pFSM)
    {
        if (triggerID == pFSM.triggerID)
            return true;

        return false;
    }
}