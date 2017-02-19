using UnityEngine;
using System.Collections;

public class UIBind<T> : MonoBehaviour
{
    protected Bindable<T> bindedData;

    public void SetData(Bindable<T> data)
    {
        bindedData = data;
    }

    public virtual void OnDataChange()
    {
    }
}
