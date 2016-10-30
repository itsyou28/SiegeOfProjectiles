using UnityEngine;
using System.Collections;


// ### Method 1.
// Use Inspector "Hacking Event" from DocsPin.Detector.DocsDetector

public class DetectEvent : MonoBehaviour
{
	public UnityEngine.UI.Text _message = null;

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	public void attackMemoryHacking(esLibs.esHashtable data)
	{
		if(data == null)
			return;
		//data.printTable("Hacking.....");
		if(this._message != null)
		{
			this._message.text = data.get<string>(esLibs.Detector.Util.mHackEvent.KEY_MESSAGE);
		}
	}
}
