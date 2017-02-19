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

public class EmptyInput : iInput
{
    public void OnDown(Vector3 hitPos) { }
    public void OnDrag(Vector3 hitPos) { }
    public void OnPressUpdate() { }
    public void OnUp(Vector3 hitPos) { }
    
    public bool isPress { get; set; }
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
    iInput iEmpty;

    const float adjustX = -5;
    Vector3 raycastResult;

    void Awake()
    {
        iNormalMode = normalMode.GetComponent<iInput>();
        iMeteoMode = meteoMode.GetComponent<iInput>();
        iObstacleMode = obstacleMode.GetComponent<iInput>();
        iEmpty = new EmptyInput();

        curInputTarget = iNormalMode;

        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_ACTIVE_METEO, OnActiveMeteo);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_DEACTIVE_METEO, OnDeactiveMeteo);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_ACTIVE_OBSTACLE, OnActiveObstacle);
        BK_EMC.Inst.AddEventCallBackFunction(BK_EVENT.SKILL_DEACTIVE_OBSTACLE, OnDeactiveObstacle);
    }

    void OnActiveMeteo(params object[] args)
    {
        curInputTarget.isPress = false;
        curInputTarget = iMeteoMode;
    }

    void OnDeactiveMeteo(params object[] args)
    {
        curInputTarget.isPress = false;
        curInputTarget = iEmpty;
    }

    void OnActiveObstacle(params object[] args)
    {
        curInputTarget.isPress = false;
        curInputTarget = iObstacleMode;
    }

    void OnDeactiveObstacle(params object[] args)
    {
        curInputTarget.isPress = false;
        curInputTarget = iEmpty;
    }

    public void OnPointerDown(PointerEventData _data)
    {
        if (curInputTarget == iEmpty)
            curInputTarget = iNormalMode;

        curInputTarget.isPress = true;

        raycastResult = _data.pointerCurrentRaycast.worldPosition;
        raycastResult.x += adjustX;
        curInputTarget.OnDown(raycastResult);
    }

    public void OnPointerUp(PointerEventData _data)
    {
        raycastResult = _data.pointerCurrentRaycast.worldPosition;
        raycastResult.x += adjustX;
        curInputTarget.OnUp(raycastResult);
        curInputTarget.isPress = false;
    }

    public void OnDrag(PointerEventData _data)
    {
        raycastResult = _data.pointerCurrentRaycast.worldPosition;
        raycastResult.x += adjustX;
        curInputTarget.OnDrag(raycastResult);
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