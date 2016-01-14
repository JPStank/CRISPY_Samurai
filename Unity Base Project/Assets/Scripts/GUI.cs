using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
	public Image guardLeft, guardRight, guardTop, balance;
	private PuppetScript player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (player.curState == PuppetScript.State.GRD_LEFT)
			guardLeft.color = Color.red;
		else
			guardLeft.color = Color.white;

		if (player.curState == PuppetScript.State.GRD_TOP)
			guardTop.color = Color.red;
		else
			guardTop.color = Color.white;

		if (player.curState == PuppetScript.State.GRD_RIGHT)
			guardRight.color = Color.red;
		else
			guardRight.color = Color.white;

		balance.fillAmount = player.curBalance / player.maxBalance;
	}
}
