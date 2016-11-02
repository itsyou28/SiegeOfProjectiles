#define DebugFileManger

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public static class FileManager
{
    ///<summary>
    ///ResourceLoad 함수는 파일 경로에 슬래쉬를 사용한다. 
    ///지원하지 않는 파일 타입일 경우 실패한다.
    ///직렬화 이진 파일을 저장하고 불러올 경우 ".bytes" 확장자를 사용해야 한다. 
    ///</summary>    
    public static object ResourceLoad(string fileName)
    {
        object result = null;
        
        TextAsset textAsset = Resources.Load(fileName) as TextAsset;
        if (textAsset != null)
        {
            MemoryStream stream = new MemoryStream(textAsset.bytes);
            BinaryFormatter bf = new BinaryFormatter();
            result = bf.Deserialize(stream);

            stream.Close();

#if DebugFileManger
            Debug.Log("Resource Load Success " + fileName);
#endif
        }
        else
        {
            Debug.Log("Resource Load Failed " + fileName);
            Exception e = new Exception(fileName + " Load Failed. textAsset is null");
            throw e;
        }

        return result;
    }

    public static string GetFileStorePath(string fileName = null)
    {
        string path = null;

#if UNITY_EDITOR
        path = Application.dataPath;
#elif UNITY_ANDROID && !UNITY_EDITOR
        path = Application.persistentDataPath;
#elif UNITY_IOS && !UNITY_EDITOR
        path = Application.persistentDataPath;
		path += "/Documents";
#endif
        if(!string.IsNullOrEmpty(fileName))
            path = Path.Combine(path, fileName);

        return path;
    }

    public static bool CheckFileExists(string filePath, string fileName)
    {
        if (filePath == null)
            filePath = GetFileStorePath(fileName);
        else
            filePath = GetFileStorePath(Path.Combine(filePath, fileName));

        if (File.Exists(filePath))
            return true;
        else
        {
            Debug.LogWarning("No file : " + filePath);
            return false;
        }
    }

    public static bool CheckFileExists(string filePath)
    {
        filePath = GetFileStorePath(filePath);

        if (File.Exists(filePath))
            return true;
        else
        {
            Debug.LogWarning("No file : " + filePath);
            return false;
        }
    }

    public static string[] GetFileList(string filePath)
    {
        string targetPath = GetFileStorePath(filePath);

        if (Directory.Exists(targetPath))
            return Directory.GetFiles(targetPath);

        Debug.LogWarning(targetPath + " is not exists.");

        return null;
    }

    public static string[] GetFileListOrderBy(string filePath)
    {
        string targetPath = GetFileStorePath(filePath);

        if (!Directory.Exists(targetPath))
            return null;

        DirectoryInfo info = new DirectoryInfo(targetPath);
        FileInfo[] files = info.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

        string[] result = new string[files.Length];
        
        for(int idx = 0; idx<files.Length; idx++)
        {
            result[idx] = files[idx].Name;
        }

        return result;
    }

    public static bool DeleteFile(string filePath)
    {
        if(CheckFileExists(filePath))
        {
            File.Delete(GetFileStorePath(filePath));

            return true;
        }

        return false;
    }

    public static object FileLoad(string filePath, string fileName)
    {
        if (filePath == null)
            filePath = GetFileStorePath(fileName);
        else
            filePath = GetFileStorePath(Path.Combine(filePath, fileName));
        
        object result = null;

        FileStream fs = null; 

        try
        {
            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();//binaryFormatter 생성
            result = bf.Deserialize(fs);
            
#if DebugFileManger
            if (result != null)
                Debug.Log("File Load Success " + fileName);
            else
            {
                Debug.Log("File Load Failed " + fileName);
                Exception e = new Exception("File Load Failed");
                throw e;
            }
#endif
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw e;
        }
        finally
        {
            if(fs != null)
                fs.Close();
        }

        return result;
    }

    ///<summary>
    ///FileStream 함수는 파일 경로에 역슬래쉬를 사용한다. 
    ///</summary>    
    public static void FileSave(string filePath, string fileName, object saveObject)
    {
        //Make FileDestination
        if (filePath == null)
            filePath = GetFileStorePath(fileName);
        else
        {
            DirectoryInfo di = new DirectoryInfo(GetFileStorePath(filePath));

            if (di.Exists == false)
                di.Create();

            filePath = GetFileStorePath(Path.Combine(filePath, fileName));
        }

        //filePath = Path.Combine(filePath, fileName);

        FileStream fs = null;

        try
        {
            //파일스트림 생성, 파일이 있으면 오픈, 없으면 생성
            //에디터 전용 데이터 저장
            fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();//binaryFormatter 생성
            bf.Serialize(fs, saveObject);//serialize(객체, 파일스트림)

#if DebugFileManger
            Debug.Log("File Save to " + filePath);
#endif
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            if (fs != null)
                fs.Close();//스트림 닫기        
        }
    }

    public static void FileSave(string filePath, string fileName, string[] stringLines)
    {
        if (filePath == null)
            filePath = GetFileStorePath(fileName);
        else
        {
            DirectoryInfo di = new DirectoryInfo(GetFileStorePath(filePath));

            if (di.Exists == false)
                di.Create();

            filePath = GetFileStorePath(Path.Combine(filePath, fileName));
        }

        try
        {
            System.IO.File.WriteAllLines(filePath, stringLines);

#if DebugFileManger
            Debug.Log("string lines file save to " + filePath);
#endif
        }
        catch(Exception e)
        {
            throw e;
        }
    }
}