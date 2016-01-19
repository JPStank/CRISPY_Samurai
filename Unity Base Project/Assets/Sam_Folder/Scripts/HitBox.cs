using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
	public List<GameObject> targets;
	List<GameObject> addList;
	//LinkedList<GameObject> targets;
	//public GameObject[] targets;
	//public float range;

	[SerializeField]
	PuppetScript owner;

	// Use this for initialization
	void Start()
	{
		targets = new List<GameObject>();
		addList = new List<GameObject>();
		gameObject.layer = owner.gameObject.layer;
	}

	// Update is called once per frame
	void Update()
	{
		foreach (GameObject obj in addList)
		{
			if (!targets.Contains(obj))
				targets.Add(obj);
		}
		//targets = addList;
		addList.Clear();
	}

	public void Attack()
	{
		foreach (GameObject victim in targets)
		{
			if (victim != null)
			{
				if (victim.GetComponent<PuppetScript>() != null)
				{
					victim.GetComponent<PuppetScript>().ResolveHit(owner.curState);
					owner.ResolveHit(victim.GetComponent<PuppetScript>().curState);
				}
			}
		}

		targets.Clear();
	}

	// Sam: not sure how to quite make this work
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Enemy"
			|| other.gameObject.tag == "Player")
		{
			if (!addList.Contains(other.gameObject))
			{
				if(other.gameObject.GetComponent<PuppetScript>())
				{
					if(other.gameObject.GetComponent<PuppetScript>().curState != PuppetScript.State.DEAD)
						addList.Add(other.gameObject);
				}
			}
		}
	}

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
		//addList.Add(obj);
	}

	public void RemoveFromList(GameObject obj)
	{
		//if(addList.Contains(obj))
		//	addList.Remove(obj);
	}
}
