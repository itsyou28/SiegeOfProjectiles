using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("resources")]
public class AndroidString
{
    [XmlElement("app_name")]
    public string app_name;
}

[System.Serializable]
public class BuildData
{
    public int ProjectID;

    public string BundlePrefix;
    public string CompanyName;
    public string ProductName;
    public string GA_PropertyID;

    public string ProductName_Default;
    public string ProductName_KO;
    public string ProductName_EN;
    public string ProductName_JP;

    public string KeyStorePW;
    public string AliasPW;

    public string Android_License_Key;

    public BuildData()
    {
        ProjectID = 0;

        BundlePrefix = "";
        CompanyName = "";
        ProductName = "";
        GA_PropertyID = "";

        ProductName_Default = "";
        ProductName_EN = "";
        ProductName_KO = "";
        ProductName_JP = "";

        KeyStorePW = "";
        AliasPW = "";

        Android_License_Key = "";
    }
}

[System.Serializable]
public class LocalbuildData
{
    public string buildPath;

    public LocalbuildData()
    {
        buildPath = "";
    }
}


public class Builder : EditorWindow
{
    static string[] SCENES;// = FindEnabledEditorScenes();

    static BuildData buildData;
    static LocalbuildData localBuildData;

    static string dev_bundleIdentifier;
    static string pub_bundleIdentifier;

    [MenuItem("Build/BuildWindow")]
    static void ShowWindow()
    {
        Initialize();

        EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        EditorWindow.GetWindow<Builder>();
    }

    static string filename = "";
    static string strNewPrj = "";


    private static void Initialize()
    {

        VersionManage.Load();

        LoadBuildData();

        if (FileManager.CheckFileExists("BuildData/", "localbuilddata.bytes"))
            localBuildData = FileManager.FileLoad("BuildData/", "localbuilddata.bytes") as LocalbuildData;

        if (localBuildData == null)
            localBuildData = new LocalbuildData();

        if (buildData != null)
        {
            VersionManage.SelectProject(buildData.ProjectID);
            MakeBundleIdentifier();
        }
        else
            Debug.LogError("builddata is null"); ;
    }

    void OnEnable()
    {
        SCENES = FindEnabledEditorScenes();
    }

    private static void LoadBuildData()
    {
        filename = "builddata_" + VersionManage.ProjectName + ".bytes";

        if (FileManager.CheckFileExists("BuildData/", filename))
            buildData = FileManager.FileLoad("BuildData/", filename) as BuildData;

        if (buildData == null)
            buildData = new BuildData();
    }
    

    void OnDestroy()
    {
        SaveBuildData();
    }

    private static void SaveBuildData()
    {
        if (buildData != null && buildData.CompanyName != null && buildData.CompanyName != "")
        {
            FileManager.FileSave("BuildData/", filename, buildData);
            FileManager.FileSave("BuildData/", "localbuilddata.bytes", localBuildData);
        }
    }

