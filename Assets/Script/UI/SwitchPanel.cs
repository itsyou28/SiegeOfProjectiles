using UnityEngine;
using System.Collections;

public class SwitchPanel : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ReinforceBG;
    public GameObject Reinforce;
    public GameObject Play;
    public GameObject StageClear;
    public GameObject GameOver;
    public GameObject ExitConfirm;

    public void Awake()
    {
        State tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_MainMenu);
        tstate.EventStart += OnStartMainMenu;
        tstate.EventEnd += OnEndMainMenu;

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
        Play.SetActive(false);
    }

    private void OnStarPlay(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Play.SetActive(true);
    }

    private void OnEndReinforce(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Reinforce.SetActive(false);
        ReinforceBG.SetActive(false);
        Play.SetActive(false);
    }

    private void OnStartReinforce(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Reinforce.SetActive(true);
        ReinforceBG.SetActive(true);
        Play.SetActive(true);
    }

    private void OnEndMainMenu(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        MainMenu.SetActive(false);
    }

    private void OnStartMainMenu(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        MainMenu.SetActive(true);
    }
}
