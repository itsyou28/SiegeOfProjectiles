using UnityEngine;
using System.Collections;

public interface iInput
{
    void OnDown();
    void OnPress();
    void OnUp();
}

public class PlayerInput : MonoBehaviour
{
    iInput curInputTarget;

    public GameObject normalMode;
    public GameObject meteoMode;
    public GameObject obstacleMode;

    iInput iNormalMode;
    iInput iMeteoMode;
    iInput iObstacleMode;
    
    void Awake()
    {
        iNormalMode = normalMode.GetComponent<iInput>();
        iMeteoMode = meteoMode.GetComponent<iInput>();
        iObstacleMode = obstacleMode.GetComponent<iInput>();

        curInputTarget = iNormalMode;

        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_ACTIVE_METEO, OnActiveMeteo);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_ACTIVE_OBSTACLE, OnActiveObstacle);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_ACTIVE_GLOBALATTACK, OnActiveGlobal);
    }

    void OnActiveMeteo(params object[] args)
    {
        curInputTarget = iMeteoMode;
    }

    void OnActiveObstacle(params object[] args)
    {
        curInputTarget = iObstacleMode;
    }

    void OnActiveGlobal(params object[] args)
    {
        StartGlobalAttack();
    }



    void OnChangeInputMode(params object[] args)
    {
        int mode = (int)args[0];

        switch(mode)
        {
            case 0:
                curInputTarget = iNormalMode;
                break;
            case 1:
                break;
            case 2:
                curInputTarget = iObstacleMode;
                break;
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            curInputTarget.OnDown();

        if (Input.GetMouseButton(0))
            curInputTarget.OnPress();

        if (Input.GetMouseButtonUp(0))
            curInputTarget.OnUp();

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            curInputTarget = iNormalMode;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curInputTarget = iMeteoMode;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            curInputTarget = iObstacleMode;
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartGlobalAttack();
        }
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
            if(iEC != null)
                iEC.OnGlobalAttack();

            yield return new WaitForSeconds(0.1f);
        }
    }
}