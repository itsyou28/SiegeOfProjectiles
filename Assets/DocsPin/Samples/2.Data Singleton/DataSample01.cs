using UnityEngine;
using System.Collections;

public class DataSample01 : DocsPin.DocsDataSingleton<DataSample01>
{
	private const string GENDER = "gender";
	private const string PRICE = "price";
	private const string LIMIT = "limit";
	private const string REWARD = "reward";
	private const string LENGTH = "length";

	#region GameObject Interface
	// Use this for initialization
	void Start()
	{}
	// Update is called once per frame
	void Update()
	{}
	#endregion

	#region Data Interface
	// Getting gender.
	public static int gender(string key)
	{
		return DataSample01.instance.getGender(key);
	}
	public int getGender(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return 0;
		return this.get<int>(key, GENDER);
	}

	// Getting price.
	public static int price(string key)
	{
		return DataSample01.instance.getPrice(key);
	}
	public int getPrice(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return 0;
		return this.get<int>(key, PRICE);
	}

	// Getting limit.
	public static int limit(string key)
	{
		return DataSample01.instance.getLimit(key);
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
		return DataSample01.instance.getReward(key);
	}
	public static int reward(string key, int index)
	{
		return DataSample01.instance.getReward(key, index);
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

	// Getting length.
	public static double length(string key)
	{
		return DataSample01.instance.getLength(key);
	}
	public double getLength(string key)
	{
		if(string.IsNullOrEmpty(key) == true)
			return 0;
		return this.get<double>(key, LENGTH);
	}
	#endregion
}
