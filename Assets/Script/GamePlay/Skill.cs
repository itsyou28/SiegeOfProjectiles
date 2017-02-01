using UnityEngine;
using System.Collections;

public class Skill
{
    public bool isActive { get; private set; } //현재 Skill 사용중 여부
    public bool isEnable { get { return enable.Value; } } //Skill 사용 가능 여부

    bool isCharging = false; //스킬이 활성화되면 충전이 중지된다. 

    int maxNumber;  //Skill 최대 충전시 사용할 수 있는 Skill 횟수
    float maxTime;  //Skill 최대 충전에 걸리는 시간

    float oneCost;              //Skill 1회 사용 비용
    float chargeSpeed;          //Skill 충전속도
    Bindable<float> curCharge;  //Skill 현재 충전량
    Bindable<bool> enable;      //가능한 사용량을 모두 사용했을 경우 false로 변경

    BK_EVENT activeMsg;
    BK_EVENT deactiveMsg;

    public Skill(int maxNumber, float maxTime, int targetUiIdx, BK_EVENT activeMsg, BK_EVENT deactiveMsg)
    {
        curCharge = new Bindable<float>();
        enable = new Bindable<bool>();

        this.activeMsg = activeMsg;
        this.deactiveMsg = deactiveMsg;

        LevelUp(maxNumber, maxTime);
        UIBinder.Inst.Bind(curCharge, targetUiIdx);
        UIBinder.Inst.Bind(enable, targetUiIdx);

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
        if (!enable.Value)
            return;

        curCharge.Value -= oneCost;

        if (curCharge.Value < oneCost)
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