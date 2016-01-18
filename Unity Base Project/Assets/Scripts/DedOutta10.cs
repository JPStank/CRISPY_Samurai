using UnityEngine;
using System.Collections;

public class DedOutta10 : MonoBehaviour
{
	PuppetScript player;
	float timer = 0.0f;
	bool timerStarted = false;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player && player.curState == PuppetScript.State.DEAD && timerStarted == false)
		{
			timer = 5.0f;
			timerStarted = true;
		}
		if (timerStarted)
		{
			timer -= Time.deltaTime;
		}
		if (timer < 0.0f)
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