    void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        GUILayout.Label("NewProject : ", GUILayout.Width(80));
        strNewPrj = GUILayout.TextField(strNewPrj);
        if (GUILayout.Button("Add", GUILayout.Width(80)))
        {
            ServerConnect connect = new ServerConnect();

            WWWForm wform = new WWWForm();
            wform.AddField("prjName", strNewPrj);

            connect.Download("http://likethisserver.cloudapp.net/VersionServer/", "AddProject.php", wform, null);
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (GUILayout.Button("Refresh"))
        {
            Initialize();
        }

        if (!VersionManage.isLoadComplete || buildData == null)
        {
            GUILayout.Label("Refresh Version 버튼을 눌러주세요");
            return;
        }

        GUILayout.BeginHorizontal();
        VersionManage.SelectedProjectID = EditorGUILayout.IntPopup(VersionManage.SelectedProjectID,
            VersionManage.arrPrjName, VersionManage.arrPrjID);

        if (GUILayout.Button("Save", GUILayout.Width(80)))
            SaveBuildData();

        if (GUI.changed)
        {
            VersionManage.SelectProject(VersionManage.SelectedProjectID);

            LoadBuildData();

            buildData.ProjectID = VersionManage.SelectedProjectID;
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("BundlePrefix : ", GUILayout.Width(160));
        buildData.BundlePrefix = EditorGUILayout.TextField(buildData.BundlePrefix);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("CompanyName : ", GUILayout.Width(160));
        buildData.CompanyName = EditorGUILayout.TextField(buildData.CompanyName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("ProductName : ", GUILayout.Width(160));
        buildData.ProductName = EditorGUILayout.TextField(buildData.ProductName);
        GUILayout.EndHorizontal();

        if (GUI.changed)
            MakeBundleIdentifier();

        GUILayout.Label("Bundle Identifier : " + dev_bundleIdentifier);

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.Label("ProductName - Default : ", GUILayout.Width(160));
        buildData.ProductName_Default = EditorGUILayout.TextField(buildData.ProductName_Default);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("ProductName - KO : ", GUILayout.Width(160));
        buildData.ProductName_KO = EditorGUILayout.TextField(buildData.ProductName_KO);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("ProductName - EN : ", GUILayout.Width(160));
        buildData.ProductName_EN = EditorGUILayout.TextField(buildData.ProductName_EN);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("ProductName - JP : ", GUILayout.Width(160));
        buildData.ProductName_JP = EditorGUILayout.TextField(buildData.ProductName_JP);
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.Label("GA PropertyID: ", GUILayout.Width(160));
        buildData.GA_PropertyID = EditorGUILayout.TextField(buildData.GA_PropertyID);
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Android License Key: ", GUILayout.Width(160));
        buildData.Android_License_Key = EditorGUILayout.TextField(buildData.Android_License_Key);
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.Label("KeyStorePW : ", GUILayout.Width(160));
        buildData.KeyStorePW = GUILayout.PasswordField(buildData.KeyStorePW, '*');
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("AliasPW : ", GUILayout.Width(160));
        buildData.AliasPW = GUILayout.PasswordField(buildData.AliasPW, '*');
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        GUILayout.Label("Major : " + VersionManage.iMajor.ToString() +
        " | MasterAndroid : " + VersionManage.iMasterAndroid.ToString() +
        " | Build : " + VersionManage.iBuild.ToString());

        GUILayout.Space(15);

        GUILayout.Label("AndroidVer : " + VersionManage.GetAndroidVersionString());

        GUILayout.Space(15);

        GUILayout.BeginHorizontal();
        GUILayout.Label("BuildPath : ", GUILayout.Width(100));
        localBuildData.buildPath = EditorGUILayout.TextField(localBuildData.buildPath);
        GUILayout.EndHorizontal();

        GUILayout.Label("deploy Path : " + localBuildData.buildPath);
        GUILayout.Label("develop Path : " + localBuildData.buildPath + "develop");
        GUILayout.Label("master Path : " + localBuildData.buildPath + "master");
        GUILayout.Label("master Path : " + localBuildData.buildPath + "publish");

        GUILayout.Space(30);

        if (GUILayout.Button("Increase Major Version"))
            VersionManage.UpdateVersion(BUILD_TYPE.IncreaseMajor);

        GUILayout.Space(30);

        if (GUILayout.Button("Build Master Android"))
            SwitchBuild(BUILD_OPTION.MASTER_ANDROID);

        GUILayout.Space(50);

        if (GUILayout.Button("Build Master Android Current Version"))
            SwitchBuild(BUILD_OPTION.MASTER_ANDROID_CUR);

        if (GUILayout.Button("Deploy Android Device"))
            SwitchBuild(BUILD_OPTION.DEPLOY_ANDROID);

        if (GUILayout.Button("Deploy Android Device Current Version"))
            SwitchBuild(BUILD_OPTION.DEPLOY_ANDROID_CUR);
    }

    private static void MakeBundleIdentifier()
    {
        dev_bundleIdentifier = buildData.BundlePrefix + "." + buildData.CompanyName + "." + buildData.ProductName;
    }

    enum BUILD_OPTION
    {
        MASTER_ANDROID,
        MASTER_ANDROID_CUR,
        DEVELOP_ANDROID,
        DEVELOP_ANDROID_CUR,
        DEPLOY_ANDROID,
        DEPLOY_ANDROID_CUR,
        PUBLISH_ANDROID,
        DEPLOY_PUBLISH_ANDROID
    }

    static void SwitchBuild(BUILD_OPTION op)
    {
        try
        {
            switch (op)
            {
                case BUILD_OPTION.MASTER_ANDROID:
                    Build_Master_Android();
                    break;
                case BUILD_OPTION.MASTER_ANDROID_CUR:
                    Build_Master_Android_Current_Version();
                    break;
                case BUILD_OPTION.DEVELOP_ANDROID:
                    Build_Develop_Android();
                    break;
                case BUILD_OPTION.DEVELOP_ANDROID_CUR:
                    Build_Develop_Android_Current_Version();
                    break;
                case BUILD_OPTION.DEPLOY_ANDROID:
                    Deploy_Android_Device();
                    break;
                case BUILD_OPTION.DEPLOY_ANDROID_CUR:
                    Deploy_Android_Device_Current_Version();
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    static void Deploy_Android_Device()
    {
        VersionManage.UpdateVersion(BUILD_TYPE.BuildDeployDevelop);
        AndroidCommonSetting();

        GenericBuild(
            SCENES,
            localBuildData.buildPath + "DeployTest_ver" + VersionManage.GetAndroidVersionString() + ".apk",
            BuildTarget.Android,
            BuildOptions.AutoRunPlayer);
    }

    static void Deploy_Android_Device_Current_Version()
    {
        VersionManage.Load();
        AndroidCommonSetting();

        GenericBuild(
            SCENES,
            localBuildData.buildPath + "DeployTest_ver" + VersionManage.GetAndroidVersionString() + ".apk",
            BuildTarget.Android,
            BuildOptions.AutoRunPlayer);
        //BuildOptions.AutoRunPlayer | BuildOptions.Development | BuildOptions.AllowDebugging);
    }

    static void Build_Develop_Android()
    {
        VersionManage.UpdateVersion(BUILD_TYPE.BuildDevelop);

        AndroidCommonSetting();

        GenericBuild(
            SCENES,
            localBuildData.buildPath + "develop/" + VersionManage.ProjectName + VersionManage.GetAndroidVersionString() + ".apk",
            BuildTarget.Android,
            BuildOptions.None);
    }

    static void Build_Develop_Android_Current_Version()
    {
        AndroidCommonSetting();

        GenericBuild(
            SCENES,
            localBuildData.buildPath + "develop/" + VersionManage.ProjectName + VersionManage.GetAndroidVersionString() + ".apk",
            BuildTarget.Android,
            BuildOptions.None);
    }

    static void Build_Master_Android()
    {
        VersionManage.UpdateVersion(BUILD_TYPE.BuildDevelop);
        VersionManage.UpdateVersion(BUILD_TYPE.BuildMasterAndroid);

        AndroidCommonSetting();

        GenericBuild(
            SCENES,
            localBuildData.buildPath + "master/" + VersionManage.ProjectName + VersionManage.GetAndroidVersionString() + ".apk",
            BuildTarget.Android,
            BuildOptions.None);
    }

    static void Build_Master_Android_Current_Version()
    {
        AndroidCommonSetting();

        GenericBuild(
            SCENES,
            localBuildData.buildPath + "master/" + VersionManage.ProjectName + VersionManage.GetAndroidVersionString() + ".apk",
            BuildTarget.Android,
            BuildOptions.None);
    }
    
    static void AndroidCommonSetting()
    {
        SaveProductNameToXML(null, buildData.ProductName_Default);
        SaveProductNameToXML("values-en", buildData.ProductName_EN);
        SaveProductNameToXML("values-ko", buildData.ProductName_KO);
        SaveProductNameToXML("values-jp", buildData.ProductName_JP);

        PlayerSettings.companyName = buildData.CompanyName;
        PlayerSettings.productName = buildData.ProductName;
        PlayerSettings.bundleVersion = VersionManage.GetAndroidVersionString();
        PlayerSettings.bundleIdentifier = dev_bundleIdentifier;
        PlayerSettings.Android.bundleVersionCode = VersionManage.iMasterAndroid;

        Version tVer = (Version)BK_Function.DeepCopy(VersionManage.GetCurrerntVersion());

        tVer.GA_propertyID = buildData.GA_PropertyID;
        tVer.GA_bundleID = PlayerSettings.bundleIdentifier;
        tVer.android_license_key = buildData.Android_License_Key;

        Debug.LogWarning("AndroidCommonSetting. bundleVersion : " + VersionManage.GetVersionString());

        FileManager.FileSave("Resources/Data/", "version.bytes", tVer);

        AssetDatabase.Refresh();

        PlayerSettings.Android.keystorePass = buildData.KeyStorePW;
        PlayerSettings.Android.keyaliasPass = buildData.AliasPW;
    }

    static void BeforeBuild()
    {
    }

    static void FinishBuild()
    {
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        if (res.Length > 0)
        {
            throw new System.Exception("BuildPlayer failure: " + res);
        }
    }

    static void SaveProductNameToXML(string targetLang, string name)
    {
        string[] lines = {
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
            "<resources>",
            "<string name=\"app_name\">",
            "</resources>"
        };

        lines[2] += name + "</string>";

        if (string.IsNullOrEmpty(targetLang))
            FileManager.FileSave("Plugins/Android/res/values", "string.xml", lines);
        else
            FileManager.FileSave("Plugins/Android/res/" + targetLang, "string.xml", lines);
    }
}
