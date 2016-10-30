using UnityEngine;
using System.Collections;

public class TestDocs : DocsPin.DocsDataSingleton<TestDocs>
{
	// Data Keys.
	private const string PRICE = "price";
	private const string NAME = "name";
	private const string LIMIT = "limit";
	private const string REWARD = "reward";
	// Localized Data Keys.
	private const string TITLE = "title";
	private const string CONTENT = "content";

	#region GameObject Interface
	// Use this for initialization
	void Start()
	{}
	// Update is called once per frame
	void Update()
	{}
	#endregion
	
	#region Data Interface
	// Getting price.
	public static int price(string key)
	{
		return TestDocs.instance.getPrice(key);
	}
	public int getPrice(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return 0;
		return this.get<int>(key, PRICE);
	}

	// Getting name.
	public static string names(string key)
	{
		return TestDocs.instance.getName(key);
	}
	public string getName(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return null;
		return this.get<string>(key, NAME);
	}

	// Getting limit.
	public static int limit(string key)
	{
		return TestDocs.instance.getLimit(key);
	}
	public int getLimit(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return 0;
		return this.get<int>(key, LIMIT);
	}
	
	// Getting reward list.
	public static ArrayList reward(string key)
	{
		return TestDocs.instance.getReward(key);
	}
	public static int reward(string key, int index)
	{
		return TestDocs.instance.getReward(key, index);
	}
	public ArrayList getReward(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return null;
		return this.get<ArrayList>(key, REWARD);
	}
	public int getReward(string key, int index)
	{
		ArrayList rewards = this.getReward(key);
		if(rewards == null || rewards.Count <= 0 || rewards.Count <= index)
			return 0;
		return (int)rewards[index];
	}
	#endregion


	#region Localize Data Interface
	// Getting title.
	public static string title(string key)
	{
		return TestDocs.instance.getTitle(key);
	}
	public string getTitle(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return null;
		return this.get<string>(key, TITLE);
	}

	// Getting content.
	public static string content(string key)
	{
		return TestDocs.instance.getContent(key);
	}
	public string getContent(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return null;
		return this.get<string>(key, CONTENT);
	}
	#endregion
}
