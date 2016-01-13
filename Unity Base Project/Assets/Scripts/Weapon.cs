using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public PuppetScript owner;
	public GameObject sparkEffect;
	
	//public string[,] animTable;
	//damage and other goodies

	// Use this for initialization
	//void Start()
	//{
	//	
	//}

	// Update is called once per frame
	//void Update()
	//{
		//if(Input.GetKeyDown(KeyCode.Alpha1))
		//{
		//	Debug.Log(animTable[0, 0]);
		//	Debug.Log(animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.IDLE]);
		//}
		//else if (Input.GetKeyDown(KeyCode.Alpha2))
		//{
		//	Debug.Log(animTable[0, 1]);
		//	Debug.Log(animTable[(int)PuppetScript.State.IDLE, (int)PuppetScript.State.MOVING]);
		//}
	//}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Weapon")
		{
			Instantiate(sparkEffect, other.contacts[0].point, Quaternion.identity);
		}
		else if (other.transform.tag == "Player"
			|| other.transform.tag == "Enemy")
		{
			if (other.gameObject.GetComponentInChildren<Weapon>() != null)
			{
				owner.ResolveHit(other.gameObject.GetComponentInChildren<Weapon>().owner.curState);
				if (other.gameObject.GetComponent<PuppetScript>() != null)
					other.gameObject.GetComponent<PuppetScript>().ResolveHit(owner.curState);
			}
		}
	}

	PuppetScript.State GetState()
	{
		return owner.curState;
	}
}
