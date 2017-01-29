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

    public float CurrentCharge
    {
        get { return chargeImg.fillAmount; }
        set { chargeImg.fillAmount = value; }
    }

    //public bool Is
    
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
