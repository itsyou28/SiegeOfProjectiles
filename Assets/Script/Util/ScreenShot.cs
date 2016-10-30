using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour
{
    string filepath;

    void Awake()
    {
        filepath = FileManager.GetFileStorePath();
        filepath = filepath.Replace("/Assets", "");
        filepath += "/ScreenShot/";

        Debug.Log("ScreenShot Path : " + filepath);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F10))
        {
            string filename = Screen.width.ToString() + "_" + Screen.height.ToString() + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss")+".png";            

            Application.CaptureScreenshot(filepath + filename);

            Debug.LogWarning("ScreenShot : " +filepath + filename);
        }
    }
}
