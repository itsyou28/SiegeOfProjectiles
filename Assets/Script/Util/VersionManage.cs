using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;


public enum BUILD_TYPE
{
    none,
    IncreaseMajor,
    BuildMasterAndroid,
    BuildMasterIOS,
    BuildDevelop,
    BuildDeployDevelop
}

public static class VersionManage 
{
    public static int SelectedProjectID = 1;

    public static string ProjectName;
    public static int iMajor = 0;
    public static int iMasterAndroid = 0;
    public static int iMasterIOS = 0;
    public static int iBuild = 0;

    public static bool isLoadComplete = false;

    const string verServerURL = "http://likethisserver.cloudapp.net/VersionServer/";
    const float timeout = 100.0f;

    static float timeoutChk = 0;

    public static Dictionary<int, Version> prjVerList = new Dictionary<int, Version>();
    public static int[] arrPrjID;
    public static string[] arrPrjName;

    public static void Load()
    {        
        string url = verServerURL +"getVersion.php";

        using (WWW www = new WWW(url))
        {
            timeoutChk = Time.realtimeSinceStartup;

            while (!www.isDone)
            {
                if (Time.realtimeSinceStartup - timeoutChk > timeout)
                {
                    Debug.LogError(timeoutChk.ToString() + "TimeOut. Check VersionServer");
                }
            }

            if (www.error != null)
            {
                Debug.LogError(www.error);
                return;
            }

            Version[] tempList = JsonMapper.ToObject<Version[]>(www.text);
            arrPrjID = new int[tempList.Length];
            arrPrjName = new string[tempList.Length];
            prjVerList.Clear();

            int i = 0;

            foreach(Version t in tempList)
            {
                arrPrjID[i] = t.ProjectID;
                arrPrjName[i] = t.ProjectName;
                prjVerList.Add(t.ProjectID, t);

                i++;
            }
            
            SelectProject(SelectedProjectID);

            isLoadComplete = true;

            Debug.Log("version Load complete From Server");
        }
    }

    public static void SelectProject(int prjID)
    {
        if(prjVerList.ContainsKey(prjID))
        {
            for(int idx=0; idx<arrPrjID.Length; idx++)
            {
                if (prjID == arrPrjID[idx])
                    ProjectName = arrPrjName[idx];
            }

            iMajor = prjVerList[prjID].MajorVersion;
            iMasterAndroid = prjVerList[prjID].MasterAndroidVersion;
            iMasterIOS = prjVerList[prjID].MasterIOSVersion;
            iBuild = prjVerList[prjID].BuildVersion;

            SelectedProjectID = prjID;
        }
    }

    public static void UpdateVersion(BUILD_TYPE bType)
    {
        isLoadComplete = false;

        string url = verServerURL + "manager.php";

        WWWForm wForm = new WWWForm();
        wForm.AddField("prjID", SelectedProjectID.ToString());
        wForm.AddField("buildType", bType.ToString());

        WWW www = new WWW(url, wForm);

        timeoutChk = Time.realtimeSinceStartup;

        while (!www.isDone)
        {
            if (Time.realtimeSinceStartup - timeoutChk > timeout)
            {
                Debug.LogError(timeoutChk.ToString() + "TimeOut. Check VersionServer");
            }
        }

        Debug.Log("Update result : " + www.text);

        Load();
    }

    public static string GetVersionString()
    {
        return iMajor.ToString() + "." + iMasterAndroid.ToString() + "." + iMasterIOS.ToString() + "." + iBuild.ToString();
    }

    public static string GetAndroidVersionString()
    {
        return iMajor.ToString() + "." + iMasterAndroid.ToString() + "." + iBuild.ToString();
    }

    public static string GetIOSVersionString()
    {
        return iMajor.ToString() + "." + iMasterIOS.ToString() + "." + iBuild.ToString();
    }

    public static Version GetCurrerntVersion()
    {
        return prjVerList[SelectedProjectID];
    }
}
