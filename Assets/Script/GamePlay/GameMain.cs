using UnityEngine;
using System.Collections;

public class GameMain : MonoBehaviour
{
    public Animator aniMainCam;
    public Animator aniBGCam;

    enum CamMode
    {
        FocusOnField,
        FocusOnNightSky
    }

    CamMode curCamMode = CamMode.FocusOnNightSky;

    void Awake()
    {
        State tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.Exit);
        tstate.EventStart += OnStartExit;
            
        tstate = FSM_Manager.GetState(FSM_LAYER.USERSTORY, FSM_ID.USERSTORY, STATE_ID.US_Reinforce);
        tstate.EventStart += OnStartReinforce;
        tstate.EventEnd += OnEndReinforce;

        FSM_Manager.RegisterEventChangeLayerState(FSM_LAYER.USERSTORY, OnChangeUserStory);
    }

    void OnChangeUserStory(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        switch(stateID)
        {
            case STATE_ID.US_StageGuide:
            case STATE_ID.US_Play:
            case STATE_ID.US_Reinforce:
            case STATE_ID.US_StageClear:
                if(curCamMode == CamMode.FocusOnNightSky)
                    SetCamMode(CamMode.FocusOnField);
                break;
            case STATE_ID.US_ExitConfirm:
                break;
            default:
                if (curCamMode == CamMode.FocusOnField)
                    SetCamMode(CamMode.FocusOnNightSky);
                break;
        }
    }

    private void SetCamMode(CamMode targetMode)
    {
        curCamMode = targetMode;
        aniMainCam.SetTrigger("Next");
        aniBGCam.SetTrigger("Next");
    }

    private void OnEndReinforce(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Time.timeScale = 1;
    }

    private void OnStartReinforce(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Time.timeScale = 0;
    }

    private void OnStartExit(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Quit();
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
    
}
