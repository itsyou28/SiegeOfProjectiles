using UnityEngine;
using System.Collections;

public delegate void deleFunc();

public class GlobalTowerInfo : GlobalList<Tower>
{
}

public class Tower : MonoBehaviour
{
    //타워 HP 관리
    //타워 애니메이션 관리
    //접촉 적 공격 처리

    [SerializeField]
    Transform[] frontPos;
    [SerializeField]
    Animator _ani;

    [SerializeField]
    Collider attackCollider;

    [SerializeField]
    Transform corePos;

    [SerializeField]
    ParticleSystem damageEffect;

    [SerializeField]
    ParticleSystem attackEffect;

    int HP = 30;
    bool isDestroyed = false;

    public event deleFunc eventDestroyTower;

    Vector3 attackColWaitPos;
    Vector3 attackColAttackPos;

    FSM attackSequence = new FSM(FSM_ID.NONE);

    void Awake()
    {
        attackColWaitPos = attackCollider.transform.position;
        attackColAttackPos = attackColWaitPos;
        attackColAttackPos.x += 30;
        GlobalTowerInfo.Add(this);

        CreateTowerAttackSequence();
    }

    void CreateTowerAttackSequence()
    {
        attackSequence.AddParamBool(TRANS_PARAM_ID.BOOL_IS_TOWER_HAVE_ENEMY, false);

        attackSequence.GetAnyState().AddTransition(
            new TransitionCondition(STATE_ID.TowerAttack_Wait, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_RESET)));

        attackSequence.MakeStateFactory(STATE_ID.TowerAttack_Wait,
            new TransitionCondition(STATE_ID.TowerAttack_AttackReady, 0, 0,
                new TransCondWithParam(TransitionType.TRIGGER, TRANS_PARAM_ID.TRIGGER_NEXT)));
        attackSequence.MakeStateFactory(STATE_ID.TowerAttack_AttackReady,
            new TransitionCondition(STATE_ID.TowerAttack_EffectStart, 0, 0.8f,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_IS_TOWER_HAVE_ENEMY, true)),
            new TransitionCondition(STATE_ID.TowerAttack_Wait, 0, 0.8f,
                new TransCondWithParam(TransitionType.BOOL, TRANS_PARAM_ID.BOOL_IS_TOWER_HAVE_ENEMY, false)));
        attackSequence.MakeStateFactory(STATE_ID.TowerAttack_EffectStart,
            new TransitionCondition(STATE_ID.TowerAttack_Attack, 0, 1.2f));
        attackSequence.MakeStateFactory(STATE_ID.TowerAttack_Attack,
            new TransitionCondition(STATE_ID.TowerAttack_AttackReady, 0, 0.3f));

        attackSequence.EventStateChange += OnChangeAttackSequence;

        attackSequence.Resume();

        attackSequence.SetTrigger(TRANS_PARAM_ID.TRIGGER_RESET);
    }

    private void OnChangeAttackSequence(TRANS_ID transID, STATE_ID stateID, STATE_ID preStateID)
    {
        switch(stateID)
        {
            case STATE_ID.TowerAttack_Wait:
                break;
            case STATE_ID.TowerAttack_AttackReady:
                attackCollider.transform.position = attackColWaitPos;
                break;
            case STATE_ID.TowerAttack_EffectStart:
                attackSequence.SetBool_NoCondChk(TRANS_PARAM_ID.BOOL_IS_TOWER_HAVE_ENEMY, false);
                attackEffect.Play();
                break;
            case STATE_ID.TowerAttack_Attack:
                attackCollider.transform.position = attackColAttackPos;
                break;
        }
    }
    
    void Update()
    {
        attackSequence.TimeCheck();
    }

    public void OnAttackEnemy()
    {
        attackSequence.SetBool_NoCondChk(TRANS_PARAM_ID.BOOL_IS_TOWER_HAVE_ENEMY, true);
    }
    
    void DestroySelf()
    {
        Debug.Log("Destroy " + gameObject.name);
        isDestroyed = true;

        gameObject.SetActive(false);

        GlobalTowerInfo.Remove(this);
        eventDestroyTower();
    }

    public void Hit()
    {
        //HP가 깍이고 흔들린다
        _ani.Play("Damage");
        damageEffect.Play();
        HP -= 1;

        attackSequence.SetBool(TRANS_PARAM_ID.BOOL_IS_TOWER_HAVE_ENEMY, true);
        attackSequence.SetTrigger(TRANS_PARAM_ID.TRIGGER_NEXT);

        if (HP <= 0 && !isDestroyed)
            DestroySelf();
    }

    public Vector3 GetRandomFrontPos()
    {
        int randIdx = Random.Range(0, frontPos.Length);

        return frontPos[randIdx].position;
    }

    public Vector3 GetCorePos()
    {
        return corePos.position;
    }
}
