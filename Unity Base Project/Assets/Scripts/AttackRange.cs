using UnityEngine;
using System.Collections;

public class AttackRange : MonoBehaviour
{

	public AI_Controller controller;

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
		controller.SetRange(true);
	}

	void OnCollisionExit(Collision col)
	{
		controller.SetRange(false);
	}
}
