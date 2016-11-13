using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Assault = 0,
    Shooter,
    Defender
}


public class EnemyTest : MonoBehaviour
{
    public Animator _ani;

    FSM myFSM;

    STATE_ID curState = STATE_ID.Enemy_Move;

    EnemyType myType = EnemyType.Assault;

    float attackRange = -61;

    void Awake()
    {
        myType = (EnemyType)Random.Range(0, 2);
        CreateFSM();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _ani.Play("Attack");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _ani.Play("run");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            _ani.Play("Dead");

        if(curState == STATE_ID.Enemy_Move)
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime);

            if (transform.position.x < attackRange)
                myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
        }

        myFSM.TimeCheck();
    }

    public void OnTriggerEnter()
    {
        myFSM.SetInt(TRANS_PARAM_ID.INT_HP, myFSM.GetParamInt(TRANS_PARAM_ID.INT_HP) - 1);
        Debug.Log(myFSM.GetParamInt(TRANS_PARAM_ID.INT_HP));
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
        switch(myType)
        {
            case EnemyType.Assault:
                attackRange = -61;
                break;
            case EnemyType.Shooter:
                attackRange = 0;
                break;
        }
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    void CreateFSM()
    {
        myFSM = new FSM(FSM_ID.NONE);

        myFSM.AddParamInt(TRANS_PARAM_ID.INT_HP, 3);

        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, 0,
            new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET)));
        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_Dead, 0, 0,
            new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 1, TransitionComparisonOperator.LESS)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_SearchTarget,
            new TransitionCondition(STATE_ID.Enemy_Move, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));
        myFSM.MakeStateFactory(STATE_ID.Enemy_Move,
            new TransitionCondition(STATE_ID.Enemy_Attack, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));
        myFSM.MakeStateFactory(STATE_ID.Enemy_Attack);

        myFSM.MakeStateFactory(STATE_ID.Enemy_Dead, 
            new TransitionCondition(STATE_ID.Enemy_DestroySelf, TRANS_ID.TIME, 0.55f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_DestroySelf);

        myFSM.EventStateChange += OnChangeState;

        myFSM.Resume();

        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_RESET);
    }

    private void OnChangeState(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Debug.Log(stateID);
        curState = stateID;
        switch(stateID)
        {
            case STATE_ID.Enemy_SearchTarget:
                SearchTarget();
                break;
            case STATE_ID.Enemy_Attack:
                switch(myType)
                {
                    case EnemyType.Assault:
                        _ani.Play("Attack");
                        break;
                    case EnemyType.Shooter:
                        _ani.Play("Shoot");
                        break;
                }
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
