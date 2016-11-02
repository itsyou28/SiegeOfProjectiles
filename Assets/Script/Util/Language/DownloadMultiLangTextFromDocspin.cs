using UnityEngine;
using System.Collections;

public class DownloadMultiLangTextFromDocspin : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SetMultiLangTextData());
    }

    IEnumerator SetMultiLangTextData()
    {
        yield return true;

        DocsPin.DocsData docsdata = DocsPin.DocsRoot.findData("LikeThis_Golf_MultiLanguageText");

        string[][] targetData = new string[docsdata.getRowCount()][];

        int surportLangCount = LangMng.SurportLanguage.Length;
        int idx = 0;
        string strLog;

        foreach (string rowid in docsdata.getRowKeyList())
        {
            idx = docsdata.get<int>(rowid, "id");
            targetData[idx] = new string[surportLangCount];
            strLog = rowid + " // " + idx + " // ";

            foreach (SystemLanguage lang in LangMng.SurportLanguage)
            {
                targetData[idx][LangMng.GetLangArrID(lang)] = docsdata.get<string>(rowid, lang.ToString());
                strLog += targetData[idx][LangMng.GetLangArrID(lang)] + " // ";
            }

            Debug.Log(strLog);
            yield return true;
        }

        FileManager.FileSave("Resources/" + MultiLangText.filePath + "/", MultiLangText.filename + ".bytes", targetData);
    }

}
