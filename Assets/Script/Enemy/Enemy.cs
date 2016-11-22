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

    [SerializeField]
    protected float damageTime;
    [SerializeField]
    protected float attackTime;
    [SerializeField]
    protected float IdleTime;
    [SerializeField]
    protected float deadTime;

    [SerializeField]
    protected int defaultHP;

    [SerializeField]
    protected float attackRange;

    protected FSM myFSM;

    protected STATE_ID curState = STATE_ID.Enemy_Move;


    TextMesh _text;

    void Awake()
    {
        _text = transform.GetComponentInChildren<TextMesh>();
        CreateFSM();
    }

    void Start()
    {
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_RESET);
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
    }

    protected virtual void CreateFSM()
    {
        myFSM = new FSM(FSM_ID.NONE);
        
        myFSM.GetAnyState().AddTransition(new TransitionCondition(STATE_ID.Enemy_SearchTarget, 0, 0,
            new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET)));

        myFSM.AddParamInt(TRANS_PARAM_ID.INT_HP, defaultHP);
        myFSM.AddParamBool(TRANS_PARAM_ID.BOOL_HAVE_TARGET, false);
        myFSM.AddParamBool(TRANS_PARAM_ID.BOOL_IS_ARRIVE_TARGET, false);


    }

    protected virtual void OnChangeState(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        curState = stateID;

        if(_text != null)
            _text.text = curState.ToString();
    }

}
