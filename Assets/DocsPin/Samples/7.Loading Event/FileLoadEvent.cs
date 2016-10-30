using UnityEngine;
using System.Collections;

public class FileLoadEvent : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	#region File Loading Event.
	public void onLoadStart(DocsPin.DocsData data)
	{
		if(data == null)
			return;
		Debug.LogWarning("******************* START LOADING FILE : " + data.name);
	}
	public void onLoadSuccess(DocsPin.DocsData data)
	{
		if(data == null)
			return;
		Debug.LogWarning("******************* SUCCESS LOADING FILE : " + data.name);
	}
	public void onLoadFail(DocsPin.DocsData data)
	{
		if(data == null)
			return;
		Debug.LogError("******************* FAIL LOADING FILE : " + data.name);
	}
	#endregion
}
