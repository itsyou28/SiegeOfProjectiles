using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    UIBind<float>[] arrCharge;
    UIBind<bool>[] arrEnable;

    void Awake()
    {
        arrCharge = GetComponentsInChildren<UIBind<float>>();
        Debug.Log(arrCharge.Length);
        arrEnable = GetComponentsInChildren<UIBind<bool>>();
    }
    
    public void Bind(Bindable<float> data, int idx)
    {
        data.valueChanged += arrCharge[idx].OnDataChange;
        arrCharge[idx].SetData(data);
    }

    public void Bind(Bindable<bool> data, int idx)
    {
        data.valueChanged += arrEnable[idx].OnDataChange;
        arrEnable[idx].SetData(data);
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
