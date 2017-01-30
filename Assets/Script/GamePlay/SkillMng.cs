using UnityEngine;
using System.Collections;

public interface iSkill
{
    void OnClickMeteo();
    void OnClickObstacle();
    void OnClickGlobalAttack();
}

public class EmptySkillMng : iSkill
{
    public void OnClickMeteo() { }
    public void OnClickObstacle() { }
    public void OnClickGlobalAttack() { }
}

public class SkillMng : MonoBehaviour, iSkill
{
    private static SkillMng instance = null;
    public static iSkill Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SkillMng>();

            return instance;
        }
    }

    SkillData meteo;
    SkillData obstacle;
    SkillData globalAttack;

    //SkillData Use() 를 InputMode에서 호출할 수 있어야 한다. 
    //Deactive 이벤트를 SkillData에서 발생시켜야 한다. 
    //skillEnable 도 SkillData에서 관리해야 한다. 
    
    void Start()
    {
        meteo = new SkillData(5, 10, UI_ID.Skill1, BK_EVENT.SKILL_ACTIVE_METEO, BK_EVENT.SKILL_DEACTIVE_METEO);
        obstacle = new SkillData(10, 10, UI_ID.Skill2, BK_EVENT.SKILL_ACTIVE_OBSTACLE, BK_EVENT.SKILL_DEACTIVE_OBSTACLE);
        globalAttack = new SkillData(1, 10, UI_ID.Skill3, BK_EVENT.SKILL_ACTIVE_GLOBALATTACK, BK_EVENT.DEFAULT_EVENT);
    }

    public void OnClickMeteo()
    {
        if (meteo.isActive)
            meteo.SetActive(false);
        else
            meteo.SetActive(true);
    }

    public void OnClickObstacle()
    {
        if (obstacle.isActive)
            obstacle.SetActive(false);
        else
            obstacle.SetActive(true);
    }

    public void OnClickGlobalAttack()
    {
        StartGlobalAttack();
    }

    void Update()
    {
        meteo.Update();
        obstacle.Update();
        globalAttack.Update();
    }
    
    void StartGlobalAttack()
    {
        StartCoroutine(GlobalAttack());
    }

    IEnumerator GlobalAttack()
    {
        iEnemyControl[] tList = EnemyList.list.ToArray();

        foreach (iEnemyControl iEC in tList)
        {
            if (iEC != null)
                iEC.OnGlobalAttack();

            yield return new WaitForSeconds(0.1f);
        }
    }
}

public class SkillData
{
    public bool isActive { get; private set; } //현재 Skill 사용중 여부
    
    bool isCharging = false; //스킬이 활성화되면 충전이 중지된다. 

    int maxNumber;  //Skill 최대 충전시 사용할 수 있는 Skill 횟수
    float maxTime;  //Skill 최대 충전에 걸리는 시간

    float oneCost;              //Skill 1회 사용 비용
    float chargeSpeed;          //Skill 충전속도
    Bindable<float> curCharge;  //Skill 현재 충전량
    Bindable<bool> enable;      //가능한 사용량을 모두 사용했을 경우 false로 변경

    BK_EVENT activeMsg;
    BK_EVENT deactiveMsg;

    public SkillData(int maxNumber, float maxTime, UI_ID targetUI, BK_EVENT activeMsg, BK_EVENT deactiveMsg)
    {
        curCharge = new Bindable<float>();
        enable = new Bindable<bool>();

        this.activeMsg = activeMsg;
        this.deactiveMsg = deactiveMsg;

        LevelUp(maxNumber, maxTime);
        UIBinder.Inst.Bind<float>(curCharge, targetUI);

        curCharge.Value = 1;
        enable.Value = true;
    }

    public void LevelUp(int maxNumber, float maxTime)
    {
        this.maxNumber = maxNumber;
        this.maxTime = maxTime;

        oneCost = 1.0f / maxNumber;
        chargeSpeed = 1.0f / maxTime;
    }

    public void Update()
    {
        if (!isCharging)
            return;

        curCharge.Value += Time.deltaTime * chargeSpeed;

        if (curCharge.Value >= 1)
            isCharging = false;

        if (curCharge.Value >= oneCost)
            enable.Value = true;
    }

    public void Use()
    {
        curCharge.Value -= oneCost;

        if(curCharge.Value < oneCost)
        {
            enable.Value = false;
            SetActive(false);
        }
    }

    public void SetActive(bool isActive)
    {
        isCharging = !isActive;
        this.isActive = isActive;

        if (isActive)
            BK_EMC.Inst.NoticeEventOccurrence(activeMsg);
        else
            BK_EMC.Inst.NoticeEventOccurrence(deactiveMsg);
    }
}