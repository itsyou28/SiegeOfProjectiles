using UnityEngine;
using System.Collections;

public class E_Defender : Enemy
{


    protected override void SearchTarget()
    {
        base.SearchTarget();

        attackRange = Random.Range(15, 20);

        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    protected override void MoveToTarget()
    {
        transform.Translate(Vector3.left * 5 * Time.deltaTime);

        if (transform.position.x < attackRange)
            myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }

    public override void OnShield()
    {
    }

    protected override void CreateFSM()
    {
        base.CreateFSM();
        
        //myFSM.GetAnyState().AddTransition(
        //    new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
        //        new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_SearchTarget,
            new TransitionCondition(STATE_ID.Enemy_Move, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Move,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_Attack, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Attack,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_Idle, 0, 1.2f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Idle,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_Attack, 0, 0.5f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Damage,
            new TransitionCondition(STATE_ID.Enemy_Dead, 0, 0.75f,
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 1, TransitionComparisonOperator.LESS)),
            new TransitionCondition(STATE_ID.HistoryBack, TRANS_ID.HISTORY_BACK, 0.4f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Dead,
            new TransitionCondition(STATE_ID.Enemy_DestroySelf, TRANS_ID.TIME, 1.6f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_DestroySelf);

        myFSM.EventStateChange += OnChangeState;

        myFSM.Resume();

        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_RESET);
    }

    protected override void OnChangeState(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        base.OnChangeState(transID, stateID, preStateID);

        switch (stateID)
        {
            case STATE_ID.Enemy_SearchTarget:
                SearchTarget();
                break;
            case STATE_ID.Enemy_Move:
                _ani.Play("E_D_Move");
                break;
            case STATE_ID.Enemy_Idle:
                _ani.Play("E_D_Idle");
                break;
            case STATE_ID.Enemy_Attack:
                _ani.Play("E_D_Attack");
                break;
            case STATE_ID.Enemy_Damage:
                _ani.Play("E_D_Damage");
                break;
            case STATE_ID.Enemy_Dead:
                _ani.Play("E_D_Die");
                break;
            case STATE_ID.Enemy_DestroySelf:
                DestroySelf();
                break;
        }

    }
}
