using UnityEngine;
using System.Collections;

public class CollectErrMsg
{
    static CollectErrMsg instance = null;
    public static CollectErrMsg Inst
    {
        get
        {
            if (instance == null)
                instance = new CollectErrMsg();

            return instance;
        }
    }

    string appName = "redphant_appbook";
    private string clientID;
    private string appVersion;
    
    CollectErrMsg()
    {
        appVersion = AppVersion.GetVersion();
        clientID = SystemInfo.deviceUniqueIdentifier;
    }

    public void Collect(string errMsg)
    {
        var url = "http://likethisserver.cloudapp.net/CollectErrorMsg/collect.php?"
            + "bundleID=" + appName
            + "&clientID=" + WWW.EscapeURL(clientID)
            + "&appVersion=" + appVersion
            + "&ErrMsg=" + WWW.EscapeURL(errMsg);

        WWW request = new WWW(url);

        if (request.error == null)
        {
            Debug.Log("send collect errmsg");
            return;
        }
        else
            Debug.LogError(request.error);
    }
}
