using UnityEngine;
using System.Collections;

public class MultiLangText
{
    private static MultiLangText instance = null;
    public static MultiLangText Inst
    {
        get
        {
            if (instance == null)
                instance = new MultiLangText();

            return instance;
        }
    }

    public static string filePath = "TextData";
    public static string filename = "MultiLangTextData";

    string[][] languageText;

    private MultiLangText()
    {
        languageText = FileManager.ResourceLoad(filePath+"/"+filename) as string[][];
    }

    public string GetText(int idx)
    {
#if UNITY_EDITOR
        if (idx >= languageText.Length || languageText[idx] == null)
            Debug.LogError("No Multi Language Text Data. idx : " + idx); 
#endif

        return languageText[idx][LangMng.CurLangArrIdx];
    }
}
