using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImportStageData : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Import());
    }

    IEnumerator Import()
    {
        yield return true;

        DocsPin.DocsData docsdata = DocsPin.DocsRoot.findData("SOP_Stage");

        Dictionary<int, Dictionary<int, PhaseSet>> dataBuffer = new Dictionary<int, Dictionary<int, PhaseSet>>();
        int idx = 0;
        
#region GoogleDocs에서 스테이지 데이터를 가져온다.

        foreach (string rowid in docsdata.getRowKeyList())
        {
            idx = docsdata.get<int>(rowid, "id");

            int stageIdx = docsdata.get<int>(rowid, "StageIdx");
            int phaseIdx = docsdata.get<int>(rowid, "PhaseIdx");
            int enemyType = docsdata.get<int>(rowid, "EnemyType");
            int limitQuantity = docsdata.get<int>(rowid, "LimitQuantity");

            if (!dataBuffer.ContainsKey(stageIdx))
                dataBuffer.Add(stageIdx, new Dictionary<int, global::PhaseSet>());

            if (!dataBuffer[stageIdx].ContainsKey(phaseIdx))
                dataBuffer[stageIdx].Add(phaseIdx, new global::PhaseSet());

            dataBuffer[stageIdx][phaseIdx].AddData(new global::EnemyPhaseData(enemyType, limitQuantity));

            Debug.Log(rowid + " // " + stageIdx + " // " + phaseIdx + " // " + enemyType + " // " + limitQuantity);

            yield return true;
        } 
#endregion


#region 스테이지 데이터를 파일로 저장한다. 

        Dictionary<int, StageSet> targetData = new Dictionary<int, StageSet>();

        foreach (KeyValuePair<int, Dictionary<int, PhaseSet>> stagePair in dataBuffer)
        {
            int stageIdx = stagePair.Key;

            if (!targetData.ContainsKey(stageIdx))
                targetData.Add(stageIdx, new global::StageSet());

            //Phase 데이터를 순서대로 입력하기 위해 정렬한다. 
            List<int> forSort = new List<int>(stagePair.Value.Keys);
            forSort.Sort();

            //PhaseIdx를 기준으로 정렬된 데이터를 순서대로 파일로 저장할 데이터에 삽입한다. 
            foreach (int phaseIdx in forSort)
            {
                targetData[stageIdx].AddPhase(stagePair.Value[phaseIdx]);

                Debug.Log("AddPhase : " + stageIdx + " // " + phaseIdx);
            }
        }

        FileManager.FileSave("Resources/", "stageData.bytes", targetData);

#endregion

        yield return true;


        UnityEditor.EditorApplication.isPlaying = false;
    }
}
