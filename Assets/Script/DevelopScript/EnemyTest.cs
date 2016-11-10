using UnityEngine;
using System.Collections;

public class EnemyTest : MonoBehaviour
{
    public Animator _ani;

    FSM myFSM;

    void Awake()
    {
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

        transform.Translate(Vector3.left * 5 * Time.deltaTime);

        myFSM.TimeCheck();
    }

    void OnTriggerEnter()
    {
        myFSM.SetInt(TRANS_PARAM_ID.INT_HP, myFSM.GetParamInt(TRANS_PARAM_ID.INT_HP) - 1);
        Debug.Log(myFSM.GetParamInt(TRANS_PARAM_ID.INT_HP));
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void SearchTarget()
    {
    }

    void CreateFSM()
    {
        myFSM = new FSM(FSM_ID.NONE);

        myFSM.AddParamInt(TRANS_PARAM_ID.INT_HP, 5);

        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, 0,
            new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET)));
        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_Dead, 0, 0,
            new TransCondWithParam(TransitionType.INT, TRANS_PARAM_ID.INT_HP, 1, TransitionComparisonOperator.LESS)));

        myFSM.MakeStateFactory(STATE_ID.Enemy_SearchTarget);
        myFSM.MakeStateFactory(STATE_ID.Enemy_Move);
        myFSM.MakeStateFactory(STATE_ID.Enemy_Attack);

        myFSM.MakeStateFactory(STATE_ID.Enemy_Dead, 
            new TransitionCondition(STATE_ID.Enemy_DestroySelf, TRANS_ID.TIME, 0.5f));

        myFSM.MakeStateFactory(STATE_ID.Enemy_DestroySelf);

        myFSM.EventStateChange += OnChangeState;

        myFSM.Resume();

        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_RESET);
    }

    private void OnChangeState(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        Debug.Log(stateID);
        switch(stateID)
        {
            case STATE_ID.Enemy_Dead:
                _ani.Play("Dead");
                break;
            case STATE_ID.Enemy_DestroySelf:
                DestroySelf();
                break;
        }
    }
}
