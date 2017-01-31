using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImgFill : UIBind<float>
{
    Image fillImg;

    void Awake()
    {
        fillImg = GetComponent<Image>();
    }

    public override void OnDataChange()
    {
        fillImg.fillAmount = 1 - bindedData.Value;
    }

}
