using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public PuppetScript owner;
	public GameObject sparkEffect;

	//damage and other goodies

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

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Weapon")
		{
			Instantiate(sparkEffect, other.contacts[0].point, Quaternion.identity);
		}
	}

	PuppetScript.State GetState()
	{
		return owner.curState;
	}
}
