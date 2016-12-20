using System;
using System.Collections;
using System.Collections.Generic;

public enum BK_EVENT
{
    DEFAULT_EVENT = 0,
    OnChangeInputMode,
    SKILL_ACTIVE_METEO,
    SKILL_ACTIVE_OBSTACLE,
    SKILL_ACTIVE_GLOBALATTACK
}

public delegate void EventCallBackFunction(params object[] args);

//Event Message Center
public class BK_EMC
{
    private static BK_EMC instance;

    Dictionary<BK_EVENT, EventCallBackFunction>
        callBackFunctionList = new Dictionary<BK_EVENT, EventCallBackFunction>();

    Dictionary<BK_EVENT, EventCallBackFunction>
        callBackFunctionListBefore = new Dictionary<BK_EVENT, EventCallBackFunction>();

    Dictionary<BK_EVENT, EventCallBackFunction>
        callBackFunctionListAfter = new Dictionary<BK_EVENT, EventCallBackFunction>();

    public static BK_EMC Inst
    {
        get
        {
            if (instance == null)
            {
                instance = new BK_EMC();
            }
            return instance;
        }
    }
    
    /// <param name="atPoint">-1, 0, 1</param>
    public void AddEventCallBackFunction(BK_EVENT key, EventCallBackFunction cbFunction, int atPoint = 0)
    {
        if (atPoint < -1 || atPoint > 1)
            throw new IndexOutOfRangeException("-1 <= atPoint <=1");

        switch (atPoint)
        {
            case -1:
                AddCallBack(ref callBackFunctionListBefore, key, cbFunction);
                break;
            case 0:
                AddCallBack(ref callBackFunctionList, key, cbFunction);
                break;
            case 1:
                AddCallBack(ref callBackFunctionListAfter, key, cbFunction);
                break;
        }
    }

    private void AddCallBack(ref Dictionary<BK_EVENT, EventCallBackFunction> target, BK_EVENT key, EventCallBackFunction cbFunction)
    {
        if (!target.ContainsKey(key))
            target.Add(key, cbFunction);
        else
            target[key] += cbFunction;
    }

    /// <param name="atPoint">-1, 0, 1</param>
    public void RemoveEventCallBackFunction(BK_EVENT key, EventCallBackFunction cbFunction, int atPoint = 0)
    {
        if (atPoint < -1 || atPoint > 1)
            throw new IndexOutOfRangeException("-1 <= atPoint <=1");

        switch (atPoint)
        {
            case -1:
                RemoveCallBack(ref callBackFunctionListBefore, key, cbFunction);
                break;
            case 0:
                RemoveCallBack(ref callBackFunctionList, key, cbFunction);
                break;
            case 1:
                RemoveCallBack(ref callBackFunctionListAfter, key, cbFunction);
                break;
        }
    }

    private void RemoveCallBack(ref Dictionary<BK_EVENT, EventCallBackFunction> target, BK_EVENT key, EventCallBackFunction cbFunction)
    {
        if (!target.ContainsKey(key))
            return;
        else
            target[key] -= cbFunction;
    }

    public void NoticeEventOccurrence(BK_EVENT key, params object[] args)
    {
        if (callBackFunctionListBefore.ContainsKey(key))
        {
            if (callBackFunctionListBefore[key] != null)
                callBackFunctionListBefore[key](args);
        }

        if (callBackFunctionList.ContainsKey(key))
        {
            if (callBackFunctionList[key] != null)
                callBackFunctionList[key](args);
        }

        if (callBackFunctionListAfter.ContainsKey(key))
        {
            if (callBackFunctionListAfter[key] != null)
                callBackFunctionListAfter[key](args);
        }
    }
}
