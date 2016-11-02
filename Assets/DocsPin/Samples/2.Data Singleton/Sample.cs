using UnityEngine;
using System.Collections;

public class Sample : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		// test_docs data.
		{
			int price = TestDocs.price("object_01");
			int limit = TestDocs.limit("object_01");
			string name = TestDocs.names("object_01");
			string title = TestDocs.title("object_01");
			string content = TestDocs.content("object_01");
			Debug.Log(string.Format("Price:{0}, Limit:{1}, Name:{2}, Title:{3}, Content:{4}",
			                        price, limit, name, title, content));
		}

		// _gd_sample_01 data.
		{
			int gender = DataSample01.gender("object_01");
			int price = DataSample01.price("object_01");
			int limit = DataSample01.limit("object_01");
			ArrayList reward = DataSample01.reward("object_01");
			double length = DataSample01.length("object_01");
			string rewardString = "";
			if(reward != null)
			{
				foreach(object r in reward)
				{
					rewardString += string.Format("{0} ", r);
				}
			}
			Debug.Log(string.Format("Gender:{0},Price:{1},Limit:{2},Reward:[{3}],Length:{4}",
			                        gender, price, limit, rewardString, length));
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

		int price = TestDocs.price("object_01");
		GUILayout.Label("    -> Price : " + price.ToString());
		int limit = TestDocs.limit("object_01");
		GUILayout.Label("    -> LIMIT : " + limit.ToString());
		string name = TestDocs.names("object_01");
		GUILayout.Label("    -> NAME : " + name);
		string title = TestDocs.title("object_01");
		GUILayout.Label("    -> TITLE : " + title);
		string content = TestDocs.content("object_01");
		GUILayout.Label("    -> CONTENT : " + content);
	}
	private void drawSample01()
	{
		GUILayout.Label("***** _gd_sample_01 *******");
		
		int gender = DataSample01.gender("object_01");
		GUILayout.Label("    -> Gender : " + gender.ToString());
		int price = DataSample01.price("object_01");
		GUILayout.Label("    -> Price : " + price.ToString());
		int limit = DataSample01.limit("object_01");
		GUILayout.Label("    -> Limit : " + limit.ToString());
		ArrayList reward = DataSample01.reward("object_01");
		double length = DataSample01.length("object_01");
		GUILayout.Label("    -> Length : " + length.ToString());
		
		string rewardString = "";
		if(reward != null)
		{
			foreach(object r in reward)
			{
				rewardString += string.Format("{0} ", r);
			}
		}
		GUILayout.Label("    -> Reward : " + rewardString);
	}
}
