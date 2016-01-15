using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class GUI : MonoBehaviour
{
	public Canvas theGUI;
	public Image guardLeft, guardRight, guardTop, balance;
	private PuppetScript player;

	private List<string> dances;

	bool action = true;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PuppetScript>();

		dances[0] = "Twerk";
		dances[1] = "Gangnam Style";
		dances[2] = "Robot";
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

		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.DOWN) && !action)
		{
			action = true;
			theGUI.enabled = !theGUI.enabled;
		}
		else if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.LEFTSTICK_CLICK, InputChecker.BUTTON_STATE.UP))
		{
			action = false;
		}

		if (InputChecker.GetButton(InputChecker.PLAYER_NUMBER.ONE, InputChecker.CONTROLLER_BUTTON.BUMPER_L, InputChecker.BUTTON_STATE.DOWN))
		{
			//action = true;
			player.ChangeState(PuppetScript.State.DANCE);
			player.animation.Play(dances[Random.Range(0, dances.Count)]);
			//GameObject.FindGameObjectWithTag("Player").GetComponent<Animation>().Play("Twerk");
		}
	}
}
