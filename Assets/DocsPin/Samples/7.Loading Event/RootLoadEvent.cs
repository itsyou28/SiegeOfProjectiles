using UnityEngine;
using System.Collections;

public class RootLoadEvent : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	#region Root Loading Event.
	public void onLoadStart(int totalCount, ArrayList files)
	{
		Debug.LogWarning("-------LOADING START--------");
		string fileList = "";
		foreach(DocsPin.DocsData file in files)
		{
			fileList += file.documentName + ", ";
		}
		Debug.Log("[Total] " + totalCount.ToString() + " : [Files] " + fileList);
	}
	public void onLoading(int totalCount, int index,
	                      DocsPin.DocsData loadedFile, DocsPin.DocsData willLoadFile)
	{
		string loaded = "NULL";
		if(loadedFile != null)
			loaded = loadedFile.documentName;
		string will = "NULL";
		if(willLoadFile != null)
			will = willLoadFile.documentName;
		string message = string.Format("        ** Loading *****    [Total] {0}, [INDEX] {1}, [Prev] {2}, [Next] {3}",
		                               totalCount, index, loaded, will);
		Debug.Log(message);
	}
	public void onLoadEnd(int totalCount, ArrayList successFiles, ArrayList failFiles)
	{
		string success = "";
		foreach(DocsPin.DocsData file in successFiles)
		{
			success += file.documentName + ", ";
		}
		string fail = "";
		foreach(DocsPin.DocsData file in failFiles)
		{
			fail += file.documentName + ", ";
		}
		string message = string.Format("[Total] {0}, [Success] {1}, [Fail] {2}",
		                               totalCount, successFiles.Count, failFiles.Count);
		Debug.Log(message);
		if(failFiles.Count > 0)
		{
			Debug.Log("     [Success] " + success);
			Debug.LogWarning("     [Fail] " + fail);
		}
		Debug.LogWarning("=======LOADING END=======");
	}
	#endregion
}
