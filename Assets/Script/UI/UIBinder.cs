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

    Dictionary<UI_ID, SkillUI> dicUIList;

    [SerializeField]
    UI_ID[] arrUI_ID;
    [SerializeField]
    SkillUI[] arrSkillUI;

    void Awake()
    {
        dicUIList = new Dictionary<UI_ID, SkillUI>();

        for(int idx=0; idx<arrUI_ID.Length; idx++)
        {
            dicUIList.Add(arrUI_ID[idx], arrSkillUI[idx]);
        }
    }
}
