using UnityEngine;
using System.Collections;

public class BtnClick : MonoBehaviour
{
    iSkill iSkillMng;

    void Awake()
    {
        iSkillMng = SkillMng.Inst;
        if (iSkillMng == null)
            iSkillMng = new EmptySkillMng();
    }

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
        iSkillMng.OnClickMeteo();
    }

    public void OnClickObstacle()
    {
        iSkillMng.OnClickObstacle();
    }

    public void OnClickGlobal()
    {
        iSkillMng.OnClickGlobalAttack();
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

    public void OnClickNext()
    {
        FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_NEXT);
    }
}
