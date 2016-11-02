using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoogleAnalytics : MonoBehaviour
{
    public static GoogleAnalytics instance;

    private string propertyID;
    private string bundleID;
    private string appName;

    private string screenRes;
    private string clientID;
    private string appVersion;

    void Awake()
    {
        if (instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    void Start()
    {
        appVersion = AppVersion.GetVersion();

        propertyID = AppVersion.propertyID;
        bundleID = AppVersion.bundleID;
        appName = "likethis_doBaseball";

        screenRes = Screen.width + "x" + Screen.height;

        clientID = SystemInfo.deviceUniqueIdentifier;

    }

    public void Collect(string logdata)
    {

        logdata = WWW.EscapeURL(logdata);

        var url = "http://www.google-analytics.com/collect?"
            + "v=1"
            + "&ul=" + GetApplicationSystemLanguage().ToString()
            + "&t=appview"
            + "&sr=" + screenRes
            + "&an=" + WWW.EscapeURL(appName)
            + "&a=448166238"
            + "&tid=" + propertyID
            + "&aid=" + bundleID
            + "&cid=" + WWW.EscapeURL(clientID)
            + "&_u=.sB"
            + "&av=" + appVersion
            + "&_v=ma1b3"
            + "&cd=" + logdata
            + "&qt=2500"
            + "&z=185";

        WWW request = new WWW(url);

        if (request.error == null)
            return;
        else
            Debug.LogError(request.error);

        #region original error check code
        //if(request.error == null)
        //{
        //    if (request.responseHeaders.ContainsKey("STATUS"))
        //    {
        //        if (request.responseHeaders["STATUS"] == "HTTP/1.1 200 OK")	
        //        {
        //            Debug.Log ("GA Success");
        //        }else{
        //            Debug.LogWarning(request.responseHeaders["STATUS"]);	
        //        }
        //    }else{
        //        Debug.LogWarning("Event failed to send to Google");	
        //    }
        //}else{
        //    Debug.LogWarning(request.error.ToString());	
        //}
        #endregion

    }
    
    static SystemLanguage GetApplicationSystemLanguage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // bugfix for Unity 4.3
        AndroidJavaClass localeClass = new AndroidJavaClass("java/util/Locale");
        AndroidJavaObject defaultLocale = localeClass.CallStatic<AndroidJavaObject>("getDefault");
        AndroidJavaObject usLocale = localeClass.GetStatic<AndroidJavaObject>("US");
        string systemLanguage = defaultLocale.Call<string>("getDisplayLanguage", usLocale);
        SystemLanguage code;
        try {
            code = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), systemLanguage);
        } catch {
            Debug.Log("Failed Get System Language");
            code = SystemLanguage.Unknown;
        }  
#else
        SystemLanguage code = Application.systemLanguage;
#endif
        return code;
    }
}