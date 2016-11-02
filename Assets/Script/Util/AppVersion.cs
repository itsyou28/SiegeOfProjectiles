using UnityEngine;
using System.Collections;

[System.Serializable]
public class Version
{
    public int ProjectID = 0;
    public string ProjectName = "";
    public int MajorVersion = 0;
    public int MasterAndroidVersion = 0;
    public int MasterIOSVersion = 0;
    public int BuildVersion = 0;

    public string GA_propertyID;
    public string GA_bundleID;

    public string android_license_key;
}

public static class AppVersion 
{
    static Version m_Ver = null;

    static void Load()
    {
        try
        {
            m_Ver = FileManager.ResourceLoad("Data/version") as Version;
        }
        catch
        {
            m_Ver = new Version();
        }        
    }

    public static string GetVersion()
    {
        if(m_Ver == null)
            Load();
#if UNITY_EDITOR 
        return "Ver." + 
            m_Ver.MajorVersion.ToString() + "." +
            m_Ver.MasterAndroidVersion.ToString() + "." +
            m_Ver.MasterIOSVersion.ToString() + "." +
            m_Ver.BuildVersion.ToString();
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        return "Ver." +
            m_Ver.MajorVersion.ToString() + "." +
            m_Ver.MasterAndroidVersion.ToString() + "." +
            m_Ver.BuildVersion.ToString();
#endif
#if UNITY_IOS && !UNITY_EDITOR
        return "Ver." +
            m_Ver.MajorVersion.ToString() + "." +
            m_Ver.MasterIOSVersion.ToString() + "." +
            m_Ver.BuildVersion.ToString();
#endif

    }

    public static int Major
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.MajorVersion;
        }
    }

    public static int MasterAndroid
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.MasterAndroidVersion;
        }
    }

    public static int MasterIOS
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.MasterIOSVersion;
        }
    }

    public static int build
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.BuildVersion;
        }
    }
    
    public static string propertyID
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.GA_propertyID;
        }
    }

    public static string bundleID
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.GA_bundleID;
        }
    }

    public static string android_license_key
    {
        get
        {
            if (m_Ver == null)
                Load();

            return m_Ver.android_license_key;
        }
    }
}
