﻿using UnityEngine;
using System.Collections;

public class E_Assault : Enemy
{


    protected override void SearchTarget()
    {
        base.SearchTarget();

        attackRange = -61;
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

        myFSM.AddParamInt(TRANS_PARAM_ID.INT_HP, 3);

        myFSM.GetAnyState().AddTransition(
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_SearchTarget,
            new TransitionCondition(STATE_ID.Enemy_Move, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Move,
            new TransitionCondition(STATE_ID.Enemy_Attack, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Attack);

        myFSM.MakeStateFactory(STATE_ID.Enemy_Damage,
            new TransitionCondition(STATE_ID.Enemy_Dead, 0, 0.4f, 
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 1, TransitionComparisonOperator.LESS)),
            new TransitionCondition(STATE_ID.HistoryBack, TRANS_ID.HISTORY_BACK, 0.4f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Dead,
            new TransitionCondition(STATE_ID.Enemy_DestroySelf, TRANS_ID.TIME, 0.55f));

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
                _ani.Play("run");
                break;
            case STATE_ID.Enemy_Idle:
                _ani.Play("Idle");
                break;
            case STATE_ID.Enemy_Attack:
                _ani.Play("Attack");
                break;
            case STATE_ID.Enemy_Dead:
                _ani.Play("Dead");
                break;
            case STATE_ID.Enemy_DestroySelf:
                DestroySelf();
                break;
        }

    }
}
