using UnityEngine;
using System.Collections;

public interface iEnemyControl
{
    void OnDamage(int damage=1);
    void OnDestroyedShield(int idx);
    void OnKnuckback(float pushpower);
    void OnMeteo();
    void OnGlobalAttack();
    void OnEnterObstacle();
    void OnExitObstacle();
}

public class EnemyList : GlobalList<iEnemyControl>
{
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
    protected int coreHP;

    [SerializeField]
    protected float attackRange;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    protected float weight = 1; //1~5 높을수록 뒤로 덜 밀린다. 

    protected FSM myFSM;

    protected STATE_ID curState = STATE_ID.Enemy_Move;

    protected float tMoveSpeed;

    float knuckback = 0;
    bool chkRepeat = false;

    TextMesh _text;

    OnShield[] arrShield;
    bool[] arrIsDestroyShield;
    bool allShieldIsDestroyed = false;

    protected virtual void Awake()
    {
        _text = transform.GetComponentInChildren<TextMesh>();

        OnCore[] arrCore = GetComponentsInChildren<OnCore>();
        foreach (OnCore target in arrCore)
            target.iControl = this;

        arrShield = GetComponentsInChildren<OnShield>();
        arrIsDestroyShield = new bool[arrShield.Length];

        for (int idx = 0; idx < arrIsDestroyShield.Length; idx++)
            arrIsDestroyShield[idx] = false;

        for (int idx = 0; idx < arrShield.Length; idx++)
        {
            arrShield[idx].iControl = this;
            arrShield[idx].shieldIdx = idx;
        }

        if (arrShield.Length == 0)
            allShieldIsDestroyed = true;

        CreateFSM();

        EnemyList.Add(this);

        tMoveSpeed = moveSpeed;
    }

    protected virtual void Start()
    {
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_RESET);
    }
    


    protected virtual void Update()
    {        
        if (curState == STATE_ID.Enemy_Move)
        {
            MoveToTarget();
        }

        if(knuckback > 0.1f)
        {
            transform.Translate(Vector3.right * knuckback * Time.deltaTime);
            knuckback -= 50 * Time.deltaTime;
        }

        myFSM.TimeCheck();
    }

    void LateUpdate()
    {
        if (chkRepeat)
            chkRepeat = false;
    }

    protected virtual void MoveToTarget()
    {
    }

    public void OnDamage(int damage=1)
    {
        myFSM.SetInt_NoCondChk(TRANS_PARAM_ID.INT_HP, myFSM.GetParamInt(TRANS_PARAM_ID.INT_HP) - damage);
        
        myFSM.SetTrigger(TRANS_PARAM_ID.TRIGGER_HIT);
    }

    public void OnDestroyedShield(int targetShield)
    {
        arrIsDestroyShield[targetShield] = true;

        bool chkDestroy = true;

        for (int idx = 0; idx < arrIsDestroyShield.Length; idx++)
            chkDestroy &= arrIsDestroyShield[idx];

        if (chkDestroy)
            OnDestroyedAllShield();
    }

    protected virtual void OnDestroyedAllShield()
    {
        allShieldIsDestroyed = true;
    }

    public void OnKnuckback(float pushpower)
    {
        knuckback = pushpower - weight;
    }

    public void OnMeteo()
    {
        if (chkRepeat)
            return;

        OnKnuckback(25);

        if(allShieldIsDestroyed)
            OnDamage(2);
        
        chkRepeat = true;
    }

    public void OnGlobalAttack()
    {
        Projectile bullet = GlobalAttackPool.Inst.Pop();
        bullet.Fire(transform.position);
        StartCoroutine(DelayGlobalAttack());
    }

    IEnumerator DelayGlobalAttack()
    {
        yield return new WaitForSeconds(0.5f);

        if(allShieldIsDestroyed)
            OnDamage(5);
        else
        {
            OnKnuckback(15);
            for(int idx=0; idx<arrShield.Length; idx++)
            {
                if (!arrIsDestroyShield[idx])
                    arrShield[idx].Damage(1);
            }
        }
    }

    public void OnEnterObstacle()
    {
        tMoveSpeed = moveSpeed * 0.1f;
    }

    public void OnExitObstacle()
    {
        tMoveSpeed = moveSpeed;
    }

    protected virtual void DestroySelf()
    {
        EnemyList.Remove(this);
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

        myFSM.AddParamInt(TRANS_PARAM_ID.INT_HP, coreHP);
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
