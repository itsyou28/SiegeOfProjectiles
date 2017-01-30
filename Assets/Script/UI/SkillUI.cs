using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillUI : MonoBehaviour
{
    [SerializeField]
    BK_EVENT activeEvent;
    [SerializeField]
    BK_EVENT deactiveEvent;

    [SerializeField]
    GameObject activeImg;

    [SerializeField]
    Image chargeImg;

    Bindable<float> bindData;

    public void SetData(Bindable<float> data)
    {
        bindData = data;
    }

    public void OnDataChange()
    {
        chargeImg.fillAmount = bindData.Value;
    }
        
    void Awake()
    {
        BK_EMC.Inst.AddEventCallBackFunction(activeEvent, OnActive);
        BK_EMC.Inst.AddEventCallBackFunction(deactiveEvent, OnDeactive);
    }

    void OnActive(params object[] args)
    {
        activeImg.SetActive(true);
    }

    void OnDeactive(params object[] args)
    {
        activeImg.SetActive(false);
    }
}
