using UnityEngine;
using System.Collections;

public class AttackRange : MonoBehaviour
{

	public GameObject Enemy;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter(Collision col)
	{
		Enemy.GetComponent<AI_Controller>().SetRange(true);
	}

	void OnCollisionExit(Collision col)
	{
		Enemy.GetComponent<AI_Controller>().SetRange(false);
	}
}
