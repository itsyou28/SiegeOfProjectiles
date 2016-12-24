using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public interface iInput
{
    void OnDown(Vector3 hitPos);
    void OnDrag(Vector3 hitPos);
    void OnPressUpdate();
    void OnUp(Vector3 hitPos);

    bool isPress { get; set; }
}

public class PlayerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
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
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_DEACTIVE_METEO, OnDeactiveMeteo);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_ACTIVE_OBSTACLE, OnActiveObstacle);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_DEACTIVE_OBSTACLE, OnDeactiveObstacle);
    }

    void OnActiveMeteo(params object[] args)
    {
        curInputTarget = iMeteoMode;
    }

    void OnDeactiveMeteo(params object[] args)
    {
        curInputTarget = iNormalMode;
    }

    void OnActiveObstacle(params object[] args)
    {
        curInputTarget = iObstacleMode;
    }

    void OnDeactiveObstacle(params object[] args)
    {
        curInputTarget = iNormalMode;
    }

    public void OnPointerDown(PointerEventData _data)
    {
        curInputTarget.isPress = true;
        curInputTarget.OnDown(_data.pointerCurrentRaycast.worldPosition);
    }

    public void OnPointerUp(PointerEventData _data)
    {
        curInputTarget.OnUp(_data.pointerCurrentRaycast.worldPosition);
        curInputTarget.isPress = false;
    }

    public void OnDrag(PointerEventData _data)
    {
        curInputTarget.OnDrag(_data.pointerCurrentRaycast.worldPosition);
    }
        
    void Update()
    {
        if (curInputTarget.isPress)
            curInputTarget.OnPressUpdate();
        
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
            BK_EMC.Inst.NoticeEventOccurrence(BK_EVENT.SKILL_ACTIVE_GLOBALATTACK);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_ESCAPE);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_CLEAR);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_ALL_CLEAR);

        if (Input.GetKeyDown(KeyCode.Alpha7))
            FSM_Manager.SetTrigger(FSM_LAYER.USERSTORY, TRANS_PARAM_ID.TRIGGER_GAMEOVER);
    }
}