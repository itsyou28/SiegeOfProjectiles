using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UI_ID
{
    Skill1, 
    Skill2,
    Skill3
}

public class UIBinder : MonoBehaviour
{
    private static UIBinder instance = null;
    public static UIBinder Inst
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIBinder>();

            return instance;
        }
    }

    Dictionary<UI_ID, SkillUI> dicUI = new Dictionary<UI_ID, SkillUI>();

    [SerializeField]
    UI_ID[] arrUIID;
    [SerializeField]
    SkillUI[] arrSkillUI;   

    void Awake()
    {
        for(int idx=0; idx<arrUIID.Length; idx++)
        {
            dicUI.Add(arrUIID[idx], arrSkillUI[idx]);
        }
    } 

    public void Bind<T>(Bindable<T> data, UI_ID targetUI)
    {
        if(typeof(T) == typeof(float))
        {
            data.valueChanged += dicUI[targetUI].OnDataChange;
            dicUI[targetUI].SetData(data as Bindable<float>);
        }
    }
}

public class Bindable<T>
{
    private T value;

    public event deleFunc valueChanged;

    public T Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            OnValueChange();
        }
    }
    
    void OnValueChange()
    {
        if (valueChanged != null)
            valueChanged();
    }
}
