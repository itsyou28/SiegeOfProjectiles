using UnityEngine;
using System.Collections;

public class SwitchPanel : MonoBehaviour
{
    public GameObject MainMenu;
    public Animator aniMainMenu;

    public GameObject ReinforceBG;
    public GameObject Reinforce;
    public GameObject Play;
    public GameObject StageClear;
    public GameObject GameOver;
    public GameObject ExitConfirm;
    public GameObject Ending;
    public GameObject StageGuide;

    void Start()
    {        
        State tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_MainMenu);
        tstate.EventStart += OnStartMainMenu;
        tstate.EventEnd += OnEndMainMenu;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_Start);
        tstate.EventStart += OnStartStart;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_Reinforce);
        tstate.EventStart += OnStartReinforce;
        tstate.EventEnd += OnEndReinforce;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_Play);
        tstate.EventStart += OnStarPlay;
        tstate.EventEnd += OnEndPlay;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_StageClear);
        tstate.EventStart += OnStarStageClear;
        tstate.EventEnd += OnEndStageClear;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_GameOver);
        tstate.EventStart += OnStartGameOver;
        tstate.EventEnd += OnEndGameOver;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_ExitConfirm);
        tstate.EventStart += OnStartExitConfirm;
        tstate.EventEnd += OnEndExitConfirm;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_Ending);
        tstate.EventStart += OnStartEnding;
        tstate.EventEnd += OnEndEnding;

        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_StageGuide);
        tstate.EventStart += OnStartStageGuide;
        tstate.EventEnd += OnEndStageGuide;

        FSM_Manager.RegisterEventChangeLayerState(FSM_LAYER.USERSTORY, OnChangeUserStory);
    }

    private void OnChangeUserStory(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        switch(stateID)
        {
            //case STATE_ID.US_StageGuide:
            case STATE_ID.US_Play:
            case STATE_ID.US_Reinforce:
            case STATE_ID.US_StageClear:
                Play.SetActive(true);
                break;
            case STATE_ID.US_StageGuide:
                break;
            default:
                Play.SetActive(false);
                break;
        }
    }

    private void OnEndStageGuide(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        StageGuide.SetActive(false);
    }

    private void OnStartStageGuide(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        StageGuide.SetActive(true);
    }

    private void OnStartStart(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        MainMenu.SetActive(true);
    }

    private void OnEndEnding(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Ending.SetActive(false);
    }

    private void OnStartEnding(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Ending.SetActive(true);
    }

    private void OnEndExitConfirm(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        ExitConfirm.SetActive(false);
    }

    private void OnStartExitConfirm(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        ExitConfirm.SetActive(true);
    }

    private void OnEndGameOver(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        GameOver.SetActive(false);
    }

    private void OnStartGameOver(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Play.SetActive(false);
        GameOver.SetActive(true);
    }

    private void OnEndStageClear(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        StageClear.SetActive(false);
    }

    private void OnStarStageClear(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        StageClear.SetActive(true);
    }

    private void OnEndPlay(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
    }

    private void OnStarPlay(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
    }

    private void OnEndReinforce(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Reinforce.SetActive(false);
        ReinforceBG.SetActive(false);
    }

    private void OnStartReinforce(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Reinforce.SetActive(true);
        ReinforceBG.SetActive(true);
    }

    private void OnEndMainMenu(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        aniMainMenu.Play("MainMenuEaseOut");
    }

    private void OnStartMainMenu(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        aniMainMenu.Play("MainMenuEaseIn");
    }
}
