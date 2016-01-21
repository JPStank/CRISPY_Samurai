using UnityEngine;
using System.Collections;

public class MyStance : MonoBehaviour 
{
	public SpriteRenderer top, left, right;

	// Use this for initialization
	//void Start () 
	//{
	//
	//}
	
	// Update is called once per frame
	//void Update () 
	//{
	//
	//}

	public void SetTopRed()
	{
		top.color = Color.red;
	}

	public void SetLeftRed()
	{
		left.color = Color.red;

	}

	public void SetRightRed()
	{
		right.color = Color.red;

	}

	public void ClearTop()
	{
		top.color = Color.white;
	}

	public void ClearLeft()
	{
		left.color = Color.white;

	}

	public void ClearRight()
	{
		right.color = Color.white;

	}

	public void Clear()
	{
		top.color = Color.white;
		left.color = Color.white;
		right.color = Color.white;
	}
}
