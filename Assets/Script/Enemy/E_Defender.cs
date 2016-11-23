using UnityEngine;
using System.Collections;

public class E_Defender : Enemy
{
    enum BehaviorMode
    {
        PROTECT = 0,
        ATTACK
    }

    Tower targetTower = null;
    Vector3 targetPos = Vector3.zero;
    Vector3 vDir = Vector3.zero;

    E_Shooter targetShooter = null;
    Transform targetTransform;

    BehaviorMode curMode = BehaviorMode.PROTECT;

    protected override void SearchTarget()
    {
        StartCoroutine(Search());
    }

    IEnumerator Search()
    {
        int rand = Random.Range(0, 10);
        if (rand < 7)
        {
            curMode = BehaviorMode.PROTECT;
            myFSM.SetInt_NoCondChk(TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE, (int)BehaviorMode.PROTECT);
            SetTargetForProtect();
        }
        else
        {
            curMode = BehaviorMode.ATTACK;
            myFSM.SetInt_NoCondChk(TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE, (int)BehaviorMode.ATTACK);
            SetTargetForAttack();
        }

        yield return true;

        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    void SetTargetForProtect()
    {
        targetShooter = GlobalShooterList.GetRandomItem();

        if (targetShooter == null)
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, false);
        else
        {
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, true);

            targetShooter.eventDestroyShooter += OnTargetDestroyed;
            targetTransform = targetShooter.GetProtectPos();
        }
    }

    private void SetTargetForAttack()
    {
        targetTower = GlobalTowerInfo.GetRandomItem();

        if (targetTower == null)
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, false);
        else
        {
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, true);

            targetTower.eventDestroyTower += OnTargetDestroyed;
            targetPos = targetTower.GetRandomFrontPos();

            vDir = targetPos - transform.position;
            vDir = vDir.normalized;
        }
    }

    float accumeTime = 0;
    protected override void Update()
    {
        if(curState == STATE_ID.Enemy_Protection)
        {
            accumeTime += Time.deltaTime;
            if(accumeTime > 0.5f)
            {                
                if (targetTransform != null && Vector3.Distance(transform.position, targetTransform.position) > 0.5f)
                {
                    myFSM.SetBool(TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, false);
                }

                accumeTime = 0;
            }
        }

        base.Update();
    }

    protected override void MoveToTarget()
    {
        switch(curMode)
        {
            case BehaviorMode.PROTECT:
                MoveToProtect();
                break;
            case BehaviorMode.ATTACK:
                MoveToAttack();
                break;
        }
    }

    private void MoveToProtect()
    {
        if (transform == null)
            return;

        vDir = targetTransform.position - transform.position;
        vDir = vDir.normalized;

        transform.Translate(vDir * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetTransform.position) <= 0.5f)
        {
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, true);
            myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
        }
    }

    private void MoveToAttack()
    {
        transform.Translate(vDir * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) <= attackRange)
        {
            myFSM.SetBool(TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, true);
            myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
        }
    }

    public override void OnDamage()
    {
        base.OnDamage();
    }

    public override void OnShield()
    {
    }

    void OnTargetDestroyed()
    {
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_TARGET_DESTROYED);
    }

    protected override void CreateFSM()
    {
        base.CreateFSM();

        myFSM.AddParamInt(TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE);

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
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE, (int)BehaviorMode.ATTACK),
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)),
            new TransitionCondition(STATE_ID.Enemy_Protection, 0, 0,
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE, (int)BehaviorMode.PROTECT),
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_Protection,
            new TransitionCondition(STATE_ID.Enemy_Damage, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_HIT)),
            new TransitionCondition(STATE_ID.Enemy_Move, 0, 0,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, false)));

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
            new TransitionCondition(STATE_ID.Enemy_Idle, 0, damageTime,
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE, (int)BehaviorMode.ATTACK)),
            new TransitionCondition(STATE_ID.Enemy_Protection, 0, damageTime,
                new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_DEFENDER_BEHAVIOR_MODE, (int)BehaviorMode.PROTECT)));

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
            case STATE_ID.Enemy_Protection:
                _ani.Play("E_D_Idle");
                break;
            case STATE_ID.Enemy_DestroySelf:
                DestroySelf();
                break;
        }
    }
}
