using UnityEngine;
using System.Collections;

public class LangMng : MonoBehaviour
{
    public static readonly SystemLanguage[] SurportLanguage =
    {
        SystemLanguage.Korean,
        SystemLanguage.Japanese
    };

    public static readonly SystemLanguage globalDefaultLang = SystemLanguage.Korean;

    static SystemLanguage curLang = globalDefaultLang;
    static int curLangArrIdx = 0;

    public static SystemLanguage CurLang { get { return curLang; } }
    public static int CurLangArrIdx { get { return curLangArrIdx; } }

    void Awake()
    {
        //SetLanguage(Application.systemLanguage);
        SetLanguage(SystemLanguage.Japanese);
    }

    void SetLanguage(SystemLanguage lang)
    {
        curLangArrIdx = GetLangArrID(lang);

        if (curLangArrIdx != -1)
            curLang = lang;
        else
        {
            curLangArrIdx = GetLangArrID(globalDefaultLang);
            curLang = globalDefaultLang;
        }
    }

    public static int GetLangArrID(SystemLanguage lang)
    {
        for(int idx=0; idx<SurportLanguage.Length; idx++)
        {
            if (lang == SurportLanguage[idx])
                return idx;
        }

#if UNITY_EDITOR
        Debug.LogError("GetLangArrID. Ask Language is Not Surport." + lang.ToString()); 
#endif

        return -1;
    }
}
