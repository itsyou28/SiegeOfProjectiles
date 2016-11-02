using UnityEngine;
using System.Collections;

// ### Method 2.
// Use Hacking event observer.
// 1) Inherit esLibs.Detector.Util.IHackEvent
// 2) Attach Observer. (in Start function)
// 3) Implement interface function. - void onAttackHacking(esLibs.esHashtable data)
// 4) Detach Observer. (in OnDestroy function)


public class DetectEvent2 : MonoBehaviour, esLibs.Detector.Util.IHackEvent
{

	// Use this for initialization
	void Start()
	{
		// Attach memory hacking event observer.
		esLibs.Detector.Util.mHackEvent.attachObserver(this);
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	void OnDestroy()
	{
		// Detach memory hacking event observer.
		esLibs.Detector.Util.mHackEvent.detachObserver(this);
	}

	// IHackEvent interface.
	public void onAttackHacking(esLibs.esHashtable data)
	{
		if(data == null)
			return;
		string message = data.get<string>(esLibs.Detector.Util.mHackEvent.KEY_MESSAGE);
		Debug.LogWarning(message);
	}
}
