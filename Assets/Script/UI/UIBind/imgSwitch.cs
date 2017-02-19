using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class imgSwitch : UIBind<bool>
{
    Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public override void OnDataChange()
    {
        if (bindedData.Value)
            img.color = Color.white;
        else
            img.color = Color.black;
    }

}
