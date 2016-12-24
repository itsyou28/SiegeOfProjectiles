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

    public void OnClickMeteo()
    {
        BK_EMC.Inst.NoticeEventOccurrence(BK_EVENT.SKILL_ACTIVE_METEO);

        StopAllCoroutines();
        StartCoroutine(Delay2Sec(BK_EVENT.SKILL_DEACTIVE_METEO));
    }

    public void OnClickObstacle()
    {
        BK_EMC.Inst.NoticeEventOccurrence(BK_EVENT.SKILL_ACTIVE_OBSTACLE);

        StopAllCoroutines();
        StartCoroutine(Delay2Sec(BK_EVENT.SKILL_DEACTIVE_OBSTACLE));
    }

    public void OnClickGlobalAttack()
    {
        StartGlobalAttack();
    }

    IEnumerator Delay2Sec(BK_EVENT targetKey)
    {
        yield return new WaitForSeconds(2);

        BK_EMC.Inst.NoticeEventOccurrence(targetKey);
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
