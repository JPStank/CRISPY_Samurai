using UnityEngine;
using System.Collections;

public class Win_Loss : MonoBehaviour
{
	public int numToKill;
	public string winScreen, loseScreen;
	static Win_Loss myself;
	// Use this for initialization
	void Start()
	{
		myself = this;
	}
	
	// Update is called once per frame
	//void Update () 
	//{
	//
	//}

	public static Win_Loss instance
	{
		get
		{
			return instance;
		}
	}
	
	public void Decrement()
	{
		numToKill--;
		if (numToKill <= 0)
		{
			if (winScreen == "")
				Application.LoadLevel("Menu_Scene");
			else
				Application.LoadLevel(winScreen);
		}
	}

	public void YouLose()
	{
		if (loseScreen == "")
			Application.LoadLevel("Hub_Scene");
		else
			Application.LoadLevel(loseScreen);
	}
}
