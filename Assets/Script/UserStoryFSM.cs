﻿using UnityEngine;
using System.Collections;

public class UserStoryFSM : MonoBehaviour
{
    void Awake()
    {
        CreateUserStoryFSM();
    }

    void Start()
    {
        FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_RESET);
    }

    void Update()
    {
        FSM_Manager.Update();
    }

    private void CreateUserStoryFSM()
    {
        FSM userstory = new FSM(FSM_ID.USERSTORY);

        userstory.GetAnyState().AddTransition(
            new TransitionCondition(STATE_ID.US_Start, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET))
            );

        userstory.MakeStateFactory(STATE_ID.US_Start,
            new TransitionCondition(STATE_ID.US_MainMenu, 0, 1)
            );

        userstory.MakeStateFactory(STATE_ID.US_MainMenu,
            new TransitionCondition(STATE_ID.US_StageGuide, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)),
            new TransitionCondition(STATE_ID.US_ExitConfirm, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.US_ExitConfirm, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_CLOSE))
            );

        userstory.MakeStateFactory(STATE_ID.US_StageGuide,
            new TransitionCondition(STATE_ID.US_Play, 0, 2));

        userstory.MakeStateFactory(STATE_ID.US_Play,
            new TransitionCondition(STATE_ID.US_Reinforce, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.US_Reinforce, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_REINFORCE)),
            new TransitionCondition(STATE_ID.US_StageClear, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_CLEAR)),
            new TransitionCondition(STATE_ID.US_GameOver, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_GAMEOVER)),
            new TransitionCondition(STATE_ID.US_Ending, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ALL_CLEAR))
            );

        userstory.MakeStateFactory(STATE_ID.US_StageClear,
            new TransitionCondition(STATE_ID.US_StageGuide, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.US_StageGuide, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT))
            );

        userstory.MakeStateFactory(STATE_ID.US_Reinforce,
            new TransitionCondition(STATE_ID.US_Play, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.US_Play, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_REINFORCE)),
            new TransitionCondition(STATE_ID.US_ExitConfirm, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_CLOSE))
            );

        userstory.MakeStateFactory(STATE_ID.US_Ending,
            new TransitionCondition(STATE_ID.US_MainMenu, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.US_MainMenu, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT))
            );

        userstory.MakeStateFactory(STATE_ID.US_GameOver,
            new TransitionCondition(STATE_ID.US_MainMenu, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.US_MainMenu, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT))
            );

        userstory.MakeStateFactory(STATE_ID.US_ExitConfirm,
            new TransitionCondition(STATE_ID.HistoryBack, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_ESCAPE)),
            new TransitionCondition(STATE_ID.HistoryBack, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_BACKBTN)),
            new TransitionCondition(STATE_ID.Exit, TRANS_ID.ESCAPE, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_CLOSE))
                );

        userstory.MakeStateFactory(STATE_ID.Exit);

        FSM_Manager.AddFSM(FSM_LAYER.USERSTORY, userstory, FSM_ID.USERSTORY);

        userstory.EventStateChange += OnChangeUserStory;
    }

    private void OnChangeUserStory(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Debug.Log(stateID);
    }
}
