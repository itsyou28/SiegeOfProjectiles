using UnityEngine;
using System.Collections;

public interface iEnemyControl
{
    void OnDamage();
    void OnShield();
}

public class Enemy : MonoBehaviour
{
    public Animator _ani;

    FSM myFSM;

    STATE_ID curState = STATE_ID.Enemy_Move;

    void Awake()
    {
        CreateFSM();
    }

    void Update()
    {
        if (curState == STATE_ID.Enemy_Move)
        {
            MoveToTarget();
        }

        myFSM.TimeCheck();
    }

    protected void MoveToTarget()
    {
    }

    public void OnDamage()
    {
    }

    public void OnShield()
    {

    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void SearchTarget()
    {
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    protected void CreateFSM()
    {
        myFSM = new FSM(FSM_ID.NONE);

        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, 0,
            new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET)));
        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_Dead, 0, 0,
            new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 1, TransitionComparisonOperator.LESS)));
        
    }

    protected void OnChangeState(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        curState = stateID;
    }

}
