using UnityEngine;
using System.Collections;

public class BtnClick : MonoBehaviour
{

    public void OnClickStart()
    {
        FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    public void OnClickStar()
    {
        FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_REINFORCE);
    }

    public void OnClickMeteo()
    {
        BK_EMC.Inst.NoticeEventOccurrence(BK_EVENT.SKILL_ACTIVE_METEO);
    }

    public void OnClickObstacle()
    {
        BK_EMC.Inst.NoticeEventOccurrence(BK_EVENT.SKILL_ACTIVE_OBSTACLE);
    }

    public void OnClickGlobal()
    {
        BK_EMC.Inst.NoticeEventOccurrence(BK_EVENT.SKILL_ACTIVE_GLOBALATTACK);
    }

    public void OnClickAchivement()
    {
    }

    public void OnClickRanking()
    {
    }
    
    public void OnClickNoAD()
    {
    }

    public void OnClickExit()
    {
        FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_CLOSE);
    }

    public void OnClickCancel()
    {
        FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_BACKBTN);
    }

}
