using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using LitJson;

public delegate void CallBackDownloadString(string str);

public class ServerConnect
{    
    bool isComplete = false;

    string jsonResult;

    CallBackDownloadString callBackDownload = null;

    public void Download(string url, string targetpage, WWWForm wform, CallBackDownloadString callback)
    {
        callBackDownload = callback;

        PostRequest(url+targetpage, wform, OnDownload);
    }

    static void PostRequest(string url, WWWForm wform, UploadDataCompletedEventHandler callBackFunc)
    {
        Debug.Log("PostRequest : " + url);

        byte[] data = new byte[0];

        if (wform != null)
            data = wform.data;

        System.Uri uri = new System.Uri(url);

        WebClient wc = new WebClient();

        wc.UploadDataCompleted += callBackFunc;
        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        wc.UploadDataAsync(uri, "POST", data);
    }

    void OnDownload(System.Object sender, UploadDataCompletedEventArgs e)
    {
        byte[] result = e.Result;
        jsonResult = Encoding.UTF8.GetString(result);

        isComplete = true;
    }
    
    public void Update()
    {
        if(isComplete)
        {
            callBackDownload(jsonResult);
            isComplete = false;
        }
    }
}
