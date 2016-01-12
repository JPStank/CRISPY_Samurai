using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public PuppetScript owner;
	public GameObject sparkEffect;
	
	public string[,] animTable;
	//damage and other goodies

	// Use this for initialization
	void Start()
	{
		//animTable = new string[,] { { "00", "01", "02" }, { "10", "11", "12" }, { "20", "21", "22" } };
		animTable = new string[18, 18];
		animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.IDLE] = "IDLE";
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log(animTable[0, 0]);
			Debug.Log(animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.IDLE]);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Debug.Log(animTable[0, 1]);
			Debug.Log(animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.MOVING]);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Weapon")
		{
			Instantiate(sparkEffect, other.contacts[0].point, Quaternion.identity);
			owner.ResolveHit(other.gameObject.GetComponent<Weapon>().owner.curState);
		}
	}

	PuppetScript.State GetState()
	{
		return owner.curState;
	}
}
