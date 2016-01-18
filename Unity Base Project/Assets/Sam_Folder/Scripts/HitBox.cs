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
	//void Update()
	//{
	//
	//}

	public void Attack()
	{
		for (int i = 0; i < targets.Count; i++)
		{
			if (targets[i] != null)
			{
				if (targets[i].GetComponent<PuppetScript>() != null)
				{
					if (targets[i].GetComponent<PuppetScript>().curState != PuppetScript.State.DEAD)
					{
						targets[i].GetComponent<PuppetScript>().ResolveHit(owner.curState);
						owner.ResolveHit(targets[i].GetComponent<PuppetScript>().curState);
					}
				}
			}
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
			{
				targets.Add(other.gameObject);
				other.gameObject.GetComponent<PuppetScript>().SetOtherBox(this);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy"
			|| other.gameObject.tag == "Player")
		{
			if (targets.Contains(other.gameObject))
			{
				targets.Remove(other.gameObject);
				other.gameObject.GetComponent<PuppetScript>().RemoveOtherBox();
			}
		}
	}

	//Sam: shouldn't need to ever call this function
	public void AddToList(GameObject obj)
	{
		targets.Add(obj);
	}

	public void RemoveFromList(GameObject obj)
	{
		targets.Remove(obj);
	}
}
