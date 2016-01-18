using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
	//public List<GameObject> targets;
	LinkedList<GameObject> targets;

	[SerializeField]
	PuppetScript owner;

	// Use this for initialization
	void Start()
	{
		targets = new LinkedList<GameObject>();
		gameObject.layer = owner.gameObject.layer;
	}

	// Update is called once per frame
	//void Update()
	//{
	//
	//}

	public void Attack()
	{
		foreach(GameObject victim in targets)
		{
			if(victim != null)
			{
				if(victim.GetComponent<PuppetScript>() != null)
				{
					victim.GetComponent<PuppetScript>().ResolveHit(owner.curState);
					owner.ResolveHit(victim.GetComponent<PuppetScript>().curState);
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
				targets.AddLast(other.gameObject);
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
		targets.AddLast(obj);
	}

	public void RemoveFromList(GameObject obj)
	{
		targets.Remove(obj);
	}
}
