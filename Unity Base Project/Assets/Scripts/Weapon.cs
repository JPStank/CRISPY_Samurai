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
	void Update()
	{
		//if (input.getkeydown(keycode.alpha1))
		//{
		//	debug.log(animtable[0, 0]);
		//	debug.log(animtable[(int)puppetscript.state.idle, (int)puppetscript.state.idle]);
		//}
		//else if (input.getkeydown(keycode.alpha2))
		//{
		//	debug.log(animtable[0, 1]);
		//	debug.log(animtable[(int)puppetscript.state.idle, (int)puppetscript.state.moving]);
		//}


		BoxCollider[] cols = gameObject.GetComponents<BoxCollider>();
		//foreach (BoxCollider bc in cols)
		//{
		//	bc.enabled = owner.canHit;
		//}
		//for (int i = 0; i < cols.Length; i++)
		//{
		//	cols[i].enabled = owner.canHit;
		//}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Weapon")
		{
			Weapon temp = other.gameObject.GetComponent<Weapon>();
			if (temp)
			{
				//owner.canHit = false;
				Instantiate(sparkEffect, other.contacts[0].point, Quaternion.identity);
			}
		}
		if ((other.transform.tag == "Player"
			|| other.transform.tag == "Enemy"))
		{
            //owner.canHit = false;
			if (other.gameObject.GetComponentInChildren<Weapon>() != null)
			{
				owner.ResolveHit(other.gameObject);
				if (other.gameObject.GetComponent<PuppetScript>() != null)
					other.gameObject.GetComponent<PuppetScript>().ResolveHit(owner.gameObject);
			}
		}
	}

	PuppetScript.State GetState()
	{
		return owner.curState;
	}
}
