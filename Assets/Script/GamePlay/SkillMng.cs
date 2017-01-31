using UnityEngine;
using System.Collections;

public interface iSkill
{
    void OnClickMeteo();
    void OnClickObstacle();
    void OnClickGlobalAttack();
    Skill GetMeteoSkillData();
    Skill GetObstacleSkillData();
}

public class EmptySkillMng : iSkill
{
    public void OnClickMeteo() { }
    public void OnClickObstacle() { }
    public void OnClickGlobalAttack() { }
    public Skill GetMeteoSkillData() { return null; }
    public Skill GetObstacleSkillData() { return null; }
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

    Skill meteo;
    Skill obstacle;
    Skill globalAttack;

    public Skill GetMeteoSkillData()
    {
        return meteo;
    }

    public Skill GetObstacleSkillData()
    {
        return obstacle;
    }
    
    //SkillData Use() 를 InputMode에서 호출할 수 있어야 한다. 
    //Deactive 이벤트를 SkillData에서 발생시켜야 한다. 
    //skillEnable 도 SkillData에서 관리해야 한다. 
    
    void Awake()
    {
        meteo = new Skill(5, 10, 0, BK_EVENT.SKILL_ACTIVE_METEO, BK_EVENT.SKILL_DEACTIVE_METEO);
        obstacle = new Skill(10, 10, 1, BK_EVENT.SKILL_ACTIVE_OBSTACLE, BK_EVENT.SKILL_DEACTIVE_OBSTACLE);
        globalAttack = new Skill(1, 10, 2, BK_EVENT.SKILL_ACTIVE_GLOBALATTACK, BK_EVENT.DEFAULT_EVENT);
    }

    public void OnClickMeteo()
    {
        if (meteo.isActive)
            meteo.SetActive(false);
        else if(meteo.isEnable)
            meteo.SetActive(true);
    }

    public void OnClickObstacle()
    {
        if (obstacle.isActive)
            obstacle.SetActive(false);
        else if(obstacle.isEnable)
            obstacle.SetActive(true);
    }

    public void OnClickGlobalAttack()
    {
        if(globalAttack.isEnable)
        {
            globalAttack.Use();
            StartCoroutine(GlobalAttack());
        }
    }

    void Update()
    {
        meteo.Update();
        obstacle.Update();
        globalAttack.Update();
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
