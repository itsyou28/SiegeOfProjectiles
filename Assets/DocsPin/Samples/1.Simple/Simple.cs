using UnityEngine;
using System.Collections;

public class Simple : MonoBehaviour
{
	void Awake()
	{
	}
	// Use this for initialization
	void Start()
	{
		// test_docs data.
		{
			DocsPin.DocsData data = DocsPin.DocsRoot.findData("test_docs");
			if(data != null)
			{
				int price = data.get<int>("object_01", "price");
				int limit = data.get<int>("object_01", "limit");
				string name = data.get<string>("object_01", "name");
				string title = data.get<string>("object_01", "title");
				string content = data.get<string>("object_01", "content");
				Debug.Log(string.Format("Price:{0}, Limit:{1}, Name:{2}, Title:{3}, Content:{4}",
				                        price, limit, name, title, content));
			}
		}

		// _gd_sample_01 data.
		{
			DocsPin.DocsData data = DocsPin.DocsRoot.findData("_gd_sample_01");
			if(data != null)
			{
				int gender = data.get<int>("object_01", "gender");
				int price = data.get<int>("object_01", "price");
				int limit = data.get<int>("object_01", "limit");
				ArrayList reward = data.get<ArrayList>("object_01", "reward");
				double length = data.get<int>("object_01", "length");

				string rewardString = "";
				foreach(object r in reward)
				{
					rewardString += string.Format("{0} ", r);
				}
				Debug.Log(string.Format("Gender:{0},Price:{1},Limit:{2},Reward:[{3}],Length:{4}",
				                        gender, price, limit, rewardString, length));
			}
		}
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	void OnGUI()
	{
		GUILayout.BeginVertical();
		//GUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			this.changeLanguage();
			GUILayout.Space(10f);
			this.drawTestDocs();
			GUILayout.Space(10f);
			this.drawSample01();
			GUILayout.FlexibleSpace();
		}
		//GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	// Change lanuage during runtime.
	private void changeLanguage()
	{
		GUILayout.Label("***** [ALL] Change language during runtime. *******");
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("English", GUILayout.Width(100)) == true)
		{
			DocsPin.DocsRoot.reloadAll(SystemLanguage.English);
		}
		if(GUILayout.Button("Korean", GUILayout.Width(100)) == true)
		{
			DocsPin.DocsRoot.reloadAll(SystemLanguage.Korean);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(10f);

		GUILayout.Label("***** [Each] Change language during runtime. *******");
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("English(test_docs)", GUILayout.Width(200)) == true)
		{
			// By docs name.
			DocsPin.DocsRoot.reloadData(SystemLanguage.English, "test_docs");
			// By docs ID.
			//DocsPin.DocsRoot.reloadDataById(SystemLanguage.English, "1-T6HKz-GiHfv6lCcGvOnvCshPD0CVkHNRylQUAedJio");
		}
		if(GUILayout.Button("Korean(test_docs)", GUILayout.Width(200)) == true)
		{
			// By docs name.
			DocsPin.DocsRoot.reloadData(SystemLanguage.Korean, "test_docs");
			// By docs ID.
			//DocsPin.DocsRoot.reloadDataById(SystemLanguage.Korean, "1-T6HKz-GiHfv6lCcGvOnvCshPD0CVkHNRylQUAedJio");
		}
		GUILayout.EndHorizontal();
	}

	private void drawTestDocs()
	{
		GUILayout.Label("***** TestDocs *******");

		DocsPin.DocsData data = DocsPin.DocsRoot.findData("test_docs");
		if(data == null)
			return;
		int price = data.get<int>("object_01", "price");
		GUILayout.Label("    -> Price : " + price.ToString());
		int limit = data.get<int>("object_01", "limit");
		GUILayout.Label("    -> LIMIT : " + limit.ToString());
		string name = data.get<string>("object_01", "name");
		GUILayout.Label("    -> NAME : " + name);
		string title = data.get<string>("object_01", "title");
		GUILayout.Label("    -> TITLE : " + title);
		string content = data.get<string>("object_01", "content");
		GUILayout.Label("    -> CONTENT : " + content);
	}
	private void drawSample01()
	{
		GUILayout.Label("***** _gd_sample_01 *******");

		DocsPin.DocsData data = DocsPin.DocsRoot.findData("_gd_sample_01");
		if(data == null)
			return;

		int gender = data.get<int>("object_01", "gender");
		GUILayout.Label("    -> Gender : " + gender.ToString());
		int price = data.get<int>("object_01", "price");
		GUILayout.Label("    -> Price : " + price.ToString());
		int limit = data.get<int>("object_01", "limit");
		GUILayout.Label("    -> Limit : " + limit.ToString());
		ArrayList reward = data.get<ArrayList>("object_01", "reward");
		double length = data.get<int>("object_01", "length");
		GUILayout.Label("    -> Length : " + length.ToString());

		string rewardString = "";
		foreach(object r in reward)
		{
			rewardString += string.Format("{0} ", r);
		}
		GUILayout.Label("    -> Reward : " + rewardString);
	}
}
