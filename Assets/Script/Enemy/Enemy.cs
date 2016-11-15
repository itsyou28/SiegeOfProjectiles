using UnityEngine;
using System.Collections;

public interface iEnemyControl
{
    void OnDamage();
    void OnShield();
}

public class Enemy : MonoBehaviour, iEnemyControl
{
    public Animator _ani;

    protected FSM myFSM;

    protected STATE_ID curState = STATE_ID.Enemy_Move;

    protected float attackRange;

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

    protected virtual void MoveToTarget()
    {
    }

    public virtual void OnDamage()
    {
        _ani.Play("Damage");
        myFSM.SetInt_NoCondChk(TRANS_PARAM_ID.INT_HP, myFSM.GetParamInt(TRANS_PARAM_ID.INT_HP) - 1);
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_HIT);
    }

    public virtual void OnShield()
    {

    }

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected virtual void SearchTarget()
    {
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    protected virtual void CreateFSM()
    {
        myFSM = new FSM(FSM_ID.NONE);

        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, 0,
            new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET)));
        
        
    }

    protected virtual void OnChangeState(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        curState = stateID;
    }

}
