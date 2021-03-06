﻿using UnityEngine;
using System.Collections;

public class GlobalShooterList : GlobalList<E_Shooter>
{
}

public class E_Shooter : Enemy
{
    [SerializeField]
    Transform protectPos;

    [SerializeField]
    Transform firePos;

    Tower targetTower = null;
    Vector3 targetPos = Vector3.zero;
    Vector3 vDir = Vector3.zero;
    Vector3 attackPos = Vector3.zero;

    public event deleFunc eventDestroyShooter;

    public Transform GetProtectPos()
    {
        return protectPos;
    }

    protected override void Awake()
    {
        GlobalShooterList.Add(this);
        base.Awake();
    }

    protected override void SearchTarget()
    {
        StartCoroutine(Search());
    }

    IEnumerator Search()
    {
        yield return true;

        targetTower = GlobalTowerInfo.GetRandomItem();

        if (targetTower == null)
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, false);
        else
        {
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, true);

            attackPos = targetTower.GetCorePos();
            targetTower.eventDestroyTower += OnTargetDestroyed;
            targetPos = targetTower.GetRandomFrontPos();

            vDir = targetPos - transform.position;
            vDir = vDir.normalized;
        }

        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    protected override void MoveToTarget()
    {
        transform.Translate(vDir * tMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) <= attackRange)
        {
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, true);
            myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
        }
    }
    

    void OnTargetDestroyed()
    {
        myFSM.SetBool(TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, false);
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_TARGET_DESTROYED);
    }

    protected override void CreateFSM()
    {
        base.CreateFSM();

        myFSM.GetAnyState().AddTransition(
            new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_TARGET_DESTROYED),
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 0, TransitionComparisonOperator.GREATER)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_SearchTarget,
            new TransitionCondition(STATE_ID.Enemy_Move, 0, 0,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_HAVE_TARGET, true),
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)),
            new TransitionCondition(STATE_ID.Enemy_Idle, 0, 0,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_HAVE_TARGET, false),
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Move,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_Attack, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Attack,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_Idle, 0, attackTime));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Idle,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, IdleTime,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_HAVE_TARGET, false)),
            new TransitionCondition(STATE_ID.Enemy_Move, 0, IdleTime,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_HAVE_TARGET, true),
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, false)),
            new TransitionCondition(STATE_ID.Enemy_Attack, 0, IdleTime,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_HAVE_TARGET, true),
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, true)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Damage,
            new TransitionCondition(STATE_ID.Enemy_Dead, 0, damageTime,
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 1, TransitionComparisonOperator.LESS)),
            new TransitionCondition(STATE_ID.Enemy_Idle, 0, damageTime));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Dead,
            new TransitionCondition(STATE_ID.Enemy_DestroySelf, TRANS_ID.TIME, deadTime));

        myFSM.MakeStateFactory(STATE_ID.Enemy_DestroySelf);

        myFSM.EventStateChange += OnChangeState;

        myFSM.Resume();
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
                _ani.Play("E_S_Move");
                break;
            case STATE_ID.Enemy_Idle:
                _ani.Play("E_S_Idle");
                break;
            case STATE_ID.Enemy_Attack:
                _ani.Play("E_S_Attack");
                break;
            case STATE_ID.Enemy_Damage:
                _ani.Play("E_S_Damage");
                break;
            case STATE_ID.Enemy_Dead:
                _ani.Play("E_S_Die");
                break;
            case STATE_ID.Enemy_DestroySelf:
                DestroySelf();
                break;
        }

    }

    public void Fire()
    {
        if(targetTower != null)
        {
            Projectile bullet = E_ProjectilePool.Inst.Pop();
            Vector3 tPos = attackPos;
            tPos.x += Random.Range(0, 10) - 5;
            tPos.z += Random.Range(0, 10) - 5;
            tPos.y = 0;
            tPos.x -= 5;
            bullet.Fire(firePos.position, tPos, 30);
        }
    }

    protected override void DestroySelf()
    {
        if(eventDestroyShooter != null)
            eventDestroyShooter();
        base.DestroySelf();
    }
}
