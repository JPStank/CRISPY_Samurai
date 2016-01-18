using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
	public List<GameObject> targets;

	[SerializeField]
	PuppetScript owner;

	// Use this for initialization
	void Start()
	{
		gameObject.layer = owner.gameObject.layer;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Attack()
	{
		foreach (GameObject victim in targets)
		{
			if (victim.GetComponent<PuppetScript>() != null)
			{
				if (victim.GetComponent<PuppetScript>().curState != PuppetScript.State.DEAD)
				{
					victim.GetComponent<PuppetScript>().ResolveHit(owner.curState);
					owner.ResolveHit(victim.GetComponent<PuppetScript>().curState);
				}
			}
		}

		for(int i = 0; i < targets.Count; i++)
		{
			if (targets[i].GetComponent<PuppetScript>().curState == PuppetScript.State.DEAD)
				targets.Remove(targets[i]);
		}
	}

	// Sam: not sure how to quite make this work
	//void OnTriggerStay(Collider other)
	//{
	//
	//}

	// Sam: this might have issues
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy"
			|| other.gameObject.tag == "Player")
		{
			if (!targets.Contains(other.gameObject))
				targets.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy"
			|| other.gameObject.tag == "Player")
		{
			if (targets.Contains(other.gameObject))
				targets.Remove(other.gameObject);
		}
	}
}
